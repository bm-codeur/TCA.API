# TCA.API — Gestion Logistique Minière

API ASP.NET Core 8 pour la gestion logistique minière (zones, groupes, camions, chargements, primes).

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL

## Démarrage rapide

```bash
cd TCA.API

# Restaurer les packages
dotnet restore

# Créer la base et appliquer les migrations
dotnet ef database update

# Lancer l'API
dotnet run
```

Swagger : http://localhost:5000/swagger

## Configuration

Modifier `appsettings.json` :

- **ConnectionStrings:DefaultConnection** — chaîne PostgreSQL
- **JwtSettings** — clé secrète, issuer, audience, durée du token (24h)

## Compte admin par défaut

- **Username** : `admin`
- **Password** : `Admin@123`

## Endpoints principaux

| Méthode | Route | Description |
|---------|-------|-------------|
| POST | `/api/auth/register` | Inscription |
| POST | `/api/auth/login` | Connexion JWT |
| GET/POST/PUT/DELETE | `/api/zones` | CRUD zones |
| GET/POST/PUT/DELETE | `/api/groupes` | CRUD groupes |
| GET/POST/PUT/DELETE | `/api/camions` | CRUD camions |
| GET/POST/PUT/DELETE | `/api/chauffeurs` | CRUD chauffeurs |
| POST | `/api/chargements/depart` | Enregistrer un départ |
| PUT | `/api/chargements/{id}/retour` | Enregistrer un retour |
| GET | `/api/statistiques/journalieres` | Stats du jour |
| GET | `/api/statistiques/mensuelles` | Stats mensuelles |
| GET | `/api/primes/chauffeurs` | Primes chauffeurs |
| GET | `/api/primes/superviseurs-groupe` | Primes superviseurs groupe |
| GET | `/api/primes/superviseurs-zone` | Primes superviseurs zone |
| GET | `/api/primes/superviseur-general` | Prime superviseur général |

## Règles métier (Chargements)

1. Pas de départ après **17h30**
2. Retour obligatoire avant un nouveau départ
3. **Carburant** = distance aller-retour × 2 L/km
4. Respect du **ToursMaxParJour** par zone

## CORS

Autorisé pour React : `http://localhost:5173`
