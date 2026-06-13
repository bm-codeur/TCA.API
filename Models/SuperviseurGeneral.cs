namespace TCA.API.Models;

public class SuperviseurGeneral
{
    public const decimal SalaireBaseDefault = 20_000_000m;
    public const decimal PrimeParChargement = 2_300m;

    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public decimal SalaireBase { get; set; } = SalaireBaseDefault;
}
