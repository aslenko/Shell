using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Globalization;

namespace Shell.API.Filters
{
    public class CultureHandler : DelegatingHandler
    {
        private static class Constants
        {
            public const string AcceptLanguage = "Accept-Language";
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //processing request
            if (request.Headers.Contains(Constants.AcceptLanguage))
            {
                string culture = request.Headers.GetValues(Constants.AcceptLanguage).FirstOrDefault();

                if (culture != "FR-fr")
                {
                    //change current culture
                    CultureInfo useCulture = new CultureInfo(culture);
                    Thread.CurrentThread.CurrentCulture = useCulture;
                    Thread.CurrentThread.CurrentUICulture = useCulture;
                }
            }

            //call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);

            //processing response
            return response;
        }

    }
}