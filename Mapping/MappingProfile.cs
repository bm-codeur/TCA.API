using AutoMapper;
using TCA.API.DTOs;
using TCA.API.Models;

namespace TCA.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Zone, ZoneDto>();
        CreateMap<CreateZoneDto, Zone>();
        CreateMap<UpdateZoneDto, Zone>();

        CreateMap<Groupe, GroupeDto>()
            .ForMember(d => d.ZoneNom, opt => opt.MapFrom(s => s.Zone != null ? s.Zone.Nom : null));
        CreateMap<CreateGroupeDto, Groupe>();
        CreateMap<UpdateGroupeDto, Groupe>();

        CreateMap<Camion, CamionDto>()
            .ForMember(d => d.GroupeNumero, opt => opt.MapFrom(s => s.Groupe != null ? s.Groupe.Numero : null));
        CreateMap<CreateCamionDto, Camion>();
        CreateMap<UpdateCamionDto, Camion>();

        CreateMap<Chauffeur, ChauffeurDto>()
            .ForMember(d => d.CamionNumero, opt => opt.MapFrom(s => s.Camion != null ? s.Camion.Numero : null));
        CreateMap<CreateChauffeurDto, Chauffeur>();
        CreateMap<UpdateChauffeurDto, Chauffeur>();

        CreateMap<Chargement, ChargementDto>()
            .ForMember(d => d.CamionNumero, opt => opt.MapFrom(s => s.Camion != null ? s.Camion.Numero : null))
            .ForMember(d => d.ZoneNom, opt => opt.MapFrom(s => s.Zone != null ? s.Zone.Nom : null));

        // Superviseur General
        CreateMap<SuperviseurGeneral, SuperviseurGeneralDto>();
        CreateMap<CreateSuperviseurGeneralDto, SuperviseurGeneral>();
        CreateMap<UpdateSuperviseurGeneralDto, SuperviseurGeneral>();

        // Superviseur Zone
        CreateMap<SuperviseurZone, SuperviseurZoneDto>()
            .ForMember(d => d.ZoneNom, opt => opt.MapFrom(s => s.Zone != null ? s.Zone.Nom : null));
        CreateMap<CreateSuperviseurZoneDto, SuperviseurZone>();
        CreateMap<UpdateSuperviseurZoneDto, SuperviseurZone>();

        // Superviseur Groupe
        CreateMap<SuperviseurGroupe, SuperviseurGroupeDto>()
            .ForMember(d => d.GroupeNumero, opt => opt.MapFrom(s => s.Groupe != null ? s.Groupe.Numero : null));
        CreateMap<CreateSuperviseurGroupeDto, SuperviseurGroupe>();
        CreateMap<UpdateSuperviseurGroupeDto, SuperviseurGroupe>();
    }
}
