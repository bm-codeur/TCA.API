using TCA.API.Models;

namespace TCA.API.DTOs;

public class PrimeChauffeurDto
{
    public int ChauffeurId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int NombreChargements { get; set; }
    public decimal PrimeUnitaire { get; set; }
    public decimal PrimeTotal { get; set; }
    public decimal SalaireBase { get; set; }
    public decimal TotalRemuneration { get; set; }
}

public class PrimeSuperviseurGroupeDto
{
    public int SuperviseurGroupeId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string GroupeNumero { get; set; } = string.Empty;
    public int NombreChargements { get; set; }
    public decimal PrimeUnitaire { get; set; }
    public decimal PrimeTotal { get; set; }
    public decimal SalaireBase { get; set; }
    public decimal TotalRemuneration { get; set; }
}

public class PrimeSuperviseurZoneDto
{
    public int SuperviseurZoneId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string ZoneNom { get; set; } = string.Empty;
    public int NombreChargements { get; set; }
    public decimal PrimeUnitaire { get; set; }
    public decimal PrimeTotal { get; set; }
    public decimal SalaireBase { get; set; }
    public decimal TotalRemuneration { get; set; }
}

public class PrimeSuperviseurGeneralDto
{
    public int SuperviseurGeneralId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int NombreChargements { get; set; }
    public decimal PrimeUnitaire { get; set; } = SuperviseurGeneral.PrimeParChargement;
    public decimal PrimeTotal { get; set; }
    public decimal SalaireBase { get; set; }
    public decimal TotalRemuneration { get; set; }
}

public class PrimesRequestDto
{
    public int? Mois { get; set; }
    public int? Annee { get; set; }
}
