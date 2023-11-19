using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using OneText.Client.Views;
using SoapDispenser.Avalonia.Helpers.MVVM;

namespace OneText.Client.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private UserControl? _currentView;
    public MainViewModel()
    {
        App.NavigationService.OnCurrentViewChanged += (_, view) => CurrentView = view;
        App.NavigationService.Navigate<LoginViewModel>(nameof(LoginView));
    }
}
