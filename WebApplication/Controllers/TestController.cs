using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication.Model;
using WebApplication.Utility;

namespace WebApplication.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index2(string name, int age)
        {
            SD.HTTPName = name; // gets value from input box 'Name' and put it to a public static variable
            SD.HTTPAge = age;   // get value from input box 'Age' and put it to a public static variable
            return RedirectToAction(nameof(SendNameAge));   // Redirect to SendNameAge
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failed()
        {
            return View();
        }

        public async Task<IActionResult> SendNameAge()
        {
            //string pname = SD.HTTPName;
            //int page = SD.HTTPAge;

            using (var client = new HttpClient())
            {
                // Create new instance of Person
                Person person = new Person
                {
                    Name = SD.HTTPName, // set value of 'Name' Parameter
                    Age = SD.HTTPAge    // set value of 'Age' Parameter
                };

                var personJSON = JsonConvert.SerializeObject(person);   // convert string array to JSON string
                var buffer = System.Text.Encoding.UTF8.GetBytes(personJSON);    // convert string array to byte
                var byteContent = new ByteArrayContent(buffer); // create new instance of byte array context
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // sets a header to 'application/json'

                client.BaseAddress = new Uri(SD.ApiUri);    // create new instance of Uri and set the HttpClient
                var response = await client.PostAsync(SD.ApiUri, byteContent);  // send a post request to the specified Uri

                if (response.IsSuccessStatusCode)   // condition for response status
                {
                    return RedirectToAction(nameof(Success), Json(response)); // if 'success' the return response to HTTP Request, redirect to success page
                }
                else
                {
                    return RedirectToAction(nameof(Failed), Json(response)); // if 'failed' the return response to HTTP Request, redirect to failed page
                }
            }
        }
    }
}