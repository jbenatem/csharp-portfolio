using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DirectLineTokenGeneration.DirectLineAPI;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace DirectLineTokenGeneration
{
    public static class TokenServiceFunction
    {
        [FunctionName("TokenGenerationFunction")]
        public static async Task<Response> Generate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            Response response = await GenerateTokenAsync();

            return response;
        }

        [FunctionName("TokenRefreshFunction")]
        public static async Task<Response> Refresh(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            string token = req.Query["token"];
            string requestBody = String.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            token = token ?? data?.token;

            Response response = await RefreshTokenAsync(token);

            return response;
        }

        private static async Task<Response> GenerateTokenAsync()
        {
            string HTMLResult;
            JObject response;
            var request = new
            {
                user = new
                {
                    id = $"dl_{Guid.NewGuid()}"
                },
                //trustedOrigins = new object[]
                //{
                //    "INSERT_TRUSTED_DOMAINS"
                //}
            };
            var requestJson = JsonConvert.SerializeObject(request);
            String URL_GENERATE_TOKEN = $"{Constants.URL_GENERATE_TOKEN}";
            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {Constants.DIRECT_LINE_SECRET}");
            wc.Headers.Add(HttpRequestHeader.ContentType, Constants.APPLICATION_JSON);
            try
            {
                HTMLResult = await wc.UploadStringTaskAsync(URL_GENERATE_TOKEN, requestJson);
                response = JObject.Parse(HTMLResult);
                Response rsp = JsonConvert.DeserializeObject<Response>(response.ToString());
                return rsp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static async Task<Response> RefreshTokenAsync(string token)
        {
            string HTMLResult;
            JObject response;
            var request = new{};
            var requestJson = JsonConvert.SerializeObject(request);
            String URL_REFRESH_TOKEN = $"{Constants.URL_REFRESH_TOKEN}";
            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {token}");
            wc.Headers.Add(HttpRequestHeader.ContentType, Constants.APPLICATION_JSON);
            try
            {
                HTMLResult = await wc.UploadStringTaskAsync(URL_REFRESH_TOKEN, requestJson);
                response = JObject.Parse(HTMLResult);
                Response rsp = JsonConvert.DeserializeObject<Response>(response.ToString());
                return rsp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
