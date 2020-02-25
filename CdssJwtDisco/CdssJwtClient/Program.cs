using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClickSWITCH.CDSS.Api.Middleware;
using ClickSWITCH.CDSS.Api.Tests.Integration.Helpers;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CdssJwtClient
{
    /*
     * 1. Generate Public+Private 
     *
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
            var clientName = "webui";

            var resourcePath = "/institution/3181/customer/11212";
            var requestBody = "messageBody";
            var signed_payload = $"{resourcePath}.{requestBody}";

            
            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(clientName));
            var sig = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(signed_payload));

            var sigStr = BitConverter.ToString(sig)
                                     .Replace("-", string.Empty);

            var jwt = new RequestJwt
            {
                Iat = 1582579659,
                Key = "institution:3181",
                Op = "CreateCustomer",
                Sub = "arthur",
                Sig = sigStr
            };
            //var payload = jwt.AsJwtClaimsDictionary();

            IdentityModelEventSource.ShowPII = true;

            var key = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); // Sha256 throws -- because Asymmetrical?
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload
            {
                { "iat", 1582579659},
                { "key", "institution:3181"},
                { "op", "CreateCustomer"},
                { "sub", "arthur" },
                { "sig", sigStr }
            };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken); //throws here


            client.DefaultRequestHeaders.Add("token", tokenString);
            client.DefaultRequestHeaders.Add("client", clientName);

            var response =  await client.PostAsync(new Uri("https://localhost:44396/weatherforecast/post"), new StringContent(requestBody));
            
        }
    }
}
