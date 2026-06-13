using TCA.API.Models;

namespace TCA.API.DTOs;

public class ChauffeurDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int CamionId { get; set; }
    public string? CamionNumero { get; set; }
    public decimal SalaireBase { get; set; }
}

public class CreateChauffeurDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int CamionId { get; set; }
    public decimal SalaireBase { get; set; } = Chauffeur.SalaireBaseDefault;
}

public class UpdateChauffeurDto : CreateChauffeurDto
{
}
