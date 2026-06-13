namespace TCA.API.Models;

public class Camion
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int GroupeId { get; set; }
    public bool EstDisponible { get; set; } = true;

    public Groupe Groupe { get; set; } = null!;
    public Chauffeur? Chauffeur { get; set; }
    public ICollection<Chargement> Chargements { get; set; } = new List<Chargement>();
}
