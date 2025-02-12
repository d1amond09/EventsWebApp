using AutoMapper;
using EventsWebApp.Application.DTOs;
using EventsWebApp.Domain.Entities;

namespace EventsWebApp.Application;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Event, EventDto>();
		CreateMap<EventForCreationDto, Event>();
		CreateMap<EventForUpdateDto, Event>();

		CreateMap<Participant, ParticipantDto>()
			.ForMember(p => p.BirthDate, opt => opt.MapFrom(x => x.User.BirthDate))
			.ForMember(p => p.FirstName, opt => opt.MapFrom(x => x.User.FirstName))
			.ForMember(p => p.LastName, opt => opt.MapFrom(x => x.User.LastName))
			.ForMember(p => p.Email, opt => opt.MapFrom(x => x.User.Email));

		CreateMap<User, UserDto>();
		CreateMap<UserForUpdateDto, User>();
		CreateMap<UserForRegistrationDto, User>();
	}
}

