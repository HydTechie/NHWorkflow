using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NHWorkflow.Services.Authentication
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        public IAuthenticationSchemeProvider Schemes { get; set; }

        public AuthenticationMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes)
        {
            _next = _next ?? throw new ArgumentNullException(nameof(next));
            Schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
        }

        public async Task Invoke(HttpContext context)
        {
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature
            { 
                 OriginalPath = context.Request.Path,
                  OriginalPathBase = context.Request.PathBase

            });

            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();

            foreach(var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                try
                {
                    // TODO: debug here !
                    var handler =   await handlers.GetHandlerAsync(context, scheme.Name) as IAuthenticationRequestHandler;

                    if(handler != null)
                    {
                        if(await handler.HandleRequestAsync())
                        return;
                    }
                   
                }
                catch
                {
                    continue; // incase of not applicable
                }

            }

            // Set the principal to the context 
            var defaultAuthenticateScheme = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if(defaultAuthenticateScheme != null)
            {
                var result = await context.AuthenticateAsync(defaultAuthenticateScheme.Name);
                if(result?.Principal != null)
                {
                    context.User = result.Principal;
                }
            }

            // pass on the chain 
            await _next(context);
        }
    }
}
