## Convenience

使用.NET5 + Angular 11 + NG-ZORRO开发，实现了很多基本功能，方便二次开发。

### 功能

实现了系统管理（用户，角色，菜单），组织管理（部门，职位），审批工作流，内容管理（文章，文件，字典等），代码生成，日志工具等。

### 演示

地址：http://180.163.89.224:8888/

账号：admin1~admin9

密码：同账号

### 开发环境

vs + vs code

### 本地运行

Convience.Web\Managent是web端

Convience.Backend是api端

### 本地运行（docker）

cd到src目录执行docker-compose up -d --build

然后访问localhost:8888

### 创建项目模板（后端）

cd到src\Convience.Backend目录，执行[dotnet new -i .]，这样就创建了一个convience名称的模板（名称可以在template.json中修改）。然后通过[dotnet new convience -n 项目名]可以创建新项目，新项目的命名空间会被修改为刚才指定的项目名。

### 重要变更

2021/03/13  api从net core3.1 升级到 .net5