using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Rira.CrudTest.Web.Grpc.Protos;
using EntityUser = Rira.CrudTest.Core.UserAggregate.Entities.User;

namespace Rira.CrudTest.Web.MapperProfiles;

// This profile maps between the EntityUser and CreateUserRequest types.
public class EntityUserAndGrpcCreateUserRequestMapperProfile : Profile
{
  public EntityUserAndGrpcCreateUserRequestMapperProfile()
  {
    CreateMap<EntityUser, CreateUserRequest>()
        .ForMember(dest => dest.DateOfBirth,
            opt => opt.MapFrom(src =>
                Timestamp.FromDateTime(src.DateOfBirth
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc))));

    CreateMap<CreateUserRequest, EntityUser>()
        .ForMember(dest => dest.DateOfBirth,
            opt => opt.MapFrom(src =>
                DateOnly.FromDateTime(src.DateOfBirth.ToDateTime())));
  }
}
