using System;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using OneText.Client.Services;
using OneText.Client.ViewModels;
using OneText.Client.Views;
using SoapDispenser.Avalonia.Helpers.Navigation;

namespace OneText.Client;

public partial class App : Application
{
    private static readonly IServiceCollection ServiceCollection = new ServiceCollection();
    public static IServiceProvider Services { get; private set; } = default!;
    public static readonly INavigationService NavigationService = new NavigationService();
    private const string BackendUrl = "http://localhost:5217";
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);
        

        #region Services

        ServiceCollection.AddSingleton(new HttpClient()
        {
            BaseAddress = new Uri(BackendUrl)
        });
        ServiceCollection.AddSingleton((_) => new AuthService());

        #endregion

        #region ViewModels
        NavigationService.AddViewAndViewModel<MainViewModel>(nameof(MainView), typeof(MainView));
        NavigationService.AddViewAndViewModel<LoginViewModel>(nameof(LoginView), typeof(LoginView));

        

        #endregion
        
        

        Services = ServiceCollection.BuildServiceProvider();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
