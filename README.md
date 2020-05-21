- 当前版本：AspNetCore3.1和Angular9

- 功能：用户管理，系统管理，角色管理，菜单设置，部门管理，职位管理，组织管理，文章管理，文件管理（使用mongoDB），代码生成

- 特点：

  - 对大部分工具类进行封装，传入简单参数即可调用。
  - 对实体添加Entity注解即实现了所有实体，仓储，服务的自动注入。
  - 代码生成器直接生成所有前端component，service，后端控制器，服务等全部代码。
  - 任务计划可选用netcore自有的Background Task或者Hangfire
  - 缓存使用EasyCaching，统一不同缓存系统接口
  - 使用NG-ZORRO实现前端大部分功能
  - 添加了wpf的客户端。

- 实验环境（docker）：config文件夹为容器映射本地配置文件，基本不需要修改。本机：docker-compose up -d。远程主机：修改config/config.json的地址为主机地址，然后用docker-compose构建。

- 示例：http://172.81.254.197/

  用户名admin1~admin9，密码相同。任务计划定时重置菜单和用户数据。使用docker部署在1核2g1m服务器上