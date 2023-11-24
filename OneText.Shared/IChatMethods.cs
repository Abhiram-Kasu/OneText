namespace OneText.Shared;

public interface IClientChatMethods
{
    Task ReceiveMessage(string message);
    Task UpdateNumSubscribers(int numSubscribers);
}

public interface IServerChatMethods
{
    Task SubscribeToUser(string groupId);
    Task UnsubscribeFromUser(string groupId);
    Task SendMessage(string groupId, string message);
}