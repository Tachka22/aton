using Microsoft.AspNetCore.Mvc;

namespace aton.api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected string GetRequestUserLogin() =>
        User.Claims.FirstOrDefault(f => f.Type == "login").Value;
}
