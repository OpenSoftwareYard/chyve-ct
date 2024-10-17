using System;
using AutoMapper;
using Persistence.Entities;

namespace Persistence.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Node, NodeDTO>().ReverseMap();
        CreateMap<Zone, ZoneDTO>()
            .ReverseMap()
            .ForMember(zone => zone.Brand, opt => opt.MapFrom((src, dest, destMember, context) => "pkgsrc"));
    }
}
