using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPIApplication.Security;

namespace WebAPIApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenProvider _tokenProvider;

        public HomeController(ILogger<HomeController> logger, ITokenProvider tokenProvider)
        {
            _logger = logger;
            _tokenProvider = tokenProvider;
        }

        [HttpGet(Name = "Protected")]
        [Authorize]
        public IActionResult Get()
        {
            _logger.LogInformation("Protected action method");
            return Ok("Token Authorizations Successfull");
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            _logger.LogInformation("Direct Login action method");

            return Ok(_tokenProvider.CreateToken(HttpContext.User, false,HttpContext.Request.Host.Value));
        }

        [HttpGet("signinwithgoogle")]
        public IActionResult SignInwithGoogle()
        {
            _logger.LogInformation("Login with Google Authentication");

            try
            {
                var redirectUrl = Url.Action("CallBack", "Auth");
                var properties = new AuthenticationProperties
                {
                    RedirectUri = redirectUrl
                };
                var response = Challenge(properties, GoogleDefaults.AuthenticationScheme);
                return response;
            }
            catch (Exception ex)
            {
            _logger.LogError($"Exception - {ex.Message}");
                throw;
            }

        }

        [HttpGet("CallBack")]
        public async Task<IActionResult> Callback()
        {
            var authResult = await HttpContext.AuthenticateAsync("Google");
            if (authResult.Succeeded)
            {
                var emailClaim = authResult.Principal.FindFirst(ClaimTypes.Email);
                var email = emailClaim?.Value; // Retrieve the user's email address from the authentication result
                //Other Logic
                return RedirectToAction("LoggerTest", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        [Route("LoggerTest")]
        public IActionResult LoggerTest()
        {
            try
            {
                int b = 0;
                var s = 7 / b;
                // Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg)); This is used to check exception of serilog
                _logger.LogInformation("Logger Test- Information");
                _logger.LogWarning("Logger Test- Warnig");
                _logger.LogError("Logger Test- Error");
                _logger.LogTrace("Logger Test- Trace");
                return Ok("Logging Tested");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Logging Failed - {ex.Message}");
                return Ok("Logging Failed");
            }
          
        }
    }
}