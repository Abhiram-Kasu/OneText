using OneText.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OneText.Client.Services;
internal class ProfileService
{
    private readonly HttpClient _httpClient;
    private CompleteUser? _user;
    public CompleteUser User => _user;
    public ProfileService()
    {
        _httpClient = App.Services.GetService(typeof(HttpClient)) as HttpClient ?? throw new ArgumentNullException("Http Client not found");
    }

    public async Task LoadUser()
    {

        var userTask = _httpClient.GetFromJsonAsync<CompleteUser>("/profile");
        var friendsTask = _httpClient.GetFromJsonAsync<List<Friend>>("/profile/friends");

        await Task.WhenAll(userTask, friendsTask);

        _user = userTask.Result;
        _user!.Friends = friendsTask.Result;
    }

}
