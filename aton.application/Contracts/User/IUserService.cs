using aton.application.DTOs.Auth;
using aton.application.DTOs.User;

namespace aton.application.Contracts.User;

public interface IUserService
{
    Task<GeneralResponce> Register(RegisterDto dto,string requestBy);
    Task<LoginResponce> Login(LoginDto login);
    Task Update(UpdateUserDto dto, string modifiedBy);
    Task<GeneralResponce> ChangeLogin(ChangeLoginDto dto, string modifiedBy);
    Task<GeneralResponce> ChangePassword(ChangePasswordDto dto, string modifiedBy);
    Task<IEnumerable<string>> GetAllActive();
    Task<GetUserDto?> GetByLogin(string login);
    Task<IEnumerable<string>> GetAllWithAgeFilter(DateTime minDateTime);
    Task Delete(string targetLogin, string modifiedBy);
    Task Recovery(string login);
}
