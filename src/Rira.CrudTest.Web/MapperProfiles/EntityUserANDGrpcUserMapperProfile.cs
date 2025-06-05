using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;
using ProtoUser = Rira.CrudTest.Web.Grpc.Protos;

namespace Rira.CrudTest.Web.MapperProfiles;

// This profile maps between the EntityUser and GrpcUser types.
public class EntityUserANDGrpcUserMapperProfile : Profile
{
  public EntityUserANDGrpcUserMapperProfile()
  {
    Guid guid;

    CreateMap<EntityUser, ProtoUser.User>()
     .ForMember(dest => dest.Id,
     opt => opt.MapFrom(src =>
     src.Id.ToString()))
     .ForMember(dest => dest.DateOfBirth,
           opt => opt.MapFrom(src =>
               Timestamp.FromDateTime(src.DateOfBirth
                   .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc))));

    CreateMap<ProtoUser.User, EntityUser>()
      .ForMember(dest => dest.Id,
      opt => opt.MapFrom(src =>
      Guid.TryParse(src.Id, out guid) ? guid : Guid.Empty))
      .ForMember(dest => dest.DateOfBirth,
            opt => opt.MapFrom(src =>
                DateOnly.FromDateTime(src.DateOfBirth.ToDateTime())));
  }
}
