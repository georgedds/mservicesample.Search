using AutoMapper;
using mservicesample.Search.Api.DataAccess.Entities;
using mservicesample.Search.Api.Dtos.Responses;

namespace mservicesample.Search.Api.Dtos.Mapping
{
     public class DataMapping : Profile
    {
        public DataMapping()
        {

            CreateMap<Artist, ArtistDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RealName, opt => opt.MapFrom(src => src.RealName))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.namevariations, opt => opt.MapFrom(src => src.namevariations))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<SearchResult<Artist>, SearchResult<ArtistDto>>().ForMember(dest => dest.ElapsedMilliseconds, opt => opt.MapFrom(src => src.ElapsedMilliseconds))
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.PageSize))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.Results, opt => opt.MapFrom(src => src.Results))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Nest.GetResponse<Release>, Release>()
                .ForMember(dest => dest.artists, opt => opt.MapFrom(src => src.Source.artists))
                .ForMember(dest => dest.country, opt => opt.MapFrom(src => src.Source.country))
                .ForMember(dest => dest.data_quality, opt => opt.MapFrom(src => src.Source.data_quality))
                .ForMember(dest => dest.genres, opt => opt.MapFrom(src => src.Source.genres))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Source.id))
                .ForMember(dest => dest.images, opt => opt.MapFrom(src => src.Source.images))
                .ForMember(dest => dest.labels, opt => opt.MapFrom(src => src.Source.labels))
                .ForMember(dest => dest.notes, opt => opt.MapFrom(src => src.Source.notes))
                .ForMember(dest => dest.released, opt => opt.MapFrom(src => src.Source.released))
                .ForMember(dest => dest.styles, opt => opt.MapFrom(src => src.Source.styles))
                .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Source.title))
                .ForMember(dest => dest.tracklist, opt => opt.MapFrom(src => src.Source.tracklist))
                .ForMember(dest => dest.videos, opt => opt.MapFrom(src => src.Source.videos))
                
                .ForAllOtherMembers(opt => opt.Ignore());
        }

    }
}
