using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OneText.Client.ViewModels;

namespace OneText.Client.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void TextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if(e.Key == Avalonia.Input.Key.Enter)
        {
            (this.DataContext as LoginViewModel)!.LoginCommand.Execute(null);
        }
    }
}