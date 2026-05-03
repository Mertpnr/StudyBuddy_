using AutoMapper;
using StudyBuddy.API.DTOs.AnswerDto;
using StudyBuddy.API.DTOs.CategoryDto;
using StudyBuddy.API.DTOs.MatchDto;
using StudyBuddy.API.DTOs.MatchRequestDto;
using StudyBuddy.API.DTOs.OptionDto;
using StudyBuddy.API.DTOs.QuestionDto;
using StudyBuddy.API.DTOs.UserDto;
using StudyBuddy.API.Model;
using StudyBuddy.API.Requests.AnswerRequest;
using StudyBuddy.API.Requests.CategoryRequest;
using StudyBuddy.API.Requests.MatchRequest;
using StudyBuddy.API.Requests.MatchRequestRequest;
using StudyBuddy.API.Requests.OptionRequest;
using StudyBuddy.API.Requests.QuestionRequest;
using StudyBuddy.API.Requests.UserRequest;

namespace StudyBuddy.API.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category
            CreateMap<Category, CategoryBaseDto>().ReverseMap();
            CreateMap<Category, CategoryListDto>().ReverseMap();
            CreateMap<Category, CategoryCreateRequest>().ReverseMap();
            CreateMap<Category, CategoryUpdateRequest>().ReverseMap();

            // Answer
            CreateMap<Answer, AnswerBaseDto>().ReverseMap();
            CreateMap<Answer, AnswerListDto>().ReverseMap();
            CreateMap<Answer, AnswerCreateRequest>().ReverseMap();
            CreateMap<Answer, AnswerUpdateRequest>().ReverseMap();

            // Option
            CreateMap<Option, OptionBaseDto>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text ?? string.Empty))
                .ReverseMap();

            CreateMap<Option, OptionListDto>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text ?? string.Empty))
                .ReverseMap();

            CreateMap<Option, OptionCreateRequest>().ReverseMap();
            CreateMap<Option, OptionUpdateRequest>().ReverseMap();

            // Question
            CreateMap<Question, QuestionBaseDto>().ReverseMap();
            CreateMap<Question, QuestionListDto>().ReverseMap();
            CreateMap<Question, QuestionCreateRequest>().ReverseMap();
            CreateMap<Question, QuestionUpdateRequest>().ReverseMap();

            // Match
            CreateMap<Match, MatchBaseDto>().ReverseMap();
            CreateMap<Match, MatchListDto>().ReverseMap();
            CreateMap<Match, MatchCreateRequest>().ReverseMap();
            CreateMap<Match, MatchUpdateRequest>().ReverseMap();

            // MatchRequest
            CreateMap<MatchRequest, MatchRequestBaseDto>().ReverseMap();
            CreateMap<MatchRequest, MatchRequestListDto>().ReverseMap();
            CreateMap<MatchRequest, MatchRequestCreateRequest>().ReverseMap();
            CreateMap<MatchRequest, MatchRequestUpdateRequest>().ReverseMap();

            // User
            CreateMap<User, UserBaseDto>().ReverseMap();
            CreateMap<User, UserListDto>().ReverseMap();

            CreateMap<UserCreateRequest, User>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.UserGuid, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<UserUpdateRequest, User>()
                .ForMember(dest => dest.UserGuid, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());
        }
    }
}