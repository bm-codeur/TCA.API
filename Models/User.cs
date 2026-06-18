namespace TCA.API.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public int? ChauffeurId { get; set; }
    public Chauffeur? Chauffeur { get; set; }

    public int? SuperviseurGroupeId { get; set; }
    public SuperviseurGroupe? SuperviseurGroupe { get; set; }

    public int? SuperviseurZoneId { get; set; }
    public SuperviseurZone? SuperviseurZone { get; set; }

    public int? SuperviseurGeneralId { get; set; }
    public SuperviseurGeneral? SuperviseurGeneral { get; set; }
}
