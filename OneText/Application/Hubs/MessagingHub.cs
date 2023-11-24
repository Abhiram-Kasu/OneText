using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OneText.Application.Database;
using OneText.Shared;

namespace OneText.Application.Hubs;
[Authorize]
public class MessagingHub(DatabaseContext db, ILogger<MessagingHub> _logger) : Hub, IServerChatMethods
{
    private static readonly ConcurrentDictionary<string, int> NumUserMappings = new();

    public async Task SubscribeToUser(string groupId)
    {
        _logger.LogInformation("Added user to group {0}", groupId);
        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        NumUserMappings.AddOrUpdate(groupId, 1, 
            (_, num) => num + 1);
        
        await Clients.Group(groupId).SendAsync(nameof(IClientChatMethods.UpdateNumSubscribers), NumUserMappings[groupId]);
    }
    public async Task UnsubscribeFromUser(string groupId)
    {
        _logger.LogInformation("Removed user from group {0}", groupId);
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        NumUserMappings[groupId]--;
        await Clients.Group(groupId).SendAsync(nameof(IClientChatMethods.UpdateNumSubscribers), NumUserMappings[groupId]);
        if(NumUserMappings[groupId] <= 0)
            NumUserMappings.Remove(groupId, out _);
    }

    public async Task SendMessage(string groupId, string message)
    {
        _logger.LogInformation("Sending message to group {0}", groupId);
        await Clients.OthersInGroup(groupId).SendAsync(nameof(IClientChatMethods.ReceiveMessage), message);
        
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        _logger.LogInformation("User Connected");
        
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("User Disconnected");
        
        return base.OnDisconnectedAsync(exception);
    }
}