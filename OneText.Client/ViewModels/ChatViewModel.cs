using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using OneText.Client.Services;
using OneText.Shared;
using SoapDispenser.Avalonia.Helpers.MVVM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OneText.Client.ViewModels;
public partial class ChatViewModel : ViewModelBase, IClientChatMethods
{
    #region Only For Design Time
    public ChatViewModel()
    {
        
    }
    #endregion

    public ChatViewModel(int friendId) => this.friendId = friendId;
    private readonly int friendId;
    private HubConnection? _hubConnection;
    private readonly HttpClient _httpClient = App.Services.GetService(typeof(HttpClient)) as HttpClient ??
                           throw new NullReferenceException();
    private readonly AuthService _authService = App.Services.GetService(typeof(AuthService)) as AuthService ?? throw new ArgumentNullException("Auth Service not found");

    public override async void OnNavigatedTo(object? parameter )
    {
        Console.WriteLine("Hello w");
        base.OnNavigatedTo(parameter);
        Connection = new ConnectionStatus("Connecting", new SolidColorBrush(Colors.Brown));
        var url = _httpClient.BaseAddress + "chat/realtime";
        _hubConnection = new HubConnectionBuilder().WithUrl(url, options => options.AccessTokenProvider = () => Task.FromResult(_authService.AuthStateInformation.Token)!).WithAutomaticReconnect().Build();
        _hubConnection.Reconnecting += x =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                Connection = new ConnectionStatus("Connecting", new SolidColorBrush(Colors.Brown));
            });
            
            return Task.CompletedTask;
        };
        _hubConnection.Reconnected += x =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                Connection = new ConnectionStatus("Connected", new SolidColorBrush(Colors.Green));
            });
            
            return Task.CompletedTask;
        };
        _hubConnection.Closed += x =>
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                Connection = new ConnectionStatus("Disconnected", new SolidColorBrush(Colors.Red));

            });
            return Task.CompletedTask;
        };
        _hubConnection.On<string>(nameof(IClientChatMethods.ReceiveMessage), ReceiveMessage);
        _hubConnection.On<int>(nameof(IClientChatMethods.UpdateNumSubscribers), UpdateNumSubscribers);
        try
        {

        await _hubConnection.StartAsync();
        }catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
        await _hubConnection.InvokeAsync("SubscribeToUser", friendId.ToString() );
        Connection = new ConnectionStatus("Connected", new SolidColorBrush(Colors.Green));
    }
    public override async void OnNavigatedAwayFrom(object? parameter = null)
    {
        base.OnNavigatedAwayFrom(parameter);
        await _hubConnection!.InvokeAsync(nameof(IServerChatMethods.UnsubscribeFromUser), friendId.ToString());
        await _hubConnection!.DisposeAsync();

    }
    ~ChatViewModel()
    {
        _hubConnection!.InvokeAsync(nameof(IServerChatMethods.UnsubscribeFromUser), friendId.ToString());
    }

    public Task ReceiveMessage(string message)
    {
        FriendMessage = message;
        return Task.CompletedTask;
        
    }

    public Task UpdateNumSubscribers(int numSubscribers)
    {
        NumSubscribers = numSubscribers;
        return Task.CompletedTask;
    }

    [ObservableProperty]
    private ConnectionStatus _connection;
    [ObservableProperty]
    private int _numSubscribers;
    [ObservableProperty]
    private string _friendMessage;
    [ObservableProperty]
    private string _localMessage;
    private readonly ProfileService _profileService = App.Services.GetService(typeof(ProfileService)) as ProfileService ?? throw new ArgumentNullException("Profile Service");
    public override string Title => $"{_profileService.User.Friends.First(x => x.FriendshipId == friendId).FullName}";
    [RelayCommand]
    public async Task UpdateMessage()
    {
        Console.WriteLine("Hello w");
        
        await _hubConnection.InvokeAsync(nameof(IServerChatMethods.SendMessage), friendId.ToString(), LocalMessage);
    }

    public record struct ConnectionStatus(string ConnectionString, SolidColorBrush ConnectionColor);
}
