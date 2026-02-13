using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;
using System;
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

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
                return BadRequest("End time must be after start time.");

            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                UserId = UserHelpers.GetCurrentUserId(),
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
                Status = (WebApi.Models.ReservationStatus)reservation.Status,
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

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound();

            reservation.Status = ReservationStatus.Approved;
            reservation.ApproverId = UserHelpers.GetCurrentUserId();
            reservation.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var room2 = await _context.Rooms.FindAsync(reservation.RoomId);
            var user2 = await _context.Users.FindAsync(reservation.UserId);
            var approver = await _context.Users.FindAsync(reservation.ApproverId);
            var responseDto2 = new ReservationResponseDto
            {
                Id = reservation.Id,
                RoomName = room2?.Name ?? string.Empty,
                UserName = user2?.FullName ?? string.Empty,
                Purpose = reservation.Purpose,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Status = (WebApi.Models.ReservationStatus)reservation.Status,
                ApprovedBy = approver?.FullName ?? string.Empty,
                ApprovedAt = reservation.ApprovedAt
            };
            return Ok(new ApiResponse<ReservationResponseDto>
            {
                Success = true,
                Message = "Reservation approved successfully",
                Data = responseDto2
            });
        }
    }
}