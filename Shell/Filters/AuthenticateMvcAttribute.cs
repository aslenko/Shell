using Shell.Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Security.Claims;

namespace Shell.Filters
{
    /// <summary>
    /// User authentication filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticateMvcUserAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        #region Nested Types

        public static class CustomClaimTypes
        {
            private const string BaseUrl = "http://security/mvc/";
            public const string UserName = BaseUrl + "username";
            public const string Timestamp = BaseUrl + "timestamp";
            public const string AuthenticationType = "password";
            public const string IdentityProvider = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Challenge client with required scheme when negotiating authentication.
        /// </summary>
        /// <param name="filterContext">The filter context</param>
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //
            // Some pages do not require authentication (e.g. login page).
            // As such, authorization handles access to resources.
            //
        }

        /// <summary>
        /// Authenticate the current request.
        /// </summary>
        /// <param name="context">The authentication context</param>
        public void OnAuthentication(AuthenticationContext context)
        {
            //context.HttpContext.Request.Cookies["CookieName"];
            //var entityContext = DependencyResolver.Current.GetService<EntityContext>();

            ClaimsPrincipal principal = null;
            ClaimsIdentity identity = null;
            List<Claim> claims = new List<Claim>();                 
                    
            //TODO: parse authentication context

            /*
             We must use these two claims for anti-forgery token support:
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier' 
            'http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider'          
            */

            claims.Add(new Claim(ClaimTypes.NameIdentifier, "slnk"));
            claims.Add(new Claim(CustomClaimTypes.IdentityProvider, "SLN"));

            claims.Add(new Claim(CustomClaimTypes.UserName, "slnk"));
            claims.Add(new Claim(CustomClaimTypes.Timestamp, DateTime.UtcNow.ToString()));

            identity = new ClaimsIdentity(claims, CustomClaimTypes.AuthenticationType);
            principal = new ClaimsPrincipal(identity);

            if (HttpContext.Current != null)
            {
                context.Principal = principal;
                HttpContext.Current.User = context.Principal;
            }                   
               
        }

        #endregion
    }
}