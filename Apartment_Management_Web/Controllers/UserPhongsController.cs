using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Apartment_Management_Web.Models;
using static Apartment_Management_Web.Models.Login.LoginRequest;
using Apartment_Management_Web.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apartment_Management_Web.Models.Login;

namespace Apartment_Management_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPhongsController : ControllerBase
    {
        private readonly IUserPhongService _userPhongService;

        public UserPhongsController(IUserPhongService userPhongService)
        {
            _userPhongService = userPhongService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPhong>>> GetUserPhongs()
        {
            var userPhongs = await _userPhongService.GetAllUserPhongsAsync();
            return Ok(userPhongs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserPhong>> GetUserPhong(string id)
        {
            var userPhong = await _userPhongService.GetUserPhongByIdAsync(id);
            if (userPhong == null)
            {
                return NotFound();
            }
            return Ok(userPhong);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var userPhong = await _userPhongService.AuthenticateAsync(loginModel.Id, loginModel.MatKhau);
            if (userPhong == null)
            {
                return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng." });
            }

            var token = GenerateJwtToken(userPhong);
            return Ok(new { token });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPhong(string id, UserPhong userPhong)
        {
            if (id != userPhong.Id)
            {
                return BadRequest();
            }

            var result = await _userPhongService.UpdateUserPhongAsync(userPhong);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPhong(string id)
        {
            var result = await _userPhongService.DeleteUserPhongAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UserPhong>> PostUserPhong(UserPhong userPhong)
        {
            var result = await _userPhongService.CreateUserPhongAsync(userPhong);
            if (!result)
            {
                return Conflict();
            }

            return CreatedAtAction(nameof(GetUserPhong), new { id = userPhong.Id }, userPhong);
        }

        private string GenerateJwtToken(UserPhong user)
        {
            // Thời gian hết hạn của token
            var expires = DateTime.UtcNow.AddDays(7); // Token có hiệu lực trong 7 ngày

            // Tạo danh sách các claim cho token
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id), // Đặt Id của người dùng làm claim
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Đặt một ID duy nhất cho token
    };

            // Tạo đối tượng SecurityKey từ chuỗi bảo mật (cần đảm bảo rằng khóa có độ dài tối thiểu 32 ký tự)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ItatriSuperSecretKeyThatIsAtLeast32CharactersLong!")); // Đảm bảo khóa đủ dài

            // Tạo token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7298", // Địa chỉ phát hành token
                audience: "https://localhost:7298", // Đối tượng sử dụng token
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            // Trả về token dưới dạng chuỗi
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
