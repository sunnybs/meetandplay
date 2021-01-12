using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServerAspNetIdentity.CustomResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace IdentityServerAspNetIdentity.Identity.SeedController
{
    public class SeedController : Controller
    {
        private readonly ConfigurationDbContext _context;

        public SeedController(ConfigurationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Seed()
        {
            if (!await _context.Clients.AnyAsync())
            {
                foreach (var client in Clients)
                {
                    await _context.Clients.AddAsync(client.ToEntity());
                }
                await _context.SaveChangesAsync();
            }

            if (!await _context.IdentityResources.AnyAsync())
            {
                foreach (var resource in IdentityResources)
                {
                    await _context.IdentityResources.AddAsync(resource.ToEntity());
                }
                await _context.SaveChangesAsync();
            }

            if (!await _context.ApiScopes.AnyAsync())
            {
                foreach (var resource in ApiScopes)
                {
                    await _context.ApiScopes.AddAsync(resource.ToEntity());
                }
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        private static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new SecurityStampResource()
            };


        private static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api", "Meet And Play API")
            };

        private static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new()
                {
                    ClientId = "meetAndPlay_blazor",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:6001/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:6001/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "security_stamp",
                        "api"
                    }
                }
            };
    }
}