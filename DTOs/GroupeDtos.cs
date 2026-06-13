namespace TCA.API.DTOs;

public class GroupeDto
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public int ZoneId { get; set; }
    public string? ZoneNom { get; set; }
}

public class CreateGroupeDto
{
    public string Numero { get; set; } = string.Empty;
    public int ZoneId { get; set; }
}

public class UpdateGroupeDto : CreateGroupeDto
{
}
