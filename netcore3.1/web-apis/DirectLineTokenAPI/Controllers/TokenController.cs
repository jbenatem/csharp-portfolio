using System;
using System.Net;
using System.Threading.Tasks;
using DirectLineTokenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DirectLineTokenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        // POST api/<TokenController>
        [HttpPost]
        public async Task<Response> GenerateTokenController()
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
    }
}
