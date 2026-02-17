using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebApi.Models.DTOs;
using WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReservationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _context.Reservations.ToListAsync();
            var reservationDtos = reservations.Select(r => new ReservationResponseDto
            {
                Id = r.Id,
                RoomName = _context.Rooms.Find(r.RoomId)?.Name ?? r.RoomId.ToString(),
                UserName = _context.Users.Find(r.UserId)?.FullName ?? r.UserId.ToString(),
                Purpose = r.Purpose,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status,
                ApprovedBy = r.ApproverId.HasValue ? _context.Users.Find(r.ApproverId.Value)?.FullName ?? string.Empty : string.Empty,
                ApprovedAt = r.ApprovedAt
            }).ToList();
            return Ok(new ApiResponse<List<ReservationResponseDto>>
            {
                Success = true,
                Message = "Reservations retrieved successfully",
                Data = reservationDtos
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserReservation(int id)
        {
            var userId = User.GetCurrentUserId();
            var reservations = await _context.Reservations.Where(r => r.UserId == id).ToListAsync();
            var reservationDtos = reservations.Select(r => new ReservationResponseDto
            {
                Id = r.Id,
                RoomName = _context.Rooms.Find(r.RoomId)?.Name ?? r.RoomId.ToString(),
                UserName = _context.Users.Find(r.UserId)?.FullName ?? r.UserId.ToString(),
                Purpose = r.Purpose,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status,
                ApprovedBy = r.ApproverId.HasValue ? _context.Users.Find(r.ApproverId.Value)?.FullName ?? string.Empty : string.Empty,
                ApprovedAt = r.ApprovedAt
            }).ToList();
            return Ok(new ApiResponse<List<ReservationResponseDto>>
            {
                Success = true,
                Message = "User reservations retrieved successfully",
                Data = reservationDtos
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
                return BadRequest("End time must be after start time.");

            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                UserId = User.GetCurrentUserId(),
                Purpose = dto.Purpose!,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            var user = await _context.Users.FindAsync(reservation.UserId);
            var responseDto = new ReservationResponseDto
            {
                Id = reservation.Id,
                RoomName = room?.Name ?? string.Empty,
                UserName = user?.FullName ?? string.Empty,
                Purpose = reservation.Purpose,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status,
                ApprovedBy = string.Empty,
                ApprovedAt = reservation.ApprovedAt
            };
            return Ok(new ApiResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Reservation created successfully",
                Data = responseDto
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update(int id, CreateReservationDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            if (reservation.UserId != User.GetCurrentUserId())
                return Unauthorized();

            reservation.RoomId = dto.RoomId;
            reservation.Purpose = dto.Purpose!;
            reservation.StartTime = dto.StartTime;
            reservation.EndTime = dto.EndTime;
            reservation.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            var user = await _context.Users.FindAsync(reservation.UserId);
            var responseDto = new ReservationResponseDto
            {
                Id = reservation.Id,
                RoomName = room?.Name ?? string.Empty,
                UserName = user?.FullName ?? string.Empty,
                Purpose = reservation.Purpose,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status,
                ApprovedBy = string.Empty,
                ApprovedAt = reservation.ApprovedAt
            };

            return Ok(new ApiResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Reservation updated successfully",
                Data = responseDto
            });
        }

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Cancel(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            if (reservation.UserId != User.GetCurrentUserId())
                return Unauthorized();

            reservation.Status = ReservationStatus.Cancelled;
            await _context.SaveChangesAsync();

            var responseDto = new ReservationResponseDto
            {
                Id = reservation.Id,
                RoomName = _context.Rooms.Find(reservation.RoomId)?.Name ?? reservation.RoomId.ToString(),
                UserName = _context.Users.Find(reservation.UserId)?.FullName ?? reservation.UserId.ToString(),
                Purpose = reservation.Purpose,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status,
                ApprovedBy = string.Empty,
                ApprovedAt = reservation.ApprovedAt
            };

            return Ok(new ApiResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Reservation cancelled successfully",
                Data = responseDto
            });
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.Status = ReservationStatus.Approved;
            reservation.ApproverId = User.GetCurrentUserId();
            reservation.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            var user = await _context.Users.FindAsync(reservation.UserId);
            var approver = await _context.Users.FindAsync(reservation.ApproverId);
            var responseDto = new ReservationResponseDto
            {
                Id = reservation.Id,
                RoomName = room?.Name ?? string.Empty,
                UserName = user?.FullName ?? string.Empty,
                Purpose = reservation.Purpose,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status,
                ApprovedBy = approver?.FullName ?? string.Empty,
                ApprovedAt = reservation.ApprovedAt
            };
            return Ok(new ApiResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Reservation approved successfully",
                Data = responseDto
            });
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.Status = ReservationStatus.Rejected;
            await _context.SaveChangesAsync();

            var responseDto = new ReservationResponseDto
            {
                Id = reservation.Id,
                RoomName = _context.Rooms.Find(reservation.RoomId)?.Name ?? reservation.RoomId.ToString(),
                UserName = _context.Users.Find(reservation.UserId)?.FullName ?? reservation.UserId.ToString(),
                Purpose = reservation.Purpose,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = reservation.Status,
                ApprovedBy = string.Empty,
                ApprovedAt = reservation.ApprovedAt
            };

            return Ok(new ApiResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Reservation rejected successfully",
                Data = responseDto
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Reservation deleted successfully",
                Data = null
            });
        }
    }
}