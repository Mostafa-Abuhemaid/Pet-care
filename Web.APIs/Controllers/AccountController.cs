using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Web.Application.DTOs;
using Web.Application.DTOs.AccountDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Infrastructure.Service;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
            private readonly IAccountService _accountService;

            public AccountController(IAccountService accountService) 
            {
                _accountService = accountService;
            }
        

        [HttpPost("Login")]
        public async Task<ActionResult<BaseResponse<TokenDTO>>> Login(LoginDTO loginDto)
        {
            var result = await _accountService.LoginAsync(loginDto);
            return result.Success ? Ok(result) : BadRequest(result);

        }

        [HttpPost("signup")]
        public async Task<IActionResult> RegisterAsync(RegisterDTO request)
        {
            var result = await _accountService
                .RegisterAsync(request);

            return result.Success ? Ok(result) : BadRequest(result);
        }


        [HttpPost("ForgetPassword")]
        public async Task<ActionResult<BaseResponse<string>>> ForgetPassword([FromBody] ForgetPasswordDto request)
        {

            var result = await _accountService.ForgotPasswordAsync(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("VerifyOTP")]
        public async Task<ActionResult<BaseResponse<bool>>> VerifyOTP([FromBody] VerfiyCodeDto verify)
        {

            var result = await _accountService.VerifyOTPAsync(verify);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPut("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            var result = await _accountService.ResetPasswordAsync(resetPassword);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
