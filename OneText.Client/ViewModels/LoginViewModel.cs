using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OneText.Client.Models;
using OneText.Client.Services;
using SoapDispenser.Avalonia.Helpers.MVVM;
using SoapExtensions;

namespace OneText.Client.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly AuthService _authService;

    public LoginViewModel()
    {
        _authService = App.Services.GetService(typeof(AuthService)) as AuthService ?? throw new ArgumentNullException("Auth Service not found");
    }

    public override string? Title => "Login";
    
    [ObservableProperty]
    private string _username = string.Empty;
    [ObservableProperty]
    private string _password = string.Empty;

    public ObservableCollection<ValidationError> Errors { get; set; } = [];

    [RelayCommand]
    public async Task Login()
    {
        
        var res = await _authService.Login(new LoginModel { Email = Username, Password = Password });
        Errors.Clear();
        if(res)
        {
            //TODO Navigate
            App.NavigationService.Navigate<HomeViewModel>(nameof(HomeView));

        }
        else
        {
            res.Errors!.Select(x => new ValidationError(x.Key, x.Value)).ForEach(Errors.Add);
        }

        
    }
    
}