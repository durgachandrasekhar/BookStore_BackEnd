using BookStoreManger.Interface;
using BookStoreModel;
using BookStoreRepository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controller
{
     [ApiController] 
    public class UserController : ControllerBase
    {
        public readonly IUserManager manager;

        private readonly IUserRepository repository;

        private readonly ILogger<UserController> logger;

        public UserController(IUserManager manager, ILogger<UserController> logger, IUserRepository repository)
        {
            this.manager = manager;
            this.logger = logger;
            this.repository = repository;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            try
            {
                this.logger.LogInformation(user.FullName + "Is trying to Register");
                HttpContext.Session.SetString("FullName", user.FullName);
                HttpContext.Session.SetString("Email", user.Email);
                var result = this.manager.Register(user);
                if (result)
                {
                    this.logger.LogInformation(user.FullName + result);
                    var userName = HttpContext.Session.GetString("FullName");
                    var userEmail = HttpContext.Session.GetString("Email");
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Registraion Successfully" });
                }
                else
                {              
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Registraion Failed" });
                }
            }
            catch(Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/login")]

        public IActionResult Login(LoginModel loginDetails)
        {
            try
            {
                this.logger.LogInformation(loginDetails.Email + "Is trying to login");
                string resultMessage = this.manager.Login(loginDetails);
                if (resultMessage.Equals("Login is Successfull"))
                {
                    this.logger.LogInformation(loginDetails.Email + " logged in successfully and the token generated is ");
                    ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                    IDatabase database = connectionMultiplexer.GetDatabase();

                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data.Add("_id", database.StringGet("userID"));
                    data.Add("fullName", database.StringGet("FullName"));
                    data.Add("phone", database.StringGet("Phone"));
                    data.Add("email", loginDetails.Email);
                    data.Add("accessToken", this.manager.GenerateToken(loginDetails.Email));
                    return this.Ok(new { Status = true, Message = resultMessage, result = data });
                }
                else if (resultMessage.Equals("Invalid Password"))
                {
                    this.logger.LogInformation(loginDetails.Email + " " + resultMessage);
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = resultMessage });
                }
                else
                {
                    this.logger.LogInformation(loginDetails.Email + " " + resultMessage);
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = resultMessage });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation("Exception occured while logging in " + ex.Message);
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/ForgotPassword")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                this.logger.LogInformation(email + "is using forgot password");
                string resultMessage = this.manager.ForgotPassword(email);
                if (resultMessage.Equals("Mail sent Succefully"))
                {
                    this.logger.LogInformation(resultMessage);
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = resultMessage });
                }
                else
                {
                    this.logger.LogInformation(resultMessage);
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = resultMessage });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation("Exception occured while using forgot password " + ex.Message);
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        //[HttpPut]
        //[Route("api/ResetPassword")]
        //public IActionResult ResetPassword(ResetPassword resetpassword)
        //{
        //    var resultMessage = this.manager.ResetPassword(resetpassword);
        //    try
        //    {
        //        this.logger.LogInformation(resetpassword + "is using reset password");
        //        //var resultMessage = this.manager.ResetPassword(resetpassword);
        //        if (resultMessage)
        //        {
        //            this.logger.LogInformation("Password reseted Successfully for " + resetpassword);
        //            return this.Ok(new ResponseModel<string>() { Status = true, Message = "Reset passsword is successfull" });
        //        }
        //        else
        //        {
        //            this.logger.LogInformation("Password Reset Failed!");
        //            return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Reset Failed" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.logger.LogInformation("Exception occured while using reset password " + ex.Message);
        //        return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
        //    }
        //}
    }
}
