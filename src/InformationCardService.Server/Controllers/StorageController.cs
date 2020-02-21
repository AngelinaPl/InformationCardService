using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InformationCardService.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly StoreDB _storeDb = new StoreDB();

        [HttpGet]
        public async Task<IActionResult> GetCardsAsync()
        {
            try
            {
                return await Task<IActionResult>.Factory.StartNew(() =>
                {
                    var cards = _storeDb.GetCards();
                    if (cards != null) 
                    {
                        return Ok(cards);
                    }

                    return NotFound();
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}