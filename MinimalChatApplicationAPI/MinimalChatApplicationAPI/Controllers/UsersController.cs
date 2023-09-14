using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalChatApplicationAPI.Data;
using MinimalChatApplicationAPI.Models;

namespace MinimalChatApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MinimalChatDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UsersController(MinimalChatDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User request)
        {
            // Validate the request
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Validation failed" });
            }

            // Check if the email is already registered
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return Conflict(new { error = "Email is already registered" });
            }

            // Hash the password
            var hashedPassword = _passwordHasher.HashPassword(null, request.Password);

            // Create a new user
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = hashedPassword
            };

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return a success response
            return Ok(new
            {
                userId = user.Id,
                name = user.Name,
                email = user.Email
            });
        }
    

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

        return Ok(new { message = "User deleted successfully" });
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
