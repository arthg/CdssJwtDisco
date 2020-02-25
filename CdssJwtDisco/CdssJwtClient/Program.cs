using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CdssJwtClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var message = await client.GetStringAsync("https://localhost:44396/weatherforecast");

            Console.WriteLine(message);
            Console.ReadKey();
        }
    }
}
