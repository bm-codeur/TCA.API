namespace TCA.API.DTOs;

public class ZoneDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public decimal Distance { get; set; }
    public decimal TarifChargement { get; set; }
    public int ToursMaxParJour { get; set; }
    public int ChargementMaxMois { get; set; }
    public decimal PrimeChauffeurParChargement { get; set; }
    public decimal PrimeSuperviseurGroupeParChargement { get; set; }
    public decimal PrimeSuperviseurZoneParChargement { get; set; }
}

public class CreateZoneDto
{
    public string Nom { get; set; } = string.Empty;
    public decimal Distance { get; set; }
    public decimal TarifChargement { get; set; }
    public int ToursMaxParJour { get; set; }
    public int ChargementMaxMois { get; set; }
    public decimal PrimeChauffeurParChargement { get; set; }
    public decimal PrimeSuperviseurGroupeParChargement { get; set; }
    public decimal PrimeSuperviseurZoneParChargement { get; set; }
}

public class UpdateZoneDto : CreateZoneDto
{
}
