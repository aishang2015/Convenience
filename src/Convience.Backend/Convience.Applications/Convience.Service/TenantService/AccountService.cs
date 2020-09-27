using Convience.Entity.Data;
using Convience.Entity.Entity.Tenants;
using Convience.EntityFrameWork.Repositories;
using Convience.EntityFrameWork.Saas;
using Convience.JwtAuthentication;
using Convience.Model.Models.Tenant;
using Convience.Util.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Convience.Service.TenantService
{
    public interface IAccountService
    {
        (bool, string) Login(TenantLoginViewModel loginViewModel);

        Task<bool> Regist(RegisterViewModel registerViewModel);
    }

    public class AccountService : IAccountService
    {
        private readonly IRepository<Tenant> _tenantRepository;

        private readonly IUnitOfWork<SystemIdentityDbContext> _systemUnitOfWork;

        private readonly SystemIdentityDbContext _systemIdentityDbContext;

        private readonly AppSaasDbContext _appSaasDbContext;

        private readonly IJwtFactory _jwtFactory;

        public AccountService(IRepository<Tenant> tenantRepository,
            IUnitOfWork<SystemIdentityDbContext> systemUnitOfWork,
            SystemIdentityDbContext systemIdentityDbContext,
            AppSaasDbContext appSaasDbContext,
            IOptionsSnapshot<JwtOption> jwtOptionAccessor)
        {
            _tenantRepository = tenantRepository;
            _systemUnitOfWork = systemUnitOfWork;
            _systemIdentityDbContext = systemIdentityDbContext;
            _appSaasDbContext = appSaasDbContext;
            var option = jwtOptionAccessor.Get(JwtAuthenticationSchemeConstants.MemberAuthenticationScheme);
            _jwtFactory = new JwtFactory(option);
        }

        public (bool, string) Login(TenantLoginViewModel loginViewModel)
        {
            var tenant = _tenantRepository.Get(tenant => tenant.Name == loginViewModel.Account)
                .FirstOrDefault();
            if (tenant == null)
            {
                return (false, "用户名或密码错误！");
            }
            if (!tenant.IsActive)
            {
                return (false, "该租户未激活！");
            }

            // PostgreSQL版本
            var querySql = @$"
                SELECT * FROM ""{tenant.Schema}"".""Member"" WHERE ""{tenant.Schema}"".""Member"".""Account""='{loginViewModel.Account}'";

            var findMemeber = _appSaasDbContext.Set<Member>().FromSqlRaw(querySql).FirstOrDefault();

            var hash = EncryptionHelper.MD5Encrypt(loginViewModel.Password + '@' + findMemeber.Salt);
            if (findMemeber.PasswordHash == hash)
            {
                var token = _jwtFactory.GenerateJwtToken(new System.Collections.Generic.List<(string, string)>
                {
                    (CustomClaimTypes.UserName,tenant.Name),
                    (CustomClaimTypes.UserSchema,tenant.Schema)
                });
                return (true, token);
            }
            return (false, "用户名或密码错误！");
        }

        public async Task<bool> Regist(RegisterViewModel registerViewModel)
        {
            if (!_tenantRepository.Get().Any(testc => testc.Name == registerViewModel.Account))
            {
                var salt = Guid.NewGuid().ToString().Substring(0, 5);
                var schema = GuidHelper.NewSquentialGuidNoDash();
                await _tenantRepository.AddAsync(new Tenant
                {
                    Name = registerViewModel.Account,
                    Schema = schema,
                    CreatedTime = DateTime.Now,
                    IsActive = true,
                });

                var hash = EncryptionHelper.MD5Encrypt(registerViewModel.Password + '@' + salt);

                // PostgreSQL版本
                var batchSql = $@"
                CREATE SCHEMA IF NOT EXISTS ""{schema}"";

                CREATE TABLE IF NOT EXISTS ""{schema}"".""Member""(
                    ""Id"" BIGINT PRIMARY KEY NOT NULL,
                    ""Account"" VARCHAR(20) NOT NULL,
                    ""Email"" VARCHAR(200) NOT NULL,
                    ""Phone"" VARCHAR(200) NULL,
                    ""Salt"" VARCHAR(200) NULL,
                    ""PasswordHash"" VARCHAR(2000) NULL
                );

                INSERT INTO ""{schema}"".""Member"" 
	                (""Id"", ""Account"", ""Email"", ""Phone"", ""Salt"", ""PasswordHash"") VALUES
                    (100001, '{registerViewModel.Account}', 'test@126.com', 15899999999, '{salt}', '{hash}')";

                _systemIdentityDbContext.Database.ExecuteSqlRaw(batchSql);
                await _systemUnitOfWork.SaveAsync();

                //await _memberRepository.AddAsync(new Member
                //{
                //    Account = registerViewModel.Account,
                //    Salt = salt,
                //    PasswordHash = EncryptionHelper.MD5Encrypt(registerViewModel.Password + '@' + salt)
                //});
                //await _saasUnitOfWork.SaveAsync();

                return true;
            }
            return false;
        }
    }
}
