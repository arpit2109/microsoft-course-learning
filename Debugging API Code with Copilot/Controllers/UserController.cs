using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Simulated database
        private static readonly List<User> _users = new List<User>();

        // Optimized the GET /users endpoint by implementing pagination
        // This avoids performance bottlenecks when retrieving large amounts of records
        [HttpGet]
        public IActionResult GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pagedUsers = _users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(pagedUsers);
            }
            catch (Exception ex)
            {
                // Implement try-catch blocks to handle unhandled exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _users.FirstOrDefault(u => u.Id == id);

                // Error handling for failed database lookups
                if (user == null)
                {
                    return NotFound($"User with ID {id} could not be found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Implement try-catch blocks to handle unhandled exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            try
            {
                // Add validation to ensure only valid user data is processed
                // Check for missing validation for user input fields (e.g., empty names or invalid emails)
                if (string.IsNullOrWhiteSpace(user.Name))
                {
                    return BadRequest("User name cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@"))
                {
                    return BadRequest("A valid email address is required.");
                }

                user.Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;
                _users.Add(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Implement try-catch blocks to handle unhandled exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
