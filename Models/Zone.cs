namespace TCA.API.Models;

public class Zone
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public decimal Distance { get; set; }
    public decimal TarifChargement { get; set; }
    public int ToursMaxParJour { get; set; }
    public int ChargementMaxMois { get; set; } // Legacy / general limit
    public int ChargementMaxMoisChauffeur { get; set; }
    public int ChargementMaxMoisGroupe { get; set; }
    public int ChargementMaxMoisZone { get; set; }
    public decimal PrimeChauffeurParChargement { get; set; }
    public decimal PrimeSuperviseurGroupeParChargement { get; set; }
    public decimal PrimeSuperviseurZoneParChargement { get; set; }

    public ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();
    public ICollection<Chargement> Chargements { get; set; } = new List<Chargement>();
    public ICollection<SuperviseurZone> SuperviseursZone { get; set; } = new List<SuperviseurZone>();
}
