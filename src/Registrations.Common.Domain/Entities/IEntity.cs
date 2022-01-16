using System;

namespace Registrations.Common.Domain.Entities
{
    public interface IEntity<TPk> 
        where TPk : IComparable
    {
        TPk Id { get; set; }
    }
}