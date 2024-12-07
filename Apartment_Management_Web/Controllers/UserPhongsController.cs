using Apartment_Management_Web.Interfaces;
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
        // API kiểm tra kết nối tới Database
        [HttpGet("TestDatabaseConnection")]
        public async Task<IActionResult> TestDatabaseConnection()
        {
            try
            {

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
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, new
                {
                    Message = "Lỗi kết nối cơ sở dữ liệu.",
                    ErrorDetails = errorMessage
                });
            }
        }



        // API lấy danh sách tài khoản phòng 
        [Authorize]
        [HttpGet("GetAllUserPhongs")]
        public async Task<ActionResult<IEnumerable<UserPhong>>> GetUserPhongs()
        {
            var userPhongs = await _userPhongService.GetAllUserPhongsAsync();
            return Ok(userPhongs);
        }


        // API lấy  tài khoản phòng theo Id
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

        // API lấy thông tin phòng theo mã phòng
        [HttpGet("GetPhongByMaPhong")]
        [Authorize]
        public async Task<ActionResult<Phong>> GetPhongByMaPhong(string maPhong)
        {
            var phong = await _userPhongService.GetPhongByMaPhongAsync(maPhong);
            if (phong == null)
            {
                return NotFound();
            }
            return Ok(phong);
        }



        // API kiểm tra đăng nhập vào trang web
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
                User = authenticationResult.User
            };

            return Ok(successResponse);
        }

        // API cập nhật tài khoản phòng
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
                UpdatedUser = userPhong
            });
        }


        // API xóa thông tin tài khoản phòng  
        [Authorize]
        [HttpDelete("DeleteUserPhong")]
        public async Task<IActionResult> DeleteUserPhong(string id)
        {
            var response = await _userPhongService.DeleteUserPhongAsync(id);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // API tạo thông tin tài khoản phòng mới 
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

        // Hàm tạo token xác thực đăng nhập cho tài khoản phòng
        private string GenerateJwtToken(UserPhong user)
        {

            var expires = DateTime.UtcNow.AddDays(7);


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                  issuer: _jwtSettings.Issuer,
                  audience: _jwtSettings.Audience,
                  claims: claims,
                  expires: expires,
                  signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
