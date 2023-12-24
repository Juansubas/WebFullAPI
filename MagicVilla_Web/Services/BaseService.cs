using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using MagivVilla_Utility;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
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

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
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

                HttpResponseMessage apiResponse = null;

                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    APIResponse response = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if (apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        response.statusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(response);
                        var obj = JsonConvert.DeserializeObject<T>(res);
                        return obj;
                    }
                }
                catch (Exception)
                {
                    var errorResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return errorResponse;
                }

                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;

            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };
                //Lo transforma a un Json
                var res = JsonConvert.SerializeObject(dto);
                //Lo transforma de Json a un objeto definido en el T
                var responseEx = JsonConvert.DeserializeObject<T>(res);
                return responseEx;
            }
        }
    }
}
