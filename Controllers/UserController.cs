using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebApi.Models.DTOs;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserRequestDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName!,
                Email = dto.Email!,
                PasswordHash = UserHelpers.HashPassword(dto.Password!),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var responseDto = new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
            return Ok(new ApiResponse<UserResponseDto>
            {
                Success = true,
                Message = "User registered successfully",
                Data = responseDto
            });
        }

        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserRequestDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName!,
                Email = dto.Email!,
                PasswordHash = UserHelpers.HashPassword(dto.Password!),
                Role = dto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var responseDto2 = new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
            return Ok(new ApiResponse<UserResponseDto>
            {
                Success = true,
                Message = "User created successfully",
                Data = responseDto2
            });
        }
    }
}