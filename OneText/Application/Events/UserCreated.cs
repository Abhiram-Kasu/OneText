using Coravel.Events.Interfaces;
using OneText.Application.Models;

namespace OneText.Application.Events;
public class UserCreated : IEvent
{
    public User User { get; set; }

    public UserCreated(User user)
    {
        this.User = user;
    }
}
