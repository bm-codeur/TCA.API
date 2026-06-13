namespace TCA.API.Models;

public class Groupe
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int ZoneId { get; set; }

    public Zone Zone { get; set; } = null!;
    public ICollection<Camion> Camions { get; set; } = new List<Camion>();
    public ICollection<SuperviseurGroupe> SuperviseursGroupe { get; set; } = new List<SuperviseurGroupe>();
}
