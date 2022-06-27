using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace InvestmentChat.Identity.Configuration
{
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>()
            {
                new ApiScope("investor", "InvestmentChat"),
            };

        public static IEnumerable<Client> Clients(string host) =>
            new List<Client>
            {
                new Client
                {
                    ClientId="investor",
                    ClientSecrets= { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris={ $"{host}/signin-oidc" },
                    PostLogoutRedirectUris={$"{host}/signout-callback-oidc" },
                    AllowedScopes=new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "investor"
                    }
                },
            };
    }
}

