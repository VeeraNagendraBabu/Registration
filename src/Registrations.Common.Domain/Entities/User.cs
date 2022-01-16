using System;
using System.Collections.Generic;
using System.Text;

namespace Registrations.Common.Domain.Entities
{
    public class User : IEntity<long>, ISystemCreateDate, ISystemModifiedDate
    {
        public string Name { get; set; }
        public long Id { get; set; }
        public DateTime SystemCreateDateTime { get; set; }
        public DateTime SystemModifiedDateTime { get; set; }
    }
}
