using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Registrations.Common.Domain.Entities;
using Registrations.UserRegistration.Application.Interfaces.Handlers;

namespace Registrations.UserRegistration.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserHandler _handler;

        public UserController(ILogger<UserController> logger, IUserHandler handler)
        {
            _logger = logger;
            _handler = handler;
        }
        // GET: api/<UserController>
        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            try
            {
                return await _handler.GetUser(id);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
            }
            return null;
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<User> Post(User user)
        {
            try
            {
                return await _handler.AddAsync(user, CancellationToken.None);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
            }
            return null;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] string userName)
        {
            try
            {
                await _handler.UpdateAsync(id, userName, CancellationToken.None);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            try
            {
                await _handler.DeleteAsync(id, CancellationToken.None);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
            }
        }
    }
}
