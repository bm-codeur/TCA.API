using Microsoft.Extensions.Options;
using TCA.API.Authentication;
using TCA.API.DTOs;
using TCA.API.Models;
using TCA.API.Repositories;

namespace TCA.API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (!Roles.All.Contains(dto.Role))
            throw new ArgumentException($"Rôle inválide. Rôles autorisés : {string.Join(", ", Roles.All)}");

        var existing = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existing is not null)
            throw new InvalidOperationException("Ce nom d'utilisateur existe déjà.");

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        return BuildAuthResponse(user);
    }

    private AuthResponseDto BuildAuthResponse(User user) => new()
    {
        Token = _tokenService.GenerateToken(user),
        Username = user.Username,
        Role = user.Role,
        ExpiresAt = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationHours)
    };
}
