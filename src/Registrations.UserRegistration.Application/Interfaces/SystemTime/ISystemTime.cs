namespace Registrations.UserRegistration.Application.Interfaces.SystemTime
{
    public interface ISystemDateTime
    {
        System.DateTime UtcNow { get; }
    }
}