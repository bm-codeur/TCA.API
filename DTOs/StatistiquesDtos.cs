namespace TCA.API.DTOs;

public class StatistiquesJournalieresDto
{
    public DateTime Date { get; set; }
    public int TotalChargements { get; set; }
    public int ChargementsRetournes { get; set; }
    public int ChargementsEnCours { get; set; }
    public decimal TotalKilometres { get; set; }
    public decimal TotalCarburant { get; set; }
    public decimal TotalRevenus { get; set; }
    public List<StatistiqueZoneDto> ParZone { get; set; } = new();
}

public class StatistiquesMensuellesDto
{
    public int Mois { get; set; }
    public int Annee { get; set; }
    public int TotalChargements { get; set; }
    public int ChargementsRetournes { get; set; }
    public decimal TotalKilometres { get; set; }
    public decimal TotalCarburant { get; set; }
    public decimal TotalRevenus { get; set; }
    public List<StatistiqueZoneDto> ParZone { get; set; } = new();
    public List<StatistiqueGroupeDto> ParGroupe { get; set; } = new();
}

public class StatistiqueZoneDto
{
    public int ZoneId { get; set; }
    public string ZoneNom { get; set; } = string.Empty;
    public int NombreChargements { get; set; }
    public decimal TotalKilometres { get; set; }
    public decimal TotalCarburant { get; set; }
    public decimal TotalRevenus { get; set; }
}

public class StatistiqueGroupeDto
{
    public int GroupeId { get; set; }
    public string GroupeNumero { get; set; } = string.Empty;
    public int NombreChargements { get; set; }
    public decimal TotalKilometres { get; set; }
    public decimal TotalCarburant { get; set; }
}
