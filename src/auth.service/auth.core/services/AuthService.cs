using auth.core.repositories;
using auth.core.services.interfaces;
using auth.shared.dtos;
using System;
using System.Threading.Tasks;

namespace auth.core.services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> SignInAsync(LoginRequest loginRequest)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email);

            // TODO: Replace with a secure password hashing and verification mechanism like BCrypt.
            if (user == null || loginRequest.Password != user.PasswordHash)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            var token = _tokenService.CreateToken(user);

            return new AuthResult
            {
                Success = true,
                Token = token
            };
        }
        catch (Exception)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "An error occurred during sign in"
            };
        }
    }

    public async Task<AuthResult> SignUpAsync(RegisterRequest registerRequest)
    {
        try
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                return new AuthResult { Success = false, ErrorMessage = "User with this email already exists." };
            }

            var verificationCode = new Random().Next(10000, 99999).ToString();

            // TODO: Replace with a secure password hashing mechanism like BCrypt.
            var passwordHash = registerRequest.Password;

            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                PasswordHash = passwordHash,
                Code = verificationCode,
                Active = true,
                UserRole = shared.enums.Role.User,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
                IsVerify = false
            };

            await _userRepository.CreateAsync(user);
            await _userRepository.SetVerificationCodeAsync(registerRequest.Email, verificationCode);


            return new AuthResult
            {
                Success = true,
                Message = "User created successfully. Please check your email for verification code."
            };
        }
        catch (Exception)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "An error occurred during sign up"
            };
        }
    }

    public async Task<AuthResult> VerificationAsync(VerificationRequest verificationRequest)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(verificationRequest.Email);
            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "User not found"
                };
            }
            if (user.Code != verificationRequest.Code)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = "Invalid verification code"
                };
            }
            await _userRepository.SetEmailVerifiedAsync(verificationRequest.Email);

            var token = _tokenService.CreateToken(user);

            return new AuthResult { Success = true, Token = token };
        }
        catch (Exception)
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
