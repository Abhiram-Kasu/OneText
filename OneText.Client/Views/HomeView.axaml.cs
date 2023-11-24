using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OneText.Client.Models;
using OneText.Client.ViewModels;
using System.Collections;
using Avalonia.Interactivity;

namespace OneText.Client;

public partial class HomeView : UserControl
{
    private HomeViewModel _vm => (DataContext as HomeViewModel)!;
    public HomeView()
    {
        InitializeComponent();
    }

   

  


 
}