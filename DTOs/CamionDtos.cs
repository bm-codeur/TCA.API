namespace TCA.API.DTOs;

public class CamionDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int GroupeId { get; set; }
    public string? GroupeNumero { get; set; }
    public bool EstDisponible { get; set; }
}

public class CreateCamionDto
{
    public string Numero { get; set; } = string.Empty;
    public int GroupeId { get; set; }
    public bool EstDisponible { get; set; } = true;
}

public class UpdateCamionDto : CreateCamionDto
{
}
