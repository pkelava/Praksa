using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Forum.netapi.Controllers
{
    public class SIOController : ApiController
    {

        //Pseudo tablica korisničkog računa
        static List<User> AllUsers = new List<User>
        {
            new User("Petar_Kelava","123456"),
            new User ("Goran_Kelava","456789"),
            new User("Igor_Kelava", "1011121314")

        };
        

        // GET: api/SIO
        [HttpGet]
        public IHttpActionResult DohvatiSveUsere()
        {
            return Ok(AllUsers);
        }

        [HttpGet]
        // GET: api/SIO/5
        public HttpResponseMessage DohvatiJednogUsera(int id)
        {
            if (id > AllUsers.Count || id < AllUsers.Count) 
                return Request.CreateResponse(HttpStatusCode.NotFound, "ID is out of boundaries");
            
            return Request.CreateResponse(HttpStatusCode.Accepted, AllUsers[id]);
        }

        // POST: api/SIO
        [HttpPost]
        public HttpResponseMessage UnesiUsera([FromBody] User newUser)
        {

            User temp = newUser;
            foreach (User existingUser in AllUsers)
            {
                if (newUser.UserName == existingUser.UserName && newUser.Password == existingUser.Password)
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Succesfuly Logged in");
                else if(newUser.UserName == existingUser.UserName)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User Already exists");
                }
                
            }
            AllUsers.Add(newUser);
            return Request.CreateResponse(HttpStatusCode.Accepted, "Account Created"); 

        }


        [HttpPut]
        // PUT: api/SIO/5
        //https://localhost:44384/api/SIO/5?user=Petar_Kelava&oldPassword=123456&newPassword=456
        public HttpResponseMessage PromjeniLozinku([FromUri] string user, [FromUri] string oldPassword, [FromUri] string newPassword)
        {
            foreach(User temp in AllUsers)
            {
                if(temp.UserName == user && temp.Password == oldPassword)
                {
                    temp.Password = newPassword;
                    return Request.CreateResponse(HttpStatusCode.Accepted, temp);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Username/password is incorrect!");
        }

        // DELETE: api/SIO/5
        public HttpResponseMessage Delete(int id)
        {
            if (id > AllUsers.Count || id < 0) 
                return Request.CreateResponse(HttpStatusCode.NotFound, "ID is out of boundaries");
            
            
            AllUsers.Remove(AllUsers[id]);
            return Request.CreateResponse(HttpStatusCode.Accepted, AllUsers);
        }

        public HttpResponseMessage SearchUser(int id)
        {
            if (id > AllUsers.Count) return Request.CreateResponse(HttpStatusCode.NotFound, "ID is out of boundaries");
            else
            {
                return Request.CreateResponse(HttpStatusCode.Accepted, "Action was successful");
            }
        }


    }

    public class User {

        public string UserName { get; set; }
        public string Password { get; set; }

        public User(string name, string pass)
        {
            UserName = name;
            Password = pass;
        }
    
    }

}
