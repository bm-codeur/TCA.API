namespace TCA.API.Models;

public class SuperviseurGroupe
{
    public const decimal SalaireBaseDefault = 9_000_000m;

    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int GroupeId { get; set; }
    public decimal SalaireBase { get; set; } = SalaireBaseDefault;

    public Groupe Groupe { get; set; } = null!;
}
