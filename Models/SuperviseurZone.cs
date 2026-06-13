namespace TCA.API.Models;

public class SuperviseurZone
{
    public const decimal SalaireBaseDefault = 14_000_000m;

    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int ZoneId { get; set; }
    public decimal SalaireBase { get; set; } = SalaireBaseDefault;

    public Zone Zone { get; set; } = null!;
}
