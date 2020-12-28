using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServerAspNetIdentity.CustomResources
{
    public class SecurityStampResource :  IdentityResource
    {
        public static string ScopeName = "AspNet.Identity.SecurityStamp";
        public SecurityStampResource()
        {
            Name = "security_stamp";
            DisplayName = "ASP NET Identity security stamp";
            Required = true;
            UserClaims.Add(ScopeName);
        }
    }
}