namespace TCA.API.DTOs;

public class ChargementDto
{
    public int Id { get; set; }
    public int CamionId { get; set; }
    public string? CamionNumero { get; set; }
    public int ZoneId { get; set; }
    public string? ZoneNom { get; set; }
    public DateTime HeureDepart { get; set; }
    public DateTime? HeureRetour { get; set; }
    public decimal Kilometre { get; set; }
    public decimal Carburant { get; set; }
    public DateTime DateChargement { get; set; }
    public bool EstRetourne { get; set; }
}

public class CreateDepartDto
{
    public int CamionId { get; set; }
    public int ZoneId { get; set; }
    public DateTime? HeureDepart { get; set; }
}

public class EnregistrerRetourDto
{
    public DateTime? HeureRetour { get; set; }
}
