using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClickSWITCH.CDSS.Api.Middleware;

namespace ClickSWITCH.CDSS.Api.Tests.Integration.Helpers
{
    public static class JwtClaimsExtensions
    {
        public static IDictionary<string, object?> AsJwtClaimsDictionary(this RequestJwt requestJwt)
        {
            return requestJwt.GetType()
                         .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                         .ToDictionary
                         (
                             propInfo => propInfo.Name.ToLower(),
                             propInfo => propInfo.GetValue(requestJwt, null)
                         );
        }
    }
}