namespace OneText.Shared;

public interface IClientChatMethods
{
    ValueTask ReceiveMessage(string message);
    ValueTask UpdateNumSubscribers(int numSubscribers);
}

public interface IServerChatMethods
{
    ValueTask SubscribeToUser(string groupId);
    ValueTask UnsubscribeFromUser(string groupId);
    ValueTask SendMessage(string groupId, string message);
}