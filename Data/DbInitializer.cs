using Microsoft.EntityFrameworkCore;
using TCA.API.Models;

namespace TCA.API.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Zones.AnyAsync())
            return;

        var zones = SeedZones(context);
        await context.SaveChangesAsync();

        var groupes = SeedGroupes(context, zones);
        await context.SaveChangesAsync();

        var camions = SeedCamions(context, groupes);
        await context.SaveChangesAsync();

        var chauffeurs = SeedChauffeurs(context, camions);
        var superviseursGroupe = SeedSuperviseursGroupe(context, groupes);
        var superviseursZone = SeedSuperviseursZone(context, zones);
        var superviseurGeneral = SeedSuperviseurGeneral(context);
        SeedAdminUser(context);

        await context.SaveChangesAsync();

        // Seed test accounts linked to physical entities
        SeedUserAccounts(context, chauffeurs, superviseursGroupe, superviseursZone, superviseurGeneral);
        await context.SaveChangesAsync();
    }

    private static List<Zone> SeedZones(ApplicationDbContext context)
    {
        var zones = new List<Zone>
        {
            new()
            {
                Nom = "Bankoh",
                Distance = 25,
                TarifChargement = 4_500_000,
                ToursMaxParJour = 4,
                ChargementMaxMois = 104,
                ChargementMaxMoisChauffeur = 104,
                ChargementMaxMoisGroupe = 1040,
                ChargementMaxMoisZone = 2080,
                PrimeChauffeurParChargement = 43_000,
                PrimeSuperviseurGroupeParChargement = 9_000,
                PrimeSuperviseurZoneParChargement = 7_000
            },
            new()
            {
                Nom = "Djoumaya",
                Distance = 37,
                TarifChargement = 5_700_000,
                ToursMaxParJour = 3,
                ChargementMaxMois = 78,
                ChargementMaxMoisChauffeur = 78,
                ChargementMaxMoisGroupe = 780,
                ChargementMaxMoisZone = 1560,
                PrimeChauffeurParChargement = 58_000,
                PrimeSuperviseurGroupeParChargement = 12_000,
                PrimeSuperviseurZoneParChargement = 9_500
            },
            new()
            {
                Nom = "Kalima",
                Distance = 22,
                TarifChargement = 4_200_000,
                ToursMaxParJour = 5,
                ChargementMaxMois = 130,
                ChargementMaxMoisChauffeur = 130,
                ChargementMaxMoisGroupe = 1300,
                ChargementMaxMoisZone = 2600,
                PrimeChauffeurParChargement = 35_000,
                PrimeSuperviseurGroupeParChargement = 7_500,
                PrimeSuperviseurZoneParChargement = 5_500
            },
            new()
            {
                Nom = "Timbi",
                Distance = 55,
                TarifChargement = 7_500_000,
                ToursMaxParJour = 2,
                ChargementMaxMois = 52,
                ChargementMaxMoisChauffeur = 52,
                ChargementMaxMoisGroupe = 520,
                ChargementMaxMoisZone = 1040,
                PrimeChauffeurParChargement = 88_000,
                PrimeSuperviseurGroupeParChargement = 18_000,
                PrimeSuperviseurZoneParChargement = 14_000
            },
            new()
            {
                Nom = "Soribaya",
                Distance = 35,
                TarifChargement = 5_500_000,
                ToursMaxParJour = 3,
                ChargementMaxMois = 78,
                ChargementMaxMoisChauffeur = 78,
                ChargementMaxMoisGroupe = 780,
                ChargementMaxMoisZone = 1560,
                PrimeChauffeurParChargement = 58_000,
                PrimeSuperviseurGroupeParChargement = 12_000,
                PrimeSuperviseurZoneParChargement = 9_500
            }
        };

        context.Zones.AddRange(zones);
        return zones;
    }

    private static List<Groupe> SeedGroupes(ApplicationDbContext context, List<Zone> zones)
    {
        var zoneByName = zones.ToDictionary(z => z.Nom, z => z);

        var affectations = new (string numero, string zone)[]
        {
            ("Groupe 01", "Bankoh"),
            ("Groupe 02", "Bankoh"),
            ("Groupe 03", "Djoumaya"),
            ("Groupe 04", "Djoumaya"),
            ("Groupe 05", "Kalima"),
            ("Groupe 06", "Kalima"),
            ("Groupe 07", "Timbi"),
            ("Groupe 08", "Timbi"),
            ("Groupe 09", "Soribaya"),
            ("Groupe 10", "Soribaya")
        };

        var groupes = affectations.Select(a => new Groupe
        {
            Numero = a.numero,
            ZoneId = zoneByName[a.zone].Id
        }).ToList();

        context.Groupes.AddRange(groupes);
        return groupes;
    }

    private static List<Camion> SeedCamions(ApplicationDbContext context, List<Groupe> groupes)
    {
        var camions = new List<Camion>();
        var camionIndex = 1;

        foreach (var groupe in groupes)
        {
            for (var i = 0; i < 10; i++)
            {
                camions.Add(new Camion
                {
                    Numero = $"CAM-{camionIndex:D3}",
                    GroupeId = groupe.Id,
                    EstDisponible = true
                });
                camionIndex++;
            }
        }

        context.Camions.AddRange(camions);
        return camions;
    }

    private static List<Chauffeur> SeedChauffeurs(ApplicationDbContext context, List<Camion> camions)
    {
        var chauffeurs = camions.Select((camion, index) => new Chauffeur
        {
            Nom = $"Chauffeur{index + 1:D3}",
            Prenom = $"Prenom{index + 1:D3}",
            CamionId = camion.Id,
            SalaireBase = Chauffeur.SalaireBaseDefault
        }).ToList();

        context.Chauffeurs.AddRange(chauffeurs);
        return chauffeurs;
    }

    private static List<SuperviseurGroupe> SeedSuperviseursGroupe(ApplicationDbContext context, List<Groupe> groupes)
    {
        var superviseurs = groupes.Select((groupe, index) => new SuperviseurGroupe
        {
            Nom = $"SupGroupe{index + 1:D2}",
            Prenom = $"Prenom{index + 1:D2}",
            GroupeId = groupe.Id,
            SalaireBase = SuperviseurGroupe.SalaireBaseDefault
        }).ToList();

        context.SuperviseursGroupe.AddRange(superviseurs);
        return superviseurs;
    }

    private static List<SuperviseurZone> SeedSuperviseursZone(ApplicationDbContext context, List<Zone> zones)
    {
        var superviseurs = zones.Select((zone, index) => new SuperviseurZone
        {
            Nom = $"SupZone{index + 1:D2}",
            Prenom = $"Prenom{index + 1:D2}",
            ZoneId = zone.Id,
            SalaireBase = SuperviseurZone.SalaireBaseDefault
        }).ToList();

        context.SuperviseursZone.AddRange(superviseurs);
        return superviseurs;
    }

    private static SuperviseurGeneral SeedSuperviseurGeneral(ApplicationDbContext context)
    {
        var sup = new SuperviseurGeneral
        {
            Nom = "Diallo",
            Prenom = "Mamadou",
            SalaireBase = SuperviseurGeneral.SalaireBaseDefault
        };
        context.SuperviseursGeneral.Add(sup);
        return sup;
    }

    private static void SeedAdminUser(ApplicationDbContext context)
    {
        context.Users.Add(new User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = Roles.Admin
        });
    }

    private static void SeedUserAccounts(
        ApplicationDbContext context,
        List<Chauffeur> chauffeurs,
        List<SuperviseurGroupe> superviseursGroupe,
        List<SuperviseurZone> superviseursZone,
        SuperviseurGeneral superviseurGeneral)
    {
        // 1. Chauffeur account
        if (chauffeurs.Any())
        {
            context.Users.Add(new User
            {
                Username = "chauffeur001",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Chauffeur@123"),
                Role = Roles.Chauffeur,
                ChauffeurId = chauffeurs[0].Id
            });
        }

        // 2. Groupe Supervisor account
        if (superviseursGroupe.Any())
        {
            context.Users.Add(new User
            {
                Username = "sup_groupe01",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Groupe@123"),
                Role = Roles.SuperviseurGroupe,
                SuperviseurGroupeId = superviseursGroupe[0].Id
            });
        }

        // 3. Zone Supervisor account
        if (superviseursZone.Any())
        {
            context.Users.Add(new User
            {
                Username = "sup_zone_bankoh",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Zone@123"),
                Role = Roles.SuperviseurZone,
                SuperviseurZoneId = superviseursZone[0].Id
            });
        }

        // 4. General Supervisor account
        context.Users.Add(new User
        {
            Username = "sup_general",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("General@123"),
            Role = Roles.SuperviseurGeneral,
            SuperviseurGeneralId = superviseurGeneral.Id
        });
    }
}
