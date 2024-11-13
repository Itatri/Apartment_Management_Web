﻿using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Authentication;
using Apartment_Management_Web.Models.Login;
using Apartment_Management_Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Apartment_Management_Web.Models.Login.LoginRequest;

namespace Apartment_Management_Web.Controllers
{
    [Route("api/UserPhongs")]
    [ApiController]
    public class UserPhongsController : ControllerBase
    {
        private readonly IUserPhongService _userPhongService;
        private readonly JwtSettings _jwtSettings;
        private readonly QlChungCuContext _dbContext;

        public UserPhongsController(IUserPhongService userPhongService, IOptions<JwtSettings> jwtSettings, QlChungCuContext dbContext)
        {
            _userPhongService = userPhongService;
            _jwtSettings = jwtSettings.Value;
            _dbContext = dbContext;
        }

        [HttpGet("TestDatabaseConnection")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {
                // Kiểm tra kết nối
                var canConnect = await _dbContext.Database.CanConnectAsync();

                if (canConnect)
                {
                    var connectionInfo = new
                    {
                        Server = _dbContext.Database.GetDbConnection().DataSource,
                        Database = _dbContext.Database.GetDbConnection().Database,
                        ConnectionState = _dbContext.Database.GetDbConnection().State.ToString()
                    };

                    return Ok(new
                    {
                        Message = "Kết nối cơ sở dữ liệu thành công.",
                        ConnectionInfo = connectionInfo
                    });
                }
                else
                {
                    return StatusCode(500, "Kết nối cơ sở dữ liệu không thành công do lý do không xác định.");
                }
            }
            catch (Exception ex)
            {
                // Kiểm tra lỗi nội bộ nếu có
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new
                {
                    Message = "Lỗi kết nối cơ sở dữ liệu.",
                    ErrorDetails = errorMessage // Cung cấp chi tiết lỗi từ Exception
                });
            }
        }



        // API lấy danh sách User phòng 
        [Authorize]
        [HttpGet("GetAllUserPhongs")]
        public async Task<ActionResult<IEnumerable<UserPhong>>> GetUserPhongs()
        {
            var userPhongs = await _userPhongService.GetAllUserPhongsAsync();
            return Ok(userPhongs);
        }


        // API lấy User phòng theo Id
        [HttpGet("GetUserPhongById")]
        [Authorize]
        public async Task<ActionResult<UserPhong>> GetUserPhong(string id)
        {
            var userPhong = await _userPhongService.GetUserPhongByIdAsync(id);
            if (userPhong == null)
            {
                return NotFound();
            }
            return Ok(userPhong);
        }


        // API kiển tra đăng nhập tài khoản
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var authenticationResult = await _userPhongService.AuthenticateAsync(loginModel.Id, loginModel.MatKhau);

            if (!authenticationResult.IsSuccess)
            {
                var response = new LoginResponse
                {
                    IsSuccess = false,
                    Message = authenticationResult.ErrorMessage,
                    Token = null,
                    User = null
                };
                return Unauthorized(response);
            }

            var token = GenerateJwtToken(authenticationResult.User);
            var successResponse = new LoginResponse
            {
                IsSuccess = true,
                Token = token,
                Message = "Đăng nhập thành công.",
                User = authenticationResult.User // Cung cấp thông tin người dùng nếu cần
            };

            return Ok(successResponse);
        }


        [HttpPut("UpdateUserPhong")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPhong(string id, [FromBody] UserPhong userPhong)
        {
            if (id != userPhong.Id)
            {
                return BadRequest(new UpdateUserResponse
                {
                    IsSuccess = false,
                    Message = "ID không khớp với thông tin người dùng."
                });
            }

            var result = await _userPhongService.UpdateUserPhongAsync(userPhong);
            if (!result)
            {
                return NotFound(new UpdateUserResponse
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy người dùng để cập nhật."
                });
            }

            return Ok(new UpdateUserResponse
            {
                IsSuccess = true,
                Message = "Cập nhật thông tin người dùng thành công.",
                UpdatedUser = userPhong // Trả về thông tin người dùng đã cập nhật
            });
        }


        // API xóa thông tin User phòng  
        [Authorize]
        [HttpDelete("DeleteUserPhong")]
        public async Task<IActionResult> DeleteUserPhong(string id)
        {
            var response = await _userPhongService.DeleteUserPhongAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response); // Trả về thông tin thành công
        }

        // API tạo thông tin User phòng mới 
        [Authorize]
        [HttpPost("CreateUserPhong")]
        public async Task<ActionResult<UserPhong>> CreateUserPhong(UserPhong userPhong)
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Tạo token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                  issuer: _jwtSettings.Issuer,
                  audience: _jwtSettings.Audience,
                  claims: claims,
                  expires: expires,
                  signingCredentials: creds);

            // Trả về token dưới dạng chuỗi
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
