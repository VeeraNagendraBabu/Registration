using Registrations.Common.Domain.Entities;
using Registrations.Common.Infrastructure.Relational;
using Registrations.Common.Infrastructure.Relational.Interfaces;
using Registrations.Common.Infrastructure.Relational.Repositories;
using Registrations.UserRegistration.Application.Interfaces.Repositories;

namespace Registrations.UserRegistration.Application.Core.Repositories
{
    public class UserRepository : AsyncRepository<User, RegistrationDbContext, long>, IUserRepository
    {
        private readonly IUnitOfWork<RegistrationDbContext> _uow;

        public UserRepository(IUnitOfWork<RegistrationDbContext> uow) : base(uow)
        {
            _uow = uow;
        }
    }
}
