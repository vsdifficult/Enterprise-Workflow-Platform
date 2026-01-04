using auth.core.repositories;
using auth.shared.dtos;
using HostMarket.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth.core.services;

public class AuthService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResult> SignInAsync(LoginDto loginDTO)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(loginDTO.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            var token = await GenerateTokenAsync(user.Id);

            return new AuthResult
            {
                Success = true,
                Token = token.ToString()
            };
        }

        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "An error occurred during sign in"
            };
        }
    }


    public async Task<AuthResult> SignUpAsync(RegisterDto registerDto)
    {
        try
        {

            var usr = await _userRepository.GetByEmailAsync(registerDto.Email) ?? 
                throw new Exception($"User with {registerDto.Email} not found"); 

            var verificationCode = new Random().Next(10000, 99999).ToString();
            await _userRepository.SetVerificationCodeAsync(registerDto.Email, verificationCode);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var userId = Guid.NewGuid();
            var user = new UserDto
            {
                Id = userId,
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Code = verificationCode,
                Active = true,
                UserRole = shared.enums.Role.User,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
                IsVerify = false
            };

            await _userRepository.CreateAsync(user);

            var token = await GenerateTokenAsync(userId);

            return new AuthResult
            {
                Success = true,
                Token = token
            };
        }

        catch (Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "An error occurred during sign up"
            };
        }
    }

    private async Task<string> GenerateTokenAsync(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:JwtSecret"] ?? string.Empty);

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userId.ToString())
        };

        var user = await _userRepository.GetByIdAsync(userId);
        claims.Add(new Claim("UserName", user.Name));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<AuthResult> VerificationAsync(VerificationDto verificationDto)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(verificationDto.Email);
            if (user == null)
            {
                return new AuthResult 
                { 
                    Success = false, 
                    ErrorMessage = "User not found" 
                };
            }
            if (user.Code != verificationDto.Code)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "Invalid verification code"
                };
            }
            await _userRepository.SetEmailVerifiedAsync(verificationDto.Email);

            return new AuthResult { Success = true };
        }
        catch (Exception ex)
        {
            return new AuthResult 
            { 
                Success = false, 
                ErrorMessage = "An error occurred during verification" 
            };
        }

    }

    public async Task<AuthResult> DeleteAsync(Guid userid)
    {
        var result = await _userRepository.DeleteAsync(userid);
        return new AuthResult { Success = result };
    }

}