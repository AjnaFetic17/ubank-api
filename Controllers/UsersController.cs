using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ubank_api.Data.Helpers;
using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Data.Models.In;
using ubank_api.Services.Interfaces;

namespace ubank_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;

        public UsersController(IUserService service, ICacheService cacheService)
        {
            _userService = service;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            try
            {
                var cacheResult = _cacheService.GetFromCache(CacheKeys.User);
                if (cacheResult != null)
                {
                    return Ok(cacheResult);
                }
                else
                {
                    var items = _userService.GetUsers();
                    if (items != null)
                    {
                        return Ok(items);
                    }
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode(409, new ControllerMessage(e.Message));
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            try
            {
                var result = _userService.GetUser(id);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
            return NotFound();
        }

        [HttpGet("{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                var result = _userService.GetUserByEmail(email);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserRegister request)
        {
            try
            {
                var result = _userService.CreateUser(request);

                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode(409, new ControllerMessage(e.Message));
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditUser([FromBody] UserIn user, Guid id)
        {
            try
            {
                if (user.Id == id)
                {
                    var result = _userService.UpdateUser(user, id);

                    if (result != null)
                    {
                        return Ok(result);
                    }

                    return NotFound(new ControllerMessage("There is no user with this id."));
                }
                else
                {
                    return StatusCode(400, new ControllerMessage("Id's provided do not match."));
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            if (_userService.DeleteUser(id))
            {
                return StatusCode(204);
            }

            return NotFound(new ControllerMessage("There is nothing to delete."));
        }
    }
}
