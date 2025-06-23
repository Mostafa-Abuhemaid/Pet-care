using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.AccountDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;

namespace Web.Infrastructure.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;    
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;
       
        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, ITokenService tokenService, IMapper mapper, IMemoryCache memoryCache, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
            _tokenService = tokenService;         
            _memoryCache = memoryCache;
            _emailService = emailService;
        }
        public async Task<BaseResponse<string>> ForgotPasswordAsync(ForgetPasswordDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new BaseResponse<string>(false, "Email not found");

            var otp = new Random().Next(100000, 999999).ToString();
            _memoryCache.Set(request.Email, otp, TimeSpan.FromMinutes(60));
            await _emailService.SendEmailAsync(request.Email, "Smile-Simulation", $"Your verification code is: {otp}");
            var Token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new BaseResponse<string>(true, "Please check your email", Token);
        }

        public async Task<BaseResponse<TokenDTO>> LoginAsync(LoginDTO loginDto)
        {
            if (loginDto == null)
                return new BaseResponse<TokenDTO>(false, "Login data is required");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return new BaseResponse<TokenDTO>(false, "Email is not registered");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return new BaseResponse<TokenDTO>(false, "Incorrect password");

            var roles = await _userManager.GetRolesAsync(user);
            var res = new TokenDTO
            {
                UserId = user.Id,
                Token = await _tokenService.GenerateTokenAsync(user, _userManager)
            };

            return new BaseResponse<TokenDTO>(true, "Login successful", res);
        }

       public async Task<BaseResponse<TokenDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO is null)
                return new BaseResponse<TokenDTO>(false, "Register data is required");
            var isExist = await _userManager.Users.AnyAsync(p=>p.Email==registerDTO.Email);

            if (isExist)
                return new BaseResponse<TokenDTO>(false, "Another user with the same email is already exists");

            var user = new AppUser
            {
                Email = registerDTO.Email,
                UserName=registerDTO.Email,
                PhoneNumber = registerDTO.Phone

            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if(result.Succeeded)
            {
                var res = new TokenDTO
                {
                    UserId = user.Id,
                    Token = await _tokenService.GenerateTokenAsync(user, _userManager)
                };

                return new BaseResponse<TokenDTO>(true, "Register successful", res);

            }

            var error=result.Errors.FirstOrDefault();
            return new BaseResponse<TokenDTO>(false,$"{error!.Description}");

        }

        public async Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordDto resetPassword)
        {
            if (resetPassword.NewPassword != resetPassword.ConfirmNewPassword)
                return new BaseResponse<bool>(false, "New password and confirmation do not match");

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) return new BaseResponse<bool>(false, "Email not found");

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (!result.Succeeded) return new BaseResponse<bool>(false, "Failed to reset the password");

            return new BaseResponse<bool>(true, "Password updated successfully");
        }

        public async Task<BaseResponse<bool>> VerifyOTPAsync(VerfiyCodeDto verify)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(verify.Email);
                if (user == null)
                    return new BaseResponse<bool>(false, $"Email '{verify.Email}' is not found.");

                var cachedOtp = _memoryCache.Get(verify.Email)?.ToString();
                if (string.IsNullOrEmpty(cachedOtp))
                    return new BaseResponse<bool>(false, "Code not found or expired. Please request a new code.");

                if (!string.Equals(verify.CodeOTP, cachedOtp, StringComparison.OrdinalIgnoreCase))
                    return new BaseResponse<bool>(false, "Incorrect code. Please enter the correct code.");

                _memoryCache.Remove(verify.Email);

                return new BaseResponse<bool>(true, "OTP verified successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in VerifyOTPAsync: {ex.Message}");
                return new BaseResponse<bool>(false, "An unexpected server error occurred. Please try again later.");
            }
        }

    }
}
