using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RicardoDevAPI.Context;
using RicardoDevAPI.Helpers;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace RicardoDevAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevController : ControllerBase
    {


        private readonly ILogger<DevController> _logger;
        private IUserService _userService;
        private IMapper _mapper;
        

        public DevController(ILogger<DevController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        

        [Route("~/signup")]
        [HttpPost]
        public JsonResult signup([FromBody] SignUp? model)
        {
            if (model == null)
            {
                Dictionary<string, Object> result = new Dictionary<string, Object>();
                result.Add("message", "Account creation failed");
                result.Add("cause", "required user_id and password");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(result);
            }
            try
            {

                _userService.SignUpModel(model);
                //var json = JsonConvert.SerializeObject(model);
                Dictionary<string, Object> result = new Dictionary<string, Object>();
                result.Add("message", "Account successfully created");

                Dictionary<string, string> newuser = new Dictionary<string, string>();
                newuser.Add("user_id", model.User_id);
                newuser.Add("nickname", model.User_id);
                result.Add("user", newuser);



                return new JsonResult(result);
            }catch(Exception ex)
            {
                
                Dictionary<string, Object> result = new Dictionary<string, Object>();
                result.Add("message", "Account creation failed");
                result.Add("cause",ex.Message);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult(result);
            }
        }

        [HttpGet("~/users/{user_id}")]
        public JsonResult Getbyuserid([FromHeader] string? Authorization,string user_id) {
            if (Authorization == null)
            {
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", "Authentication Failed");
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(err);
            }
            byte[] data;
            try
            {
                data = Convert.FromBase64String(Authorization);
            }catch(Exception e)
            {
                data = null;
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", "Authentication Failed");
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(err);
            }
            string authentication = Encoding.UTF8.GetString(data);
            try
            {
                var user = _userService.GetUser(user_id);
                string compareAuth = "Basic " + user.User_id + ":" + user.Password;

                //compare auth
                //Basic string:string = QmFzaWMgc3RyaW5nOnN0cmluZw==
                if (!authentication.Equals(compareAuth))
                {
                    Dictionary<string, Object> err = new Dictionary<string, Object>();
                    err.Add("message", "Authentication Failed");

                    return new JsonResult(err);
                }

                Dictionary<string, Object> result = new Dictionary<string, Object>();
                result.Add("message", "User details by user_id");
                Dictionary<string, string> newuser = new Dictionary<string, string>();
                newuser.Add("user_id", user.User_id);
                newuser.Add("nickname", user.Nickname);
                if (!user.Comment.Equals(""))
                {
                    newuser.Add("comment", user.Comment);
                }

                result.Add("user", newuser);

                return new JsonResult(result);
            }catch(Exception ex)
            {
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", ex.Message);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return new JsonResult(err);
            }
        }

        [HttpPatch("~/users/{user_id}")]
        public JsonResult Updateuser([FromHeader] string? Authorization, string user_id, [FromBody] UpdateUser model)
        {
            //auth
            if(Authorization == null)
            {
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", "Authentication Failed");
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(err);
            }
            byte[] data;
            try
            {
                data = Convert.FromBase64String(Authorization);
            }
            catch (Exception e)
            {
                data = null;
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", "Authentication Failed");
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(err);
            }
            string authentication = Encoding.UTF8.GetString(data);
            try
            {
                var user = _userService.GetUser(user_id);
                string compareAuth = "Basic " + user.User_id + ":" + user.Password;

                //compare auth
                //Basic string:string = QmFzaWMgc3RyaW5nOnN0cmluZw==
                if (!authentication.Equals(compareAuth))
                {
                    Dictionary<string, Object> err = new Dictionary<string, Object>();
                    err.Add("message", "No Permission for Update");
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new JsonResult(err);
                }

                //logic
                //if (!model.UserName.Equals(""))
                //{
                //    //return 400
                //    Dictionary<string, Object> err = new Dictionary<string, Object>();
                //    err.Add("message", "User updation failed");
                //    err.Add("cause", "not updatable user_id and password");
                //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //    return new JsonResult(err);
                //}
                if (model.nickname.Equals(""))
                {
                    model.nickname = user.User_id;
                }
                _userService.UpdateModel(user_id, model);
                Dictionary<string, Object> result = new Dictionary<string, Object>();
                result.Add("message", "User successfully updated");
                Dictionary<string, string> newuser = new Dictionary<string, string>();
                
                newuser.Add("nickname", user.Nickname);
                if (!user.Comment.Equals(""))
                {
                    newuser.Add("comment", user.Comment);
                }

                result.Add("user", newuser);

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", ex.Message);
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return new JsonResult(err);
            }
        }

        [Route("~/close")]
        [HttpPost]
        public JsonResult closeUser([FromHeader] string? Authorization)
        {
            if (Authorization == null)
            {
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", "Authentication Failed");
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(err);
            }
            //auth
            byte[] data;
            try
            {
                data = Convert.FromBase64String(Authorization);
            }
            catch (Exception e)
            {
                data = null;
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", "Authentication Failed");
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(err);
            }
            string authentication = Encoding.UTF8.GetString(data);
            string auth_decoded = authentication.Substring(authentication.IndexOf(' ')+1);
            string user_id = auth_decoded.Substring(0, (auth_decoded.IndexOf(':')));
            try
            {
                var user = _userService.GetUser(user_id);
                string compareAuth = "Basic " + user.User_id + ":" + user.Password;

                //compare auth
                //Basic string:string = QmFzaWMgc3RyaW5nOnN0cmluZw==
                if (!authentication.Equals(compareAuth))
                {
                    Dictionary<string, Object> err = new Dictionary<string, Object>();
                    err.Add("message", "No Permission for Update");
                    Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return new JsonResult(err);
                }

               
                _userService.Delete(user_id);

                Dictionary<string, Object> result = new Dictionary<string, Object>();
                result.Add("message", "Account and user successfully removed");
                

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                Dictionary<string, Object> err = new Dictionary<string, Object>();
                err.Add("message", "Authentication Failed");
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(err);
            }
        }
    }

   
}