using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OneText.Client.Models;
using OneText.Client.Services;
using SoapDispenser.Avalonia.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneText.Client.ViewModels;
public partial class HomeViewModel : ViewModelBase
{
    private readonly AuthService _authService = App.Services.GetService(typeof(AuthService)) as AuthService ?? throw new ArgumentNullException("Auth Service");
    private readonly ProfileService _profileService = App.Services.GetService(typeof(ProfileService)) as ProfileService?? throw new ArgumentNullException("Profile Service");
    public override string? Title => $"Welcome {_authService.AuthStateInformation.FirstName}!";

    public ObservableCollection<Friend> Friends { get; set; } = [];

    public HomeViewModel()
    {
        InitialLoadCommand.Execute(null);
    }
    [RelayCommand]
    public async Task InitialLoad()
    {
        await _profileService.LoadUser();
        _profileService.User.Friends.ForEach(Friends.Add);
    }
    
}
