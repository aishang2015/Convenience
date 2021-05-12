using AutoMapper;

using Convience.Entity.Data;
using Convience.Entity.Entity;
using Convience.EntityFrameWork.Repositories;
using Convience.Model.Models.SystemManage;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.SystemManage
{
    public interface IMenuService
    {
        IQueryable<MenuResultModel> GetAllMenu();

        Task<bool> AddMenuAsync(MenuViewModel model);

        Task<bool> UpdateMenuAsync(MenuViewModel model);

        Task<bool> DeleteMenuAsync(int id);

        bool HavePermission(string[] menuIds, string permission);

        (string, string) GetIdentificationRoutes(string[] menuIds);
    }

    public class MenuService : IMenuService
    {
        private readonly ILogger<MenuService> _logger;

        private readonly IRepository<Menu> _menuRepository;

        private readonly IRepository<MenuTree> _menuTreeRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _unitOfWork;

        private IMapper _mapper;

        public MenuService(
            ILogger<MenuService> logger,
            IRepository<Menu> menuRepository,
            IRepository<MenuTree> menuTreeRepository,
            IUnitOfWork<SystemIdentityDbContext> unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _menuRepository = menuRepository;
            _menuTreeRepository = menuTreeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> AddMenuAsync(MenuViewModel model)
        {
            using var tran = await _unitOfWork.StartTransactionAsync();
            var menu = _mapper.Map<Menu>(model);
            var entity = await _menuRepository.AddAsync(menu);
            await _unitOfWork.SaveAsync();

            if (!string.IsNullOrEmpty(model.UpId))
            {
                var tree = _menuTreeRepository.Get(t => t.Descendant == int.Parse(model.UpId));

                // 做成树数据
                foreach (var m in tree)
                {
                    await _menuTreeRepository.AddAsync(new MenuTree
                    {
                        Ancestor = m.Ancestor,
                        Descendant = entity.Id,
                        Length = m.Length + 1
                    });
                }
            }
            await _menuTreeRepository.AddAsync(new MenuTree
            {
                Ancestor = entity.Id,
                Descendant = entity.Id,
                Length = 0
            });
            await _unitOfWork.SaveAsync();
            await _unitOfWork.CommitAsync(tran);
            return true;
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            var descendantIds = _menuTreeRepository.Get(menu => menu.Ancestor == id)
                .Select(menu => menu.Descendant);
            await _menuRepository.RemoveAsync(menu => descendantIds.Contains(menu.Id));
            await _menuTreeRepository.RemoveAsync(tree => descendantIds.Contains(tree.Ancestor) || descendantIds.Contains(tree.Descendant));
            await _unitOfWork.SaveAsync();
            return true;
        }

        public IQueryable<MenuResultModel> GetAllMenu()
        {
            var query = from menu in _menuRepository.Get()
                        join tree in _menuTreeRepository.Get()
                        on new { id = menu.Id, length = 1 } equals new { id = tree.Descendant, length = tree.Length }
                        into e
                        from rtree in e.DefaultIfEmpty()
                        orderby menu.Sort
                        select new MenuResultModel
                        {
                            Id = menu.Id,
                            UpId = rtree.Ancestor.ToString(),
                            Name = menu.Name,
                            Identification = menu.Identification,
                            Permission = menu.Permission,
                            Type = (int)menu.Type,
                            Route = menu.Route,
                            Sort = menu.Sort,
                        };
            return query;
        }

        public async Task<bool> UpdateMenuAsync(MenuViewModel model)
        {
            var menu = _mapper.Map<Menu>(model);
            _menuRepository.Update(menu);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public bool HavePermission(string[] menuIds, string permission)
        {
            var query = from menu in _menuRepository.Get()
                        where menuIds.Contains(menu.Id.ToString()) && menu.Permission.Contains(permission)
                        select menu;
            return query.Any();
        }

        public (string, string) GetIdentificationRoutes(string[] menuIds)
        {
            var query = from menu in _menuRepository.Get()
                        where menuIds.Contains(menu.Id.ToString())
                        select menu;
            var identifications = string.Join(',', query
                .Where(m => !string.IsNullOrEmpty(m.Identification)).Select(m => m.Identification));
            var routes = string.Join(',', query
                .Where(m => !string.IsNullOrEmpty(m.Route)).Select(m => m.Route));
            return (identifications, routes);
        }
    }
}
