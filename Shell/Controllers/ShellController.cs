using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shell.Controllers
{
    /// <summary>
    /// Shell controller.
    /// </summary>
    [AllowAnonymous]
    public class ShellController : Controller
    {
        #region Public Methods
        /// <summary>
        /// Get the associated view.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View("~/views/shared/_shell.cshtml");
        }
        #endregion
    }
}