- 当前版本：AspNetCore3.1和Angular9

- 功能：用户管理，系统管理，角色管理，菜单设置，部门管理，职位管理，组织管理，文章管理，文件管理（使用mongoDB），代码生成

- 特点：对实体添加Entity注解即实现了所有实体，仓储，服务的自动注入。代码生成器直接生成所有angular组件，后端控制器，服务等全部代码。

- 实验环境（docker）：本机：docker-compose up -d。远程主机：修改config/config.json的地址为主机地址，然后用docker-compose构建

  ​									远程主机：修改config/config.json的地址，然后用docker-compose构建

- 示例：http://172.81.254.197/

  用户名admin1~admin9，密码相同。任务计划定时重置菜单和用户数据。使用docker部署在1核2g1m服务器上