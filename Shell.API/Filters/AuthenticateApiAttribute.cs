using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Shell.API.Filters
{
    /// <summary>
    /// Authentication filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticateApiAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        #region Nested Types

        public static class ResonPhrases
        {
            public const string Expired = "Unauthorized, authorization expired.";
            public const string UnauthorizedApplication = "Unauthorized, application.";
            public const string UnauthorizedRoles = "Unauthorized, roles.";
        }   
    
        public static class Claims
        {
            private const string BaseUrl = "http://security/api/";
            public const string UserName = BaseUrl + "username";
            public const string Timestamp = BaseUrl + "timestamp";
            public const string AuthenticationType = "signature";
        }

        /// <summary>
        /// Represents an authentication challenge.
        /// </summary>
        public class ChallengeResult : IHttpActionResult
        {

            #region Constructors
            
            public ChallengeResult(HttpAuthenticationChallengeContext context)
            {
                Context = context;
            }

            #endregion

            #region Public Properties
          
            public HttpAuthenticationChallengeContext Context { get; private set; }

            #endregion

            #region Public Methods
           
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = Context.Request.CreateResponse(HttpStatusCode.Unauthorized);
                response.ReasonPhrase = "Authorization denied.";
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Shell.Core.Security.AuthHeaderSchemes.MAC));
                return Task.FromResult(response);
            }

            #endregion
        }

        /// <summary>
        /// Represents an authentication failure.
        /// </summary>
        public class UnauthorizedResult : IHttpActionResult
        {
            #region Constructors
        
            public UnauthorizedResult(HttpAuthenticationContext context, string reason)
            {
                Context = context;
                Reason = reason;
            }

            #endregion

            #region Public Properties
           
            public HttpAuthenticationContext Context { get; private set; }
                       
            public string Reason { get; private set; }

            #endregion

            #region Public Methods
            /// <summary>
            /// Produce a response message.
            /// </summary>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = Context.Request.CreateResponse(HttpStatusCode.Unauthorized);
                response.ReasonPhrase = Reason;
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Shell.Core.Security.AuthHeaderSchemes.MAC));
                return Task.FromResult(response);
            }
            #endregion
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Challenge client with required scheme when negotiating authentication.
        /// All anonymous requests are challenged (disallowed).
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Task</returns>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var principal = ClaimsPrincipal.Current;
            if (principal == null || principal.Identity == null || principal.Identity.IsAuthenticated == false)
            {
                if (context.Result == null)
                {
                    context.Result = new ChallengeResult(context);
                }
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Authenticate the current request, occurs before authorization filters and controllers are executed.
        /// A claims principal is initialized if authentication succeeds.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            //
            // Note that there may be an auth header, which is not compliant with the http spec.
            // In such a case, the authorization property (below) will silently return null.
            // This is a result of microsoft code validating the header value.
            // API consumers must be given explicit format instructions.
            //
            if (context.ActionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Count > 0 ||
                context.ActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(true).Count > 0)
            {
                //Anonymous requests allowed against this controller\action 
                return Task.FromResult(0);
            }

            var authHeader = context.Request.Headers.Authorization; //headers: "Authorization" : "SLN BlahBlahBlah"
            if (authHeader != null && authHeader.Scheme != null)
            {
                if (authHeader.Scheme.Equals(Shell.Core.Security.AuthHeaderSchemes.MAC, StringComparison.OrdinalIgnoreCase))
                {
                    return AuthenticateRequest(context, cancellationToken);
                }
                else
                {
                    context.ErrorResult = new UnauthorizedResult(context, authHeader.Scheme + " authorization scheme is unsupported.");
                    return Task.FromResult(0);
                }
            }

            context.ErrorResult = new UnauthorizedResult(context, "Authorization header missing, or has invalid scheme");
            return Task.FromResult(0);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Authenticate the current request based on a custom auth header.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private Task AuthenticateRequest(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {                
                var principal = this.ProcessHeaders(context.Request.Headers);
         
                if (principal == null || !principal.Identity.IsAuthenticated)
                {
                    context.ErrorResult = new UnauthorizedResult(context, "Authentication failure: invalid headers");
                    return Task.FromResult(0);
                }
              
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.User = principal;                    
                }

                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                //log error
                throw;
            }
        }

        private ClaimsPrincipal ProcessHeaders(HttpRequestHeaders headers)
        {
            ClaimsPrincipal principal = null;
            ClaimsIdentity identity = null;
            List<Claim> claims = new List<Claim>();

            //TODO: parse headers and create claims

            claims.Add(new Claim(ClaimTypes.NameIdentifier, "slnk"));
            claims.Add(new Claim(Claims.UserName, "slnk"));
            claims.Add(new Claim(Claims.Timestamp, DateTime.UtcNow.ToString()));

            identity = new ClaimsIdentity(claims, Claims.AuthenticationType);            
            principal = new ClaimsPrincipal(identity);

            return principal;
        }

        #endregion
    }

}