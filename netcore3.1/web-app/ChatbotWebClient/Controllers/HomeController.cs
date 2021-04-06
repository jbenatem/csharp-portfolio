using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChatbotWebClient.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace ChatbotWebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly string userId;

        public HomeController()
        {
            this.userId = $"dl_{Guid.NewGuid()}";
        }

        public IActionResult Index()
        {
            string directLineToken = string.Empty;

            var directLineClient = new HttpClient();
            directLineClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", Constants.DIRECT_LINE_SECRET);

            string content = JsonConvert.SerializeObject(
                    new
                    {
                        User = new
                        {
                            Id = this.userId
                        }
                    });

            HttpResponseMessage response = directLineClient.PostAsync(Constants.URL_GENERATE_TOKEN, 
                new StringContent(content, Encoding.UTF8, "application/json")).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseString = response.Content.ReadAsStringAsync().Result;
                var directLineResponse = JsonConvert.DeserializeObject<Response>(responseString);
                directLineToken = directLineResponse.token;
            }

            ViewData["DirectLineToken"] = directLineToken;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
