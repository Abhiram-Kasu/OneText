using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using OneText.Client.ViewModels;

namespace OneText.Client;

public partial class ChatView : UserControl
{
    private ChatViewModel _vm => (DataContext as ChatViewModel)!;
    public ChatView()
    {
        InitializeComponent();
        
    }

    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        _vm.UpdateMessageCommand.Execute(null);
        
    }
}