using AutoMapper;

using backend.model.AccountViewModels;

namespace backend.model
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<LoginViewModel, LoginViewModel>();
        }
    }
}
