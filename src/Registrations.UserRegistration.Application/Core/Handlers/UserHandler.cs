using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Registrations.Common.Domain.Entities;
using Registrations.UserRegistration.Application.Core.Models;
using Registrations.UserRegistration.Application.Interfaces.Handlers;
using Registrations.UserRegistration.Application.Interfaces.Repositories;

namespace Registrations.UserRegistration.Application.Core.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly ILogger<UserHandler> _logger;
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserHandler(IUserRepository repository, ILogger<UserHandler> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async ValueTask<int> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var userDetails = await _repository.FindAsync(id);
            if (userDetails == null) throw new Exception("Invalid Request");
            return await _repository.DeleteAsync(userDetails, true, cancellationToken);
        }

        public async ValueTask<UserModel> AddAsync(UserRequestModel user, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<User>(user);
            var userResponse = await _repository.AddAsync(request, true, cancellationToken);
            return _mapper.Map<UserModel>(userResponse);
        }

        public async ValueTask<int> UpdateAsync(int id, UserRequestModel user, CancellationToken cancellationToken)
        {
            var existingUser = await GetUserEntity(id);
            if (existingUser == null) throw new Exception("Invalid Request");
            existingUser.Name = user.username;
            existingUser.Id = id;
            return await _repository.UpdateAsync(existingUser, true, cancellationToken);
        }
        private async ValueTask<User> GetUserEntity(int id)
        {
            return await _repository.FindAsync(id);
        }
        public async ValueTask<UserModel> GetUser(int id)
        {
            var user = await _repository.FindAsync(id);
            return _mapper.Map<UserModel>(user);
        }
    }
}
