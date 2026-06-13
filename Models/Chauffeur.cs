namespace TCA.API.Models;

public class Chauffeur
{
    public const decimal SalaireBaseDefault = 4_000_000m;

    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int CamionId { get; set; }
    public decimal SalaireBase { get; set; } = SalaireBaseDefault;

    public Camion Camion { get; set; } = null!;
}
