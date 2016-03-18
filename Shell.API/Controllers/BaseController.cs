using Shell.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Shell.API.Controllers
{
    public abstract class BaseController : ApiController
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="entityContext">The entity context</param>
        public BaseController(EntityContext entityContext)
        {
            #region Assertions
            Debug.Assert(entityContext != null);
            #endregion

            this.EntityContext = entityContext;
        }

        #endregion

        #region Public Properties

        public EntityContext EntityContext
        {
            get;
            private set;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Create a response having the specified code and reason phrase.
        /// </summary>
        /// <param name="statusCode">The status code</param>
        /// <param name="dto">The optional dto</param>
        /// <returns>HttpResponseMessage</returns>
        protected HttpResponseMessage CreateResponse(HttpStatusCode statusCode, object dto = null)
        {
            return Request.CreateResponse(statusCode, dto);
        }

        /// <summary>
        /// Create a response having the forbidden code and reason phrase.
        /// </summary>
        /// <param name="reasonPhrase">The optional reason phrase</param>
        /// <returns>HttpResponseMessage</returns>
        protected HttpResponseMessage CreateForbiddenResponse(string reasonPhrase = null)
        {
            var response = Request.CreateResponse(HttpStatusCode.Forbidden);
            response.ReasonPhrase = !string.IsNullOrEmpty(reasonPhrase) ? reasonPhrase.Replace(Environment.NewLine, "  ") : "Forbidden";
            return response;
        }

        /// <summary>
        /// Create a generic unauthorized response 
        /// </summary>
        /// <param name="ex">Original Exception</param>
        /// <returns></returns>
        public HttpResponseMessage CreateUnauthorizedResponse(Exception ex)
        {
            var response = Request.CreateResponse(HttpStatusCode.Unauthorized);
            response.ReasonPhrase = string.Format("{0}", "Unauthorized");
            return response;
        }

        /// <summary>
        /// Create a response for the specified exception.
        /// </summary>
        /// <param name="x">The exception</param>
        /// <param name="logException">Whether to log the exception</param>
        /// <returns>HttpResponseMessage</returns>
        public HttpResponseMessage CreateErrorResponse(Exception x, bool logException = true)
        {
            HttpResponseMessage result = CreateErrorResponse(HttpStatusCode.InternalServerError, x.Message, null);            

            if (x is UnauthorizedAccessException)
            {
                result = CreateUnauthorizedResponse(x);
            }
            else if (x is System.Security.SecurityException)
            {
                result = CreateForbiddenResponse(x.Message);
            }           

            if (logException)
            {
                //log exception
            }

            return result;
        }

        /// <summary>
        /// Create a response having the specified code and reason phrase.
        /// </summary>
        /// <param name="statusCode">The status code</param>
        /// <param name="modelState">The optional model state (if request was invalid)</param>
        /// <returns>HttpResponseMessage</returns>
        public HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, ModelStateDictionary modelState)
        {
            return CreateErrorResponse(statusCode, null, modelState);
        }

        /// <summary>
        /// Create a response having the specified code and reason phrase.
        /// </summary>
        /// <param name="statusCode">The status code</param>
        /// <param name="reasonPhrase">The reason phrase</param>
        /// <param name="modelState">The optional model state (if request was invalid)</param>
        /// <returns>HttpResponseMessage</returns>
        public HttpResponseMessage CreateErrorResponse(HttpStatusCode statusCode, string reasonPhrase, ModelStateDictionary modelState)
        {
            var response = Request.CreateResponse(statusCode, modelState);
            response.ReasonPhrase = !string.IsNullOrEmpty(reasonPhrase) ? reasonPhrase.Replace(Environment.NewLine, "  ") : "Server Error";
            return response;
        }

        #endregion
    }
}