using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneText.Application.Database;
using OneText.Application.Services.Auth;
using OneText.Application.ViewModels;
using Spark.Library.Extensions;

namespace tstApi.Application.Controllers;
[Route("profile")]
public class ProfileController : Controller
{
    private readonly UsersService _usersService;
    private readonly DatabaseContext _db;
    private readonly AuthService _authService;

    public ProfileController(UsersService usersService, DatabaseContext db, AuthService authService)
    {
        _usersService = usersService;
        _db = db;
        _authService = authService;
    }

    [HttpGet, Authorize]
    [Route("")]
    public async Task<IActionResult> ProfileInfo()
    {
        var user = await _authService.GetAuthenticatedUser(User);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new { FirstName = user.FirstName, LastName = user.LastName, user.Email, user.EmailVerifiedAt });
    }

    [HttpPost, Authorize]
    [Route("edit")]
    public async Task<IActionResult> Update(ProfileInfoEditor request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var currentUser = await _authService.GetAuthenticatedUser(User);

        if (currentUser != null)
        {
            var existingUser = await _usersService.FindUserByEmailAsync(request.Email);

            if (existingUser != null && currentUser.Id != existingUser.Id)
            {
                ModelState.AddModelError("Email", "Email already in use.");
                return BadRequest(ModelState);
            }

            currentUser.Email = request.Email;
            currentUser.FirstName = request.Name;

            _db.Users.Save(currentUser);
        }

        return Ok("Profile updated!");
    }

    [HttpPost, Authorize]
    [Route("edit/password")]
    public async Task<IActionResult> UpdatePassword(ProfilePasswordEditor request)
    {
        var user = await _authService.GetAuthenticatedUser(User);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUser = await _usersService.FindUserAsync(user.Email, _usersService.GetSha256Hash(request.CurrentPassword));

        if (existingUser == null)
        {
            ModelState.AddModelError("CurrentPassword", "Current password was incorrect.");
            return BadRequest(ModelState);
        }

        existingUser.Password = _usersService.GetSha256Hash(request.NewPassword);

        _db.Users.Save(existingUser);

        return Ok("Password was updated");
    }

    [HttpGet, Authorize]
    [Route("/profile/friends")]
    public async Task<IActionResult> GetFriends()
    {
        var user = await _authService.GetAuthenticatedUser(User);
        var friendships = _db.Friendships.Where(x => x.User1Id == user.Id || x.User2Id == user.Id)
            .Select(x => new { FriendId = x.User1Id == user.Id ? x.User2Id : x.User1Id, FriendshipId = x.Id }).ToList();
        var friends = _db.Users.Where(x => friendships.Select(f => f.FriendId).Contains(x.Id))
            .Select(x => new { FirstName = x.FirstName, LastName = x.LastName, Id = x.Id, FriendshipId = friendships.First(f => f.FriendId == x.Id).FriendshipId }).ToList();
        return Ok(friends);
    }
}
