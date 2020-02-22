using System;
using System.Text.Json;
using System.Threading.Tasks;
using InformationCardService.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                    if (cards != null) return Ok(cards);

                    return NotFound();
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync(object card)
        {
            try
            {
                return await Task<IActionResult>.Factory.StartNew(() =>
                {
                    if (card != null)
                    {
                        var json = ((JsonElement) card).GetRawText();
                        var infCard = (InformationCard) JsonConvert.DeserializeObject(json, typeof(InformationCard));
                        _storeDb.SaveCard(infCard);
                        return Ok();
                    }

                    return NotFound();
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadAsync(int id)
        {
            try
            {
                return await Task<IActionResult>.Factory.StartNew(() =>
                {
                    _storeDb.DeleteCard(id);
                    return Ok();
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}