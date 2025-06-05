using System;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Rira.CrudTest.Web.Grpc.Protos;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;

namespace Rira.CrudTest.Web.MapperProfiles;

// This profile maps between the EntityUser and UpdateUserRequest types.  
public class EntityUserAndGrpcUpdateUserRequestMapperProfile : Profile
{
  public EntityUserAndGrpcUpdateUserRequestMapperProfile()
  {
    Guid guid;

    CreateMap<EntityUser, UpdateUserRequest>()
          .ForMember(dest => dest.Id,
          opt => opt.MapFrom(src =>
          src.Id.ToString()))
          .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src =>
                    Timestamp.FromDateTime(src.DateOfBirth
                        .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc))));

    CreateMap<UpdateUserRequest, EntityUser>()
          .ForMember(dest => dest.Id,
          opt => opt.MapFrom(src =>
                   Guid.TryParse(src.Id, out guid) ? guid : Guid.Empty))
          .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src =>
                    DateOnly.FromDateTime(src.DateOfBirth.ToDateTime())));
  }
}
