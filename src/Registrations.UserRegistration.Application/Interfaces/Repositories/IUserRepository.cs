using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Registrations.Common.Domain.Entities;
using Registrations.Common.Infrastructure.Relational;
using Registrations.Common.Infrastructure.Relational.Interfaces.Repositories;

namespace Registrations.UserRegistration.Application.Interfaces.Repositories
{
    public interface IUserRepository : IAsyncRepository<User, RegistrationDbContext, long>
    {
        
    }
}