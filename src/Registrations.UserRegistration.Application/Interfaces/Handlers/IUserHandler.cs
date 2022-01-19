using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Registrations.Common.Domain.Entities;
using Registrations.UserRegistration.Application.Core.Models;

namespace Registrations.UserRegistration.Application.Interfaces.Handlers
{
    public interface IUserHandler
    {
        ValueTask<int> DeleteAsync(int id, CancellationToken cancellationToken);
        ValueTask<UserModel> AddAsync(UserRequestModel user, CancellationToken cancellationToken);
        ValueTask<int> UpdateAsync(int id, UserRequestModel user, CancellationToken cancellationToken);
        ValueTask<UserModel> GetUser(int id);
    }
}