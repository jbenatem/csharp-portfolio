using System;
using System.Net;
using System.Threading.Tasks;
using DirectLineTokenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DirectLineTokenAPI.Controllers
{
    [Route("api/token/refresh")]
    [ApiController]
    public class RefreshController : ControllerBase
    {
        // POST api/<TokenController>
        [HttpPost("{token}")]
        public async Task<Response> RefreshTokenController(string token)
        {
            string HTMLResult;
            JObject response;
            var request = new { };
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
