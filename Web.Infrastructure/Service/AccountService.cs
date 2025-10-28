using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.AccountDTO;
using Web.Application.DTOs.PetProfileDTO;
using Web.Application.DTOs.ProductDTO;
using Web.Application.DTOs.VetDTO;
using Web.Application.Interfaces;
using Web.Application.Response;
using Web.Domain.Entites;
using Web.Infrastructure.Persistence.Data;

namespace Web.Infrastructure.Service
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;    
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _memoryCache;
       
        public AccountService(AppDbContext context,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, ITokenService tokenService, IMapper mapper, IMemoryCache memoryCache, IEmailService emailService)
        {
            _context = context;
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
            await _emailService.SendEmailAsync(request.Email, "Pet Paw", $"Your verification code is: {otp}");
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
                PhoneNumber = registerDTO.Phone,
                FullName=registerDTO.Name 
               
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


        public async Task<BaseResponse<UserProfileDTO>>GetProfileUser(string userid)
        {
            var user=await _context.Users.Include(x=>x.Pets).FirstOrDefaultAsync(x=>x.Id== userid);
            var response=new UserProfileDTO
                (
                 userid
                ,user!.FullName
                ,user.Email!
                ,user.PhotoURl!
                ,user.Pets.Select(x=>new Application.DTOs.PetProfileDTO.YourPetsDTO
                (
                    x.PhotoUrl!
                    ,x.Name
                    ,x.Breed
                    ,x.Gender
                    )).ToList());
            return new BaseResponse<UserProfileDTO>(true,"success",response);
           
        }


        public async Task<BaseResponse<IEnumerable<UserHistoryDto>>> GetHistory(string userid)

        {
            var histories = await _context.histories
                .Include(x => x.VetClinic)
                .Include(x => x.Product)
                .Include(x => x.User)
                .Where(x => x.UserId == userid)
                .AsNoTracking()
                .OrderBy(x => x.Date)
                .Select(x => new UserHistoryDto(
                    userid,
                    x.Name,
                    x.Desciption,
                    x.Unit,
                   x.ImageURL,
                    x.Price,
                    x.Date
                    ))
                .Take(20)
                .ToListAsync();
            if(histories is  null)
            return new BaseResponse<IEnumerable<UserHistoryDto>>(false, "Your History is Empty");


            return new BaseResponse<IEnumerable<UserHistoryDto>>(true, "Success", histories);

        }

        public async Task<BaseResponse<IEnumerable<ProductResponse>>> GetFavoriteProduct(string userid)
        {
            var favoriteProducts = _context.Favorites
                .Include(x => x.Product)
                .Where(x => x.UserId == userid)
                .Select(x => new ProductResponse(
                    x.Product.Id,
                    x.Product.Name,
                    x.Product.Description,
                    x.Product.Size,
                    x.Product.rate,
                    x.Product.Price,
                    x.Product.ImageUrl
                ))
                .ToList();
            if(favoriteProducts is null)
                return new BaseResponse<IEnumerable<ProductResponse>>(false, "Your favorite Products is Empty");

            return new BaseResponse<IEnumerable<ProductResponse>>(true,"Success",favoriteProducts);
        }

        public async Task<BaseResponse<IEnumerable<VetListItemFavoriteDto>>> GetFavoriteVetClinc(string userid)
        {
            var favoriteProducts = _context.Favorites
                .Include(x => x.VetClinic)
                .Where(x => x.UserId == userid)
                .Select(x => new VetListItemFavoriteDto(
                    x.VetClinic.Id,
                    x.VetClinic.Name,
                    x.VetClinic.Type,
                    x.VetClinic.logoUrl,
                    x.VetClinic.PricePerNight
                ))
                .ToList();
            if (favoriteProducts is null)
                return new BaseResponse<IEnumerable<VetListItemFavoriteDto>>(false, "Your favorite Products is Empty");

            return new BaseResponse<IEnumerable<VetListItemFavoriteDto>>(true,"Success",favoriteProducts);
        }
        public async Task<BaseResponse<IEnumerable<YourPetsDTO>>> GetFavoritePets(string userid)
        {
            var favoriteProducts = _context.Favorites
                .Include(x => x.Pet)
                .Where(x => x.UserId == userid)
                .Select(x => new YourPetsDTO(
                    x.Pet.PhotoUrl,
                    x.Pet.Name,
                    x.Pet.Breed,
                    x.Pet.Gender
                ))
                .ToList();

            return new BaseResponse<IEnumerable<YourPetsDTO>>(true,"Success",favoriteProducts);
        }



    }
}
