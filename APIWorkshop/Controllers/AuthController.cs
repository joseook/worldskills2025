using APIWorkshop.Controllers.Dto;
using APIWorkshop.Model;
using APIWorkshop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APIWorkshop.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Login(loginDto.Email, loginDto.Password);

            if (!result.success)
                return Unauthorized(new { message = result.message });

            // Gerar token JWT
            var token = GenerateJwtToken(result.user);

            return Ok(new 
            { 
                token,
                user = new
                {
                    id = result.user.Id,
                    email = result.user.Email,
                    firstName = result.user.FirstName,
                    lastName = result.user.LastName
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.Register(
                registerDto.Email,
                registerDto.Password,
                registerDto.FirstName,
                registerDto.LastName);

            if (!result.success)
                return BadRequest(new { message = result.message });

            // Gerar token JWT
            var token = GenerateJwtToken(result.user);

            return Ok(new 
            { 
                token,
                user = new
                {
                    id = result.user.Id,
                    email = result.user.Email,
                    firstName = result.user.FirstName,
                    lastName = result.user.LastName
                }
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ForgotPassword(forgotPasswordDto.Email);

            if (!result.success)
                return BadRequest(new { message = result.message });

            // Em um ambiente de produção, você enviaria um e-mail com o link contendo o token
            // Para fins de teste, estamos retornando o token na resposta
            return Ok(new { 
                message = "Um e-mail com instruções para redefinir sua senha foi enviado.",
                // O token seria enviado por e-mail em um ambiente real
                token = result.token 
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPassword(
                resetPasswordDto.Email,
                resetPasswordDto.Token,
                resetPasswordDto.Password);

            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = result.message });
        }
        
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "bellecroissantdefaultsecretkey2025");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
