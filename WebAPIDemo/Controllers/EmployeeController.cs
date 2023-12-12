using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiDBAccess;

namespace WebAPIDemo.Controllers
{
    public class EmployeeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ShowEmpByGender( string gender="All")
        {
            using (APIDBEntities Entities = new APIDBEntities())
            {
                switch(gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, Entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, Entities.Employees.Where(m=>m.Gender.ToLower()=="male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, Entities.Employees.Where(m => m.Gender.ToLower() == "female").ToList());
                    default:
                        return
                            Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please Select All, Male, Female as gender,"+gender+" is Invalid gender..!");
                }
                
            }
        }
        public Employee Get(int id)
        {
            using (APIDBEntities Entities = new APIDBEntities())
            {
                return Entities.Employees.FirstOrDefault(m => m.ID == id);
            }
        }
        public void Post([FromBody]Employee emp)
        {
            using (APIDBEntities Entities = new APIDBEntities())
            {
                Entities.Employees.Add(emp);
                Entities.SaveChanges();
            }
        }
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (APIDBEntities Entities = new APIDBEntities())
                {
                    var Emp = Entities.Employees.FirstOrDefault(m => m.ID == id);
                    if (Emp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, id.ToString() + " is not found");
                    }
                    else
                    {
                        Entities.Employees.Remove(Emp);
                        Entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
