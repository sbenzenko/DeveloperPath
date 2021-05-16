// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityProvider
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),

                // let's include the role claim in the profile
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                // the api requires the role claim
                new ApiResource("pathapi", "The Developer Path API", new[] { JwtClaimTypes.Role })
                {
                    Scopes = { "pathapi" }
                }
            };

        public static IEnumerable<ApiScope> Scopes =>
            new ApiScope[]
            {
                new ApiScope("pathapi")
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Developer Path Blazor client",
                    ClientId = "WebUI.Blazor",
                
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = {
                        "https://www.developer-path.com",
                        "https://localhost:5005",
                        "https://victorious-cliff-02bdab803.azurestaticapps.net"
                    },

                    AllowedScopes = { 
                      IdentityServerConstants.StandardScopes.OpenId,
                      IdentityServerConstants.StandardScopes.Profile,
                      IdentityServerConstants.StandardScopes.Email,
                      "pathapi" 
                    },
                    RedirectUris = {
                        "https://www.developer-path.com/authentication/login-callback",
                        "https://localhost:5005/authentication/login-callback",
                        "https://victorious-cliff-02bdab803.azurestaticapps.net/authentication/login-callback"
                    },

                    PostLogoutRedirectUris = {
                        "https://www.developer-path.com/",
                        "https://localhost:5005/",
                        "https://victorious-cliff-02bdab803.azurestaticapps.net/"
                    },
                    Enabled = true
                }
            };
    }
}