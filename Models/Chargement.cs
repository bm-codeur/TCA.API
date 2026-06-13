namespace TCA.API.Models;

public class Chargement
{
    public int Id { get; set; }
    public int CamionId { get; set; }
    public int ZoneId { get; set; }
    public DateTime HeureDepart { get; set; }
    public DateTime? HeureRetour { get; set; }
    public decimal Kilometre { get; set; }
    public decimal Carburant { get; set; }
    public DateTime DateChargement { get; set; }
    public bool EstRetourne { get; set; }

    public Camion Camion { get; set; } = null!;
    public Zone Zone { get; set; } = null!;
}
