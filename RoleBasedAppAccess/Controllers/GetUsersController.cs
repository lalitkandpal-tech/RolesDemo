using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoleBasedAppAccess.Models;

namespace RoleBasedAppAccess.Controllers
{
    public class GetUsersController : ApiController
    {

        //public HttpResponseMessage GetUsers()
        //{
            //List<RoleController> Userlist = new List<RoleController>();
            //using (ApplicationDbContext dc = new ApplicationDbContext())
            //{
            //    Userlist = dc.Users.OrderBy(a => a.UserName).ToList();
            //    HttpResponseMessage response;
            //    response = Request.CreateResponse(HttpStatusCode.OK, Userlist);
            //    return response;
            //}
        //}
    }
}
