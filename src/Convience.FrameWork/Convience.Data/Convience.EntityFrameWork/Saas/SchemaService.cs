using Microsoft.AspNetCore.Http;

namespace Convience.EntityFrameWork.Saas
{
    public interface ISchemaService
    {
        string Schema { get; }
    }

    public class SchemaService : ISchemaService
    {
        public SchemaService(IHttpContextAccessor httpContextAccessor)
        {
            var path = httpContextAccessor.HttpContext.Request.Path;
            // todo 解析path，根据path和待定的字典服务取得对应的schema
            Schema = string.Empty;
        }

        public string Schema { get; }

    }
}
