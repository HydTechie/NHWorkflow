using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NHWorkflow.Services.Authentication
{
   public static  class NHCookieAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Authentication";
        public const string ExternalAuthenticationScheme = "ExternalAuthentication";

        public static readonly string CookiePrefix = ".NH.";
        public static readonly string ClaimIssuer = "NHWorkflow";

        public static readonly PathString LoginPath = new PathString("/login");
        public static readonly PathString LogoutPath = new PathString("/logout");
        public static readonly PathString AccessDeniedPath = new PathString("/page-not-found");

        public static readonly string ReturnUrlParameter = "";

    }
}
