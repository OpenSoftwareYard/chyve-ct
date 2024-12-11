using System;
using AutoMapper;
using Persistence.Entities;

namespace Persistence.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Node, NodeDTO>().ReverseMap();
        CreateMap<Zone, ZoneDTO>().ReverseMap();
        CreateMap<PersonalAccessToken, PatDetailsDTO>().ReverseMap();
        CreateMap<PersonalAccessTokenScope, PatScopeDTO>().ReverseMap();
        CreateMap<Organization, OrganizationDTO>().ReverseMap();
    }
}
