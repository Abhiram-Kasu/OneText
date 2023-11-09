using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using OneText.Application.Database;
using OneText.Shared;

namespace OneText.Application.Hubs;

public class MessagingHub : Hub, IServerChatMethods
{
    private readonly DatabaseContext db;
    private static readonly ConcurrentDictionary<string, int> NumUserMappings = new();

    public MessagingHub(DatabaseContext db)
    {
        this.db = db;
    }
    public async ValueTask SubscribeToUser(string groupId)
    {
        
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        NumUserMappings.AddOrUpdate(groupId, 1, 
            (_, num) => num + 1);
        
        await Clients.Group(groupId).SendAsync(nameof(IClientChatMethods.UpdateNumSubscribers), NumUserMappings[groupId]);
    }
    public async ValueTask UnsubscribeFromUser(string groupId)
    {
        
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        NumUserMappings[groupId]--;
        await Clients.Group(groupId).SendAsync(nameof(IClientChatMethods.UpdateNumSubscribers), NumUserMappings[groupId]);
        if(NumUserMappings[groupId] <= 0)
            NumUserMappings.Remove(groupId, out _);
    }

    public async ValueTask SendMessage(string groupId, string message)
    {
        await Clients.OthersInGroup(groupId).SendAsync(nameof(IClientChatMethods.ReceiveMessage), message);
        
    }
    
}