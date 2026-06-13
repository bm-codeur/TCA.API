namespace TCA.API.Models;

public static class Roles
{
    public const string Admin = "Admin";
    public const string SuperviseurGeneral = "SuperviseurGeneral";
    public const string SuperviseurZone = "SuperviseurZone";
    public const string SuperviseurGroupe = "SuperviseurGroupe";
    public const string Chauffeur = "Chauffeur";

    public static readonly string[] All =
    [
        Admin,
        SuperviseurGeneral,
        SuperviseurZone,
        SuperviseurGroupe,
        Chauffeur
    ];
}
