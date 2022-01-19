using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Registrations.UserRegistration.Application.Core.Models;
using Registrations.UserRegistration.Application.Interfaces.Handlers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Registrations.UserManagement.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<UserModel>> Get(int id)
        {
            try
            {
                var response = await _handler.GetUser(id);
                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
            }
            return null;
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<UserModel>> Post(UserRequestModel user)
        {
            try
            {
                var response = await _handler.AddAsync(user, CancellationToken.None);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    exception.InnerException != null && exception.InnerException.Message.Contains("UNIQUE constraint failed")
                        ? "Invalid input, User already exists"
                        : "Invalid input/ Unable to create given User");
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UserRequestModel user)
        {
            try
            {
                await _handler.UpdateAsync(id, user, CancellationToken.None);
                return new StatusCodeResult(202);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest,
                    exception.InnerException != null && exception.InnerException.Message.Contains("UNIQUE constraint failed")
                        ? "Invalid input, User already exists"
                        : "Invalid input/ Unable to update the User");
            }
        }
        
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _handler.DeleteAsync(id, CancellationToken.None);
                return new StatusCodeResult(202);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
                return StatusCode(StatusCodes.Status400BadRequest,"Invalid input/ Unable to delete the User");
            }
        }
    }
}
