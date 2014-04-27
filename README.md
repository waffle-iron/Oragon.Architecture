![Oragon Architecture][1]
===================

Oragon Architecture is a toolset of frameworks and architectural design to provide simple way to development robust and primorous applications and services.

Inside Oragon Architecture you will find:

- Roubust Application Server, pure in .Net is a best choice to deploy services based on MicroServices Pattern
- Isolated service hosting, container independent.
- Deployment abstractions to help you when you need deploy any Service Facade as WPF, RabbitMQ listener, Remoting, Enterprise Services, Web Services 1.1 and more.
- Robust database first code generator, generating FluentNhibernate Mappings(1), NHibernate (minimal) configuration, Entities and Data Process. All you need to perform CRUD operations in most commom SGDB`s
- Infrastructure to manage Data Access using AOP (MongoDB, NHibernate With MySQL, SQL Server or/and DB2, Redis)
- Infrastructure to help you to implement abstract AOP compositions using Spring.Net
- Infrastructure to use Exception Management and Handling using AOP.
- Infrastructure to Log Exceptions contextually with tags and values (improve business tracking) using Sentry.
- Extensions, a lot of extensions to help you and your develoment team.


[See more in Oragon Architecture Wiki][2]

(1) NHibernate Mappings will work fine with SQL Server, MySQL and DB2, because theirs have a same way to define identity columns. To support other SGDBs, you will need to write a new code generation template. The database metadata abstractions supports many others providers like MS Access, Advantage, DB2, Firebird, MySql, PostgreSQL, SQLite and VistaDB.


  [1]: http://luizcarlosfaria.net/wp-content/uploads/2014/03/OragonArchitecture.export.png
  [2]: https://github.com/luizcarlosfaria/Oragon.Architecture/wiki/Oragon-Architecture
