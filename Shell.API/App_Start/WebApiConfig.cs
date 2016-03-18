using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using System.Web.Http.ModelBinding;
using System.Net.Http.Formatting;
using System.Web.Http.Validation;
using System.Web.Http.Validation.Providers;
using System.Web.Http.Cors;

namespace Shell.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //
            // Use action based routing (avoid method & attribute routing for now).
            //
            config.Routes.MapHttpRoute(
                "DefaultRoute",
                "v1/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });

            //
            // Remove unnecessary formatters (not processing html forms).
            //
            var jqueryFormatter = config.Formatters.FirstOrDefault(x => x.GetType() == typeof(JQueryMvcFormUrlEncodedFormatter));
            config.Formatters.Remove(jqueryFormatter);
            config.Formatters.Remove(config.Formatters.FormUrlEncodedFormatter);

            //
            // Configure message handlers, filters and such.
            //
            config.MessageHandlers.Add(new Shell.API.Filters.CultureHandler());

            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            config.Services.Replace(typeof(IContentNegotiator), new DefaultContentNegotiator(true));
            config.Services.RemoveAll(typeof(ModelValidatorProvider), validator => !(validator is DataAnnotationsModelValidatorProvider));
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.SuppressHostPrincipal();

            // Convert all dates to UTC
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
        }
    }
}
