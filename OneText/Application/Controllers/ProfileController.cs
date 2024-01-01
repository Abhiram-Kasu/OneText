using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneText.Application.Database;
using OneText.Application.Models;
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

        var friends = from friendship in _db.Friendships
                      join user1 in _db.Users on friendship.User1Id equals user1.Id
                      join user2 in _db.Users on friendship.User2Id equals user2.Id
                      where friendship.User1Id == user.Id || friendship.User2Id == user.Id
                      select new
                      {
                          FriendshipId = friendship.Id,
                          Id = friendship.User1Id == user.Id ? friendship.User2Id : friendship.User1Id,
                          FirstName = friendship.User1Id == user.Id ? user2.FirstName : user1.FirstName,
                          LastName = friendship.User1Id == user.Id ? user2.LastName : user1.LastName
                      };

        return Ok(friends);
    }
    [HttpGet, Authorize]
    [Route("friends/search/{query}")]
    public async IAsyncEnumerable<LimitedUser> PeopleSearch(string query)
    {
        query = query.ToLower();

        var res = _db.Users.Where(u => u.FirstName.ToLower().Contains(query) || u.LastName.ToLower().Contains(query)).AsAsyncEnumerable();

        await foreach (var user in res)
        {
            yield return (LimitedUser)user;
        }
    }


}
