using AutoMapper;

using backend.model.backend.api.AccountViewModels;

namespace backend.model.backend.api
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<LoginViewModel, LoginViewModel>();
        }
    }
}
