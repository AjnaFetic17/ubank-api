using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ubank_api.Data.Helpers;
using ubank_api.Data.Models.Entities;
using ubank_api.Data.Models.In;
using ubank_api.Services.Interfaces;

namespace ubank_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService service)
        {
            _accountService = service;
        }

        [HttpGet]
        public IActionResult GetAccounts(Guid clientId)
        {
            try
            {
                var items = _accountService.GetAccounts(clientId);
                if (items != null)
                {
                    return Ok(items);
                }
            }
            catch (ArgumentException e)
            {
                return StatusCode(409, new ControllerMessage(e.Message));
            }

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(Guid id)
        {
            try
            {
                var result = _accountService.GetAccount(id);
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
        public IActionResult CreateAccount(AccountIn account)
        {
            try
            {
                var result = _accountService.CreateAccount(account);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult EditAccount([FromBody] AccountIn account, Guid id)
        {
            try
            {
                if (account.Id == id)
                {
                    var result = _accountService.UpdateAccount(account, id);

                    if (result != null)
                    {
                        return Ok(result);
                    }

                    return NotFound(new ControllerMessage("There is no account with this id."));
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
        public IActionResult DeleteAccount(Guid id)
        {
            if (_accountService.DeleteAccount(id))
            {
                return StatusCode(204);
            }

            return NotFound(new ControllerMessage("There is nothing to delete."));
        }

        [HttpPost("{id}/deposit")]
        public IActionResult DepositFromAccount(Guid id, [FromBody] float amount)
        {
            try
            {
                var result = _accountService.Deposit(id, amount);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
            return BadRequest();
        }

        [HttpPost("{id}/withdraw")]
        public IActionResult WithdrawFromAccount(Guid id, [FromBody] float amount)
        {
            try
            {
                var result = _accountService.Withdraw(id, amount);
                if (result != null)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentException e)
            {
                return BadRequest(new ControllerMessage(e.Message));
            }
            return BadRequest();
        }

    }
}
