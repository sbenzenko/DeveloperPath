// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace IdentityProvider
{
    public static class Config
    {
        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("pathapi", "The Developer Path API")
                {
                    Scopes = { "pathapi" }
                }
            };

        public static IEnumerable<ApiScope> Scopes => 
            new ApiScope[]
            {
                new ApiScope("pathapi")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebUI.Blazor",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { "https://victorious-cliff-02bdab803.azurestaticapps.net/" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        //IdentityServerConstants.StandardScopes.Email,
                        "pathapi"
                    },
                    RedirectUris = { "https://victorious-cliff-02bdab803.azurestaticapps.net//authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://victorious-cliff-02bdab803.azurestaticapps.net/" },
                    Enabled = true
                },
            };
    }
}