using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneText.Application.Models;

namespace OneText.Application.Controllers;
public class AdminController : Controller
{
    [HttpGet]
    [Authorize(Policy = CustomRoles.Admin)]
    [Route("admin/dashboard")]
    public IActionResult Dashboard()
    {
        return Ok();
    }
}
