using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using InformationCardService.Common;
using InformationCardService.Common.interfaces;
using Newtonsoft.Json;

namespace InformationCardService.Client.Services
{
    public class CardService
    {
        private readonly string _apiUrl;
        private readonly HttpClient _client;

        public CardService()
        {
            _client = new HttpClient();
            _apiUrl = ConfigurationManager.AppSettings["CardStorageApi"];
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<IResult<ObservableCollection<InformationCard>>> GetCardsAsync()
        {
            IResult<ObservableCollection<InformationCard>> result = null;
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    var response = _client.GetAsync(_apiUrl).Result;
                    var cards = response.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<ObservableCollection<InformationCard>>(cards);
                    result = new Result<ObservableCollection<InformationCard>>(data);
                });
            }
            catch (Exception e)
            {
                result = new Result<ObservableCollection<InformationCard>>(e);
            }

            return result;
        }

        public async Task<IResult<bool>> SaveCardAsync(InformationCard card)
        {
            IResult<bool> result = null;
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    var serializedProduct = JsonConvert.SerializeObject(card);
                    var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                    var response = _client.PostAsync(_apiUrl, content).Result;
                    result = new Result<bool>(response.IsSuccessStatusCode);
                });
            }
            catch (Exception e)
            {
                result = new Result<bool>(e);
            }

            return result;
        }

        public async Task<IResult<bool>> DeleteCardAsync(int cardId)
        {
            IResult<bool> result = null;
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    var query = _apiUrl + "/" + cardId;
                    var response = _client.GetAsync(query).Result;
                    result = new Result<bool>(response.IsSuccessStatusCode);
                });
            }
            catch (Exception e)
            {
                result = new Result<bool>(e);
            }

            return result;
        }
    }
}