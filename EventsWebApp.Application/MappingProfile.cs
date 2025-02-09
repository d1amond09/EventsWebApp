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

		CreateMap<Participant, ParticipantDto>();
		CreateMap<ParticipantForCreationDto, Participant>();
		CreateMap<ParticipantForUpdateDto, Participant>();

		CreateMap<User, UserDto>();
		CreateMap<UserForCreationDto, User>();
		CreateMap<UserForUpdateDto, User>();
	}
}

