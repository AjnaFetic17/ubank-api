using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ubank_api.Data.Helpers;
using ubank_api.Data.Models.In;
using ubank_api.Services.Interfaces;

namespace ubank_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService service)
        {
            _transactionService = service;
        }

        [HttpGet]
        public IActionResult GetTransactions(Guid clientId)
        {
            try
            {
                var items = _transactionService.GetTransaction(clientId);
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
        public IActionResult GetTransaction(Guid id)
        {
            try
            {
                var result = _transactionService.GetTransaction(id);
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
        public IActionResult CreateTransaction(TransactionIn transaction)
        {
            try
            {
                var result = _transactionService.CreateTransaction(transaction);
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

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(Guid id)
        {
            if (_transactionService.DeleteTransaction(id))
            {
                return StatusCode(204);
            }

            return NotFound(new ControllerMessage("There is nothing to delete."));
        }
    }
}
