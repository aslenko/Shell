using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Shell.Model;
using Shell.Model.Managers;

namespace Shell.API.Controllers
{   
    public class UnitController : BaseController
    {
        #region Constructors

        public UnitController(EntityContext entityContext)
            : base(entityContext)
        {            
        }

        #endregion

        #region Public Methods        

        /// <summary>
        /// GET .../unit/get
        /// </summary>       
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UnitManager manager = new UnitManager(EntityContext);
                    var dtos = manager.GetAll();
                    return CreateResponse(HttpStatusCode.OK, dtos);
                }

                return CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            catch (Exception x)
            {
                return CreateErrorResponse(x);
            }
        }

        /// <summary>
        /// GET .../unit/get/5
        /// </summary>       
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UnitManager manager = new UnitManager(EntityContext);
                    var dto = manager.GetAll().Find(u => u.Id == id);
                    return CreateResponse(HttpStatusCode.OK, dto);
                }

                return CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            catch (Exception x)
            {
                return CreateErrorResponse(x);
            }
        }      

        // POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        //public void Delete(int id)
        //{
        //}

        #endregion

    }
}
