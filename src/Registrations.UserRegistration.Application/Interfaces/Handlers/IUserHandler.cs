using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Registrations.Common.Domain.Entities;

namespace Registrations.UserRegistration.Application.Interfaces.Handlers
{
    public interface IUserHandler
    {
        ValueTask<int> DeleteAsync(int id, CancellationToken cancellationToken);
        ValueTask<User> AddAsync(User user, CancellationToken cancellationToken);
        ValueTask<int> UpdateAsync(int id, string userName, CancellationToken cancellationToken);
        ValueTask<User> GetUser(int id);
    }
}