using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProradisEx1.Data.Config;
using ProradisEx1.Data.Dto;
using ProradisEx1.Interface;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProradisEx1.Service
{
    public class SendRequestService : ISendRequestService<CityPayloadDto, ResponseDto>
    {
        private readonly IConfiguration _configuration;
        private readonly string Host;
        public SendRequestService(IConfiguration configuration)
        {
            _configuration = configuration;
            var IOConfig = _configuration.GetSection("Integration").Get<IntegrationConfig>();
            Host = IOConfig.Host;
        }
        public async Task<ResponseDto> Send(CityPayloadDto param)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
                    data.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync(Host, data);

                    var content = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = JsonConvert.DeserializeObject<ResponseDto>(content);
                        return result;
                    }
                    else
                    {
                        throw new Exception($"Error reached in '{Host}' request. Message: {JsonConvert.SerializeObject(response.Content)}");
                    }

                }
            }
            catch
            {
                throw;
            }
        }
    }
}
