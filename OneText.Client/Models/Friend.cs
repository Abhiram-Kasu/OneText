using System.Text.Json.Serialization;

namespace OneText.Client.Models;

public class Friend
{
    [JsonPropertyName("id")]
    public int Id {get;set;}
    [JsonPropertyName("firstName")]
    public required string FirstName {get;set;}
    [JsonPropertyName("lastName")]
    public required string LastName {get;set;}
    [JsonPropertyName("friendshipId")]
    public required int FriendshipId { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}