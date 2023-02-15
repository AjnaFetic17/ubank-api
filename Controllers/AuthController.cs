using Microsoft.AspNetCore.Mvc;
using ubank_api.Data.Helpers;
using ubank_api.Data.Helpers.AuthHelpers;
using ubank_api.Services.Interfaces;

namespace ubank_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _serviceAuth;

        public AuthController(IAuthService service)
        {
            _serviceAuth = service;
        }

        [HttpPost()]
        public IActionResult Login([FromBody] LoginModel request)
        {
            try
            {
                if (request.GrantType == "password")
                {
                    if (request.User == null || request.User.Password == null || request.User.Email == null)
                    {
                        return BadRequest(new ControllerMessage("Invalid client request."));
                    }

                    var result = _serviceAuth.UserLogin(request.User);

                    return Ok(result);
                }
                else if (request.GrantType == "refresh_token")
                {

                    if (request.RefreshToken is null)
                    {
                        return BadRequest(new ControllerMessage("Invalid client request."));
                    }

                    var result = _serviceAuth.GetNewToken(request.RefreshToken);

                    return Ok(result);
                }
                else
                {
                    return BadRequest(new ControllerMessage("Invalid client request."));
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode(401, new ControllerMessage(e.Message));
            }
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword(string email)
        {
            try
            {

                if (email is null)
                {
                    return BadRequest(new ControllerMessage("Invalid client request."));
                }

                _serviceAuth.SendActivationEmail(email);

                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
        }

        [HttpPost("set-new-password")]
        public IActionResult SetNewPassword(ResetPassword resetPassword)
        {
            try
            {

                if (resetPassword is null)
                {
                    return BadRequest(new ControllerMessage("Invalid client request."));
                }

                var result = _serviceAuth.ResetPassword(resetPassword);

                if (result)
                {
                    return Ok("Password changed.");
                }

                return BadRequest(new ControllerMessage("User not verified."));
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
        }
    }
}