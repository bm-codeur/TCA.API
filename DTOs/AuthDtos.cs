namespace TCA.API.DTOs;

public class RegisterDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public int? ChauffeurId { get; set; }
    public int? SuperviseurGroupeId { get; set; }
    public int? SuperviseurZoneId { get; set; }
    public int? SuperviseurGeneralId { get; set; }
}

public class LoginDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public int? LinkedEntityId { get; set; }
}
