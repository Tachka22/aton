using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using aton.application.Contracts.User;
using aton.application.DTOs.Auth;
using aton.application.DTOs.User;
using aton.application.Options;
using aton.domain.Entities;
using aton.domain.Interfaces;
using Mapster;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace aton.application.Services;

internal class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOptions<Jwt> _option;
    public UserService(IUnitOfWork unitOfWork,
                       IOptions<Jwt> options)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _option = options ?? throw new ArgumentNullException(nameof(options));

        MapsterConfiguration();
    }

    public async Task<GeneralResponce> ChangeLogin(ChangeLoginDto dto, string modifiedBy)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByLogin(dto.Login);
            if (user == null)
                return new GeneralResponce(false, "Пользователь не найден.");

            if (!IsActiveUser(user))
                return new GeneralResponce(false, "Пользователь заблокирован.");

            var isExistLogin = await _unitOfWork.UserRepository.GetByLogin(dto.NewLogin);
            if (isExistLogin != null)
                return new GeneralResponce(false, $"Логин {dto.NewLogin} уже существует.");

            user.Login = dto.NewLogin;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            await _unitOfWork.SaveChangesAsync();

            return new GeneralResponce(true, "Логин успешно обновлён.");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<GeneralResponce> ChangePassword(ChangePasswordDto dto, string modifiedBy)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByLogin(dto.Login);
            if (user == null)
                return new GeneralResponce(false, "Пользователь не найден.");

            if (!IsActiveUser(user))
                return new GeneralResponce(false, "Пользователь заблокирован.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            await _unitOfWork.SaveChangesAsync();

            return new GeneralResponce(true, "Пароль успешно обновлён.");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<GeneralResponce> Delete(string targetLogin, string requestLogin)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByLogin(targetLogin);
            if (user == null)
                return new GeneralResponce(false, $"Пользователь {targetLogin} не найден.");

            user.RevokedBy = requestLogin;
            user.RevokedOn = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return new GeneralResponce(true, $"Пользователь {targetLogin} успешно удалён.");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<GetUserDto>> GetAllActive()
    {
        var users = await _unitOfWork.UserRepository.GetAllActive();
        return users.Select(x => x.Adapt<GetUserDto>()).ToList();
    }

    public async Task<IEnumerable<GetUserDto>> GetAllWithAgeFilter(DateTime minDateTime)
    {
        var users =  await _unitOfWork.UserRepository.GetAllByAge(minDateTime);
        return users.Select(x => x.Adapt<GetUserDto>()).ToList();
    }
    public async Task<GetUserDto?> GetByLogin(string login)
    {
        var user = await _unitOfWork.UserRepository.GetByLogin(login);
        if (user == null)
            return null;

        var dto = user.Adapt<GetUserDto>();
        if (IsActiveUser(user))
            dto.IsActive = true;

        return dto;
    }

    public async Task<LoginResponce> Login(LoginDto dto)
    {
        var user = await _unitOfWork.UserRepository.GetByLogin(dto.Login);
        if (user == null)
            return new LoginResponce(false, "Пользователь не найден.");

        if (!IsActiveUser(user))
            return new LoginResponce(false, "Учётная запись заблокирована.");

        if (!CheckPassword(dto.Password, user.Password))
            return new LoginResponce(false, "Неверный логин или пароль");

        var token = GenerateJwtToken(user);

        return new LoginResponce(true, string.Empty, token);
    }

    public async Task<GeneralResponce> Recovery(string login)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByLogin(login);
            if (user == null)
                return new GeneralResponce(false, $"Пользователь {login} не найден.");

            user.RevokedOn = null;
            user.RevokedBy = string.Empty;

            await _unitOfWork.SaveChangesAsync();

            return new GeneralResponce(true, $"Пользователь {login} восстановлен.");
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    public async Task<GeneralResponce> Register(RegisterDto dto, string requestBy)
    {
        try
        {
            var isExist = await _unitOfWork.UserRepository.GetByLogin(dto.Login);
            if (isExist != null)
                return new GeneralResponce(false, "Пользователь с таким логином уже существует.");

            var newUser = new User()
            {
                Login = dto.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                Gender = dto.Gender,
                Birthday = dto.Birthday,
                Admin = dto.Admin,
                CreatedBy = requestBy,
                CreatedOn = DateTime.UtcNow,
                ModifiedBy = requestBy,
                ModifiedOn = DateTime.UtcNow
            };

            await _unitOfWork.UserRepository.Create(newUser);
            await _unitOfWork.SaveChangesAsync();

            return new GeneralResponce(true, $"Пользователь {newUser.Name} успешно создан");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<GeneralResponce> Update(UpdateUserDto dto, string modifiedBy)
    {
        try
        {
            var ext = await _unitOfWork.UserRepository.GetByLogin(dto.Login);
            if (ext == null)
                return new GeneralResponce(true, $"Пользователь {ext.Login} не найден");

            ext.Gender = dto.Gender;
            ext.Name = dto.Name;
            ext.Birthday = dto.Birthday;
            ext.ModifiedBy = modifiedBy;
            ext.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return new GeneralResponce(true, $"Пользователь {ext.Login} успешно обновлён");
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_option.Value.Key);

        var role = user.Admin ? "admin" : "user";
        var claims = new List<Claim>
        {
            new Claim("login", user.Login),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_option.Value.TokenValidityInDay),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _option.Value.Issuer,
            Audience = _option.Value.Audience
        };

        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
    private bool CheckPassword(string password, string passwordHash)
    {
        if(!BCrypt.Net.BCrypt.Verify(password, passwordHash))
            return false;

        return true;
    }

    private bool IsActiveUser(User user) => user.RevokedOn == null;

    private static void MapsterConfiguration()
    {
        TypeAdapterConfig<User, GetUserDto>
            .NewConfig()
            .Map(dest => dest.IsActive, src => src.RevokedOn == null);
    }
}
