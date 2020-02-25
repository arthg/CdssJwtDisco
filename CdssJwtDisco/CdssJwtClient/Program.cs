using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CdssJwtClient
{
    /*
     * Client needs to know the public key
     * how?  expose on API endpoint?
     * How to generate the pairs?
     * Will need yto stire the private key in vault
     * 
     */
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            client.DefaultRequestHeaders.Add("token", "{\"message\":\"got JSON?\"}");
            var message = await client.GetStringAsync("https://localhost:44396/weatherforecast");

            Console.WriteLine(message);
            Console.ReadKey();

            HttpRequestMessage request = new HttpRequestMessage
            {
                //Method = 
                RequestUri = new Uri("https://localhost:44396/weatherforecast")
            };
         //   request.Headers.Add("token", "{\"message\":\"got JSON?\"}");
           

            await client.PostAsync(new Uri("https://localhost:44396/weatherforecast/post"), new StringContent(""));
            
        }
    }
}
