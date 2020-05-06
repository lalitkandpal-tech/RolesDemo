using RoleBasedAppAccess.CustomFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoleBasedAppAccess.Controllers
{
    public class UsersController : Controller
    {
       [AuthLog(Roles = "User")]
        public ActionResult Index()
        {
            return View();
        }
    }
}