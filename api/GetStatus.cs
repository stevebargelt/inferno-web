using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Relay;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Infero.Function
{



    public static class GetStatus
    {
        private const string RelayNamespace = "inferno-bargelt.servicebus.windows.net";
        private const string ConnectionName = "inferno";
        private const string KeyName = "inferno";
        private const string Key = "70deTWujWqKQF9kxA2QeLWECbLdhimsFjB+i94IwZNk=";

        [FunctionName("GetStatus")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Begin
            HttpClient client = HttpClientFactory.Create();
            var baseUri = new Uri(string.Format("https://{0}/{1}/", RelayNamespace, ConnectionName));
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(baseUri, "status"),
                Method = HttpMethod.Get
            };

            await AddAuthToken(request);

            var response = await client.SendAsync(request);

            // return response;
            // End
            string responseMessage;
            if (response.IsSuccessStatusCode)
            {
                // string payload = await response.Content.ReadAsStringAsync();
                // return JsonConvert.DeserializeObject<SmokerStatus>(payload);
                responseMessage = "IT FUCKING WORKED";
            }
            else
            {
                responseMessage = "fail";
            }
            // string name = req.Query["name"];

            // string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

            return new OkObjectResult(responseMessage);
        }

        // private async Task<HttpResponseMessage> SendRelayRequest(string apiEndpoint, HttpMethod method, string payload = "")
        // {
        //     var request = new HttpRequestMessage()
        //     {
        //         RequestUri = new Uri(_baseUri, apiEndpoint),
        //         Method = method
        //     };

        //     if (method == HttpMethod.Post)
        //     {
        //         request.Content = new StringContent(payload);
        //         request.Content.Headers.ContentType.MediaType = "application/json";
        //         request.Content.Headers.ContentType.CharSet = null;
        //     }

        //     await AddAuthToken(request);

        //     var response = await _client.SendAsync(request);

        //     return response;
        // }

        private static async Task AddAuthToken(HttpRequestMessage request)
        {
            TokenProvider tokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(KeyName, Key);
            string token = (await tokenProvider.GetTokenAsync(request.RequestUri.AbsoluteUri, TimeSpan.FromHours(1))).TokenString;

            request.Headers.Add("ServiceBusAuthorization", token);
        }


    }
}
