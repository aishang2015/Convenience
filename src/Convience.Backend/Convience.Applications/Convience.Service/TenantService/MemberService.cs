using Convience.Entity.Entity.Tenants;
using Convience.EntityFrameWork.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Convience.Service.TenantService
{
    public interface IMemberService
    {
        IList<Member> GetAllMemeber();
    }

    public class MemberService : IMemberService
    {
        private IRepository<Member> _memeberRepository;

        public MemberService(IRepository<Member> memeberRepository)
        {
            _memeberRepository = memeberRepository;
        }

        public IList<Member> GetAllMemeber()
        {
            return _memeberRepository.Get().ToList();
        }
    }
}
