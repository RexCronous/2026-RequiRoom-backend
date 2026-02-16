using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _context.Rooms.ToListAsync();
            var roomDtos = rooms.Select(r => new RoomResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                IsAvailable = r.IsAvailable
            }).ToList();
            return Ok(new ApiResponse<List<RoomResponseDto>>
            {
                Success = true,
                Message = "Rooms retrieved successfully",
                Data = roomDtos
            });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Room not found"
                });
            }

            var responseDto = new RoomResponseDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Location = room.Location,
                IsAvailable = room.IsAvailable
            };

            return Ok(new ApiResponse<RoomResponseDto>
            {
                Success = true,
                Message = "Room retrieved successfully",
                Data = responseDto
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            var room = new Room
            {
                Name = dto.Name!,
                Capacity = dto.Capacity,
                Location = dto.Location!,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var responseDto = new RoomResponseDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Location = room.Location,
                IsAvailable = room.IsAvailable
            };
            return Ok(new ApiResponse<RoomResponseDto>
            {
                Success = true,
                Message = "Room created successfully",
                Data = responseDto
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, CreateRoomDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Room not found"
                });
            }

            room.Name = dto.Name!;
            room.Capacity = dto.Capacity;
            room.Location = dto.Location!;
            room.IsAvailable = true;
            room.UpdatedAt = DateTime.UtcNow;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();

            var responseDto = new RoomResponseDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Location = room.Location,
                IsAvailable = room.IsAvailable
            };
            return Ok(new ApiResponse<RoomResponseDto>
            {
                Success = true,
                Message = "Room updated successfully",
                Data = responseDto
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Room not found"
                });
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Room deleted successfully"
            });
        }
    }
}