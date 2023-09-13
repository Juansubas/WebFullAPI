using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using MagivVilla_Utility;
using Newtonsoft.Json;
using System.Text;
using static MagivVilla_Utility.DS;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this._httpClient = httpClient;
        }

        public Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest is { Datos: not null }) //apiRequest.Datos != null
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos), Encoding.UTF8, "application/json");
                }

                message.Method = apiRequest.APITipo switch
                {
                    APITipo.POST => HttpMethod.Post,
                    APITipo.PUT => HttpMethod.Put,
                    APITipo.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get,
                };

                //The following code is an extended version of the code above.
                //switch (apiRequest.APITipo)
                //{
                //    case DS.APITipo.POST:
                //        message.Method = HttpMethod.Post;
                //        break;
                //    case DS.APITipo.PUT:
                //        message.Method = HttpMethod.Put;
                //        break;
                //    case DS.APITipo.DELETE:
                //        message.Method = HttpMethod.Delete;
                //        break;
                //    default:
                //        message.Method = HttpMethod.Get;
                //        break;
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
