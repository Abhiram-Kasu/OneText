using Coravel;
using Coravel.Events.Interfaces;
using OneText.Application.Events;
using OneText.Application.Events.Listeners;

namespace OneText.Application.Startup;
public static class Events
{
    public static IServiceProvider RegisterEvents(this IServiceProvider services)
    {
        IEventRegistration registration = services.ConfigureEvents();

        // add events and listeners here
        registration
            .Register<UserCreated>()
            .Subscribe<EmailNewUser>();

        return services;
    }
}
