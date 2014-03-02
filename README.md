![Oragon Architecture][1]
===================

Oragon Architecture is a toolset of frameworks and architectural design to provide simple way to development robust and primorous applications and services.

Inside Oragon Architecture you will find:

 - Robust data code generator, responsible to generate FluentNhibernate Mappings(1), NHibernate (minimal) configuration, Entities and Data Process.
- Infrastructure to manage Data Access (MongoDB, NHibernate With MySQL, SQL Server or/and DB2, Redis)
- Infrastructure to help you to implement abstractions for AOP, using Spring.Net
- Infrastructure to use Exception Management and Handling using AOP.
- Infrastructure to Log Exceptions contextually with tags and values (improve business tracking)
- And much more...

  [1]: http://luizcarlosfaria.net/wp-content/uploads/2014/03/OragonArchitecture.export.png

(1) NHibernate Mappings will work fine in SQL Server, MySQL and DB2, because theirs have a same way to define identity columns. To support other SGDBs, you will need to write a new code generation template. The database metadata abstractions supports many others providers like MS Access, Advantage, DB2, Firebird, MySql, PostgreSQL, SQLite and VistaDB.
