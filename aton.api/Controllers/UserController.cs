using aton.application.Contracts.User;
using aton.application.DTOs.Auth;
using aton.application.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aton.api.Controllers;

[Route("api/user")]
[ApiController]
[Authorize]
public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _service;
    public UserController(ILogger<UserController> logger,
                            IUserService service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [Authorize(Roles = "admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var requestBy = GetRequestUserLogin();
            var res = await _service.Register(dto, requestBy);
            if (!res.Flag)
                return BadRequest(res.Message);

            return Ok(res.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Ошибка регистрации");

            return Problem("Ошибка регистрации, попробуйте позже");
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var res = await _service.Login(dto);
            if(!res.Flag)
                return BadRequest(res.Message);

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка входа в систему");

            return Problem("Ошибка входа, попробуйте позже");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllActive()
    {
        try
        {
            var res = await _service.GetAllActive();
            if (!res.Any())
                return NotFound();

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения активных пользователей");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        try
        {
            var res = await _service.ChangePassword(dto, GetRequestUserLogin());
            if (!res.Flag)
                return BadRequest(res.Message);

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения операции изменения пароля");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }

    [HttpPost("change-login")]
    public async Task<IActionResult> ChangeLogin(ChangeLoginDto dto)
    {
        try
        {
            var res = await _service.ChangeLogin(dto, GetRequestUserLogin());
            if (!res.Flag)
                return BadRequest(res.Message);

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения операции изменения пароля");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserDto dto)
    {
        try
        {
            await _service.Update(dto, GetRequestUserLogin());

            return Ok("Пользователь успешно обновлён.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения операции обнвления пользоватлея");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string login)
    {
        try
        {
            var res = await _service.GetByLogin(login);
            if(res == null)
                return NotFound($"Пользователь с логином {login} не найден.");

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения операции получения пользователя по логину");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("filter")]
    public async Task<IActionResult> GetWithAgeFilter([FromQuery] DateTime minDateTime)
    {
        try
        {
            var res = await _service.GetAllWithAgeFilter(minDateTime);
            if(!res.Any())
                return NotFound();

            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения операции получения пользователей с фильтрацией по возрасту");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string login)
    {
        try
        {
            await _service.Delete(login, GetRequestUserLogin());

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения операции удаления пользователя");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPost("recovery")]
    public async Task<IActionResult> Recovery([FromQuery] string login)
    {
        try
        {
            await _service.Recovery(login);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения операции восстанолвения пользователя");

            return Problem("Ошибка. Попробуйте позже.");
        }
    }
}

