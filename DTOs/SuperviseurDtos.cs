using TCA.API.Models;

namespace TCA.API.DTOs;

public class SuperviseurGeneralDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public decimal SalaireBase { get; set; }
}

public class CreateSuperviseurGeneralDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public decimal SalaireBase { get; set; } = SuperviseurGeneral.SalaireBaseDefault;
}

public class UpdateSuperviseurGeneralDto : CreateSuperviseurGeneralDto
{
}

public class SuperviseurZoneDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int ZoneId { get; set; }
    public string? ZoneNom { get; set; }
    public decimal SalaireBase { get; set; }
}

public class CreateSuperviseurZoneDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int ZoneId { get; set; }
    public decimal SalaireBase { get; set; } = SuperviseurZone.SalaireBaseDefault;
}

public class UpdateSuperviseurZoneDto : CreateSuperviseurZoneDto
{
}

public class SuperviseurGroupeDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int GroupeId { get; set; }
    public string? GroupeNumero { get; set; }
    public decimal SalaireBase { get; set; }
}

public class CreateSuperviseurGroupeDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public int GroupeId { get; set; }
    public decimal SalaireBase { get; set; } = SuperviseurGroupe.SalaireBaseDefault;
}

public class UpdateSuperviseurGroupeDto : CreateSuperviseurGroupeDto
{
}
