using Registrations.UserRegistration.Application.Interfaces.SystemTime;

namespace Registrations.UserRegistration.Application.Core.SystemTime
{
    public class SystemDateTime : ISystemDateTime
    {
        public System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}