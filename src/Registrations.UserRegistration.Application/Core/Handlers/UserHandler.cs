using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Registrations.Common.Domain.Entities;
using Registrations.UserRegistration.Application.Interfaces.Handlers;
using Registrations.UserRegistration.Application.Interfaces.Repositories;

namespace Registrations.UserRegistration.Application.Core.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly ILogger<UserHandler> _logger;
        private readonly IUserRepository _repository;
        public UserHandler(IUserRepository repository, ILogger<UserHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async ValueTask<int> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var getUserAsyncTask = _repository.FindAsync(id);
            var userDetails = await ExecuteTaskWithRetryPolicyAsync(getUserAsyncTask);
            if (userDetails == null) return 0;
            var addAsyncTask = _repository.DeleteAsync(userDetails, true, cancellationToken);
            return await ExecuteTaskWithRetryPolicyAsync(addAsyncTask.AsTask());
        }

        public async ValueTask<User> AddAsync(User user, CancellationToken cancellationToken)
        {
            var addAsyncTask = _repository.AddAsync(user, true, cancellationToken);

            return await ExecuteTaskWithRetryPolicyAsync(addAsyncTask.AsTask());
        }

        public async ValueTask<int> UpdateAsync(int id, string userName, CancellationToken cancellationToken)
        {
            var existingUser = await GetUser(id);
            if (existingUser == null) return 0;
            existingUser.Name = userName;
            var updateAsyncTask = _repository.UpdateAsync(existingUser, true, cancellationToken);
            return await ExecuteTaskWithRetryPolicyAsync(updateAsyncTask.AsTask());
        }
        public async ValueTask<User> GetUser(int id)
        {
            var getUserAsyncTask = _repository.FindAsync(id);

            return await ExecuteTaskWithRetryPolicyAsync(getUserAsyncTask);
        }
        private Task<T> ExecuteTaskWithRetryPolicyAsync<T>(Task<T> task)
        {
            var policy = Policy.Handle<Exception>()
                .Or<TimeoutException>().WaitAndRetryAsync(
                    4,
                    retryCount => retryCount == 1 ? TimeSpan.Zero : TimeSpan.FromSeconds(Math.Pow(2, retryCount - 1)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(retryCount == 1 ? $"Immediate retry #1, due to {exception.Message}" :
                            $"Retry #{retryCount}, due to {exception.Message}");
                    });

            return policy.ExecuteAsync(async () => await task);
        }

    }
}
