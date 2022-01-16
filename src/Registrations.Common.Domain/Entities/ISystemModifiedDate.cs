using System;

namespace Registrations.Common.Domain.Entities
{
    public interface ISystemModifiedDate
    {
        DateTime SystemModifiedDateTime { get; set; }
    }
}