using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shell.Controllers
{
    public class UserController : Controller
    {
        /// <summary>
        /// Get the associated view.
        /// </summary>
        /// <returns>ActionResult</returns>
        [AllowAnonymous]
        public ActionResult Signin()
        {            
            return View();
        }

    }
}