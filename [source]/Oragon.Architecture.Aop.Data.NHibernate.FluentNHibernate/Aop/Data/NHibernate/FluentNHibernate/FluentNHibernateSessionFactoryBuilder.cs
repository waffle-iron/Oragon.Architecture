﻿using System;
using System.Collections.Generic;
using System.Configuration;
using NH = NHibernate;
using FluentNH = FluentNHibernate;
using Oragon.Architecture.Data.ConnectionStrings;

namespace Oragon.Architecture.Aop.Data.NHibernate.FluentNHibernate
{
	/// <summary>
	/// Responsável por inicializar a configuraçãio do NHibernate e disponibilizar um SessionFactory pra a aplicação
	/// </summary>
	public abstract class FluentNHibernateSessionFactoryBuilder: AbstractSessionFactoryBuilder
	{
		public FluentNHibernateSessionFactoryBuilder()
		{
		}

		
		/// <summary>
		/// Principal método privado, realiza a criação do SessionFactory e este não deve ser criado novamente até que o domínio de aplicação seja finalizado.
		/// </summary>
		/// <returns></returns>
		protected override NH.ISessionFactory BuildSessionFactoryInternal()
		{
			FluentNH.Cfg.Db.IPersistenceConfigurer databaseConfiguration = this.GetDataBaseConfiguration();

			FluentNH.Cfg.FluentConfiguration configuration = FluentNH.Cfg.Fluently
				.Configure()
				.Database(databaseConfiguration)
				.Cache(it =>
					it.UseQueryCache()
					.ProviderClass<NH.Cache.HashtableCacheProvider>()
				)
				.Diagnostics(it =>
					it.Enable(this.EnabledDiagnostics)
					.OutputToConsole()
				)
				.ExposeConfiguration(it =>
					it
					.SetProperty("command_timeout", this.CommandTimeout.ToString())
					.SetProperty("adonet.batch_size", this.BatchSize.ToString())
				);

			foreach (string typeName in this.TypeNames)
			{
				Type typeInfo = Type.GetType(typeName);
				if (typeInfo == null)
					throw new ConfigurationErrorsException(string.Format("Não foi possível carregar o tipo '{0}', informado na propriedade TypeNames do SessionFactoryBuilder.", typeName));
				configuration.Mappings(it =>
				{
					it.FluentMappings.AddFromAssembly(typeInfo.Assembly);
					it.HbmMappings.AddFromAssembly(typeInfo.Assembly);
				});
			}

			NH.ISessionFactory sessionFactory = configuration.BuildSessionFactory();
			return sessionFactory;
		}

		private FluentNH.Cfg.Db.IPersistenceConfigurer GetDataBaseConfiguration()
		{
			string mySQLProviderName = "MySql.Data.MySqlClient".ToLower();
			string sqlServerProviderName = "System.Data.SqlClient".ToLower();
			string db2ProviderName = "System.Data.DB2Client".ToLower();

			FluentNH.Cfg.Db.IPersistenceConfigurer returnValue = null;

			ConnectionStringSettings connectionStringSettings = this.GetProviderName();

			string connectionStringSettingsProviderName = connectionStringSettings.ProviderName;
			switch (connectionStringSettingsProviderName)
			{

				case "MySql.Data.MySqlClient":
					var configMySqlClient = FluentNH.Cfg.Db.MySQLConfiguration.Standard
						.ConnectionString(connectionStringSettings.ConnectionString)
						.MaxFetchDepth(this.MaxFetchDepth)
						.IsolationLevel(this.DefaultIsolationLevel);
					if(this.EnabledDiagnostics)
						configMySqlClient = configMySqlClient.ShowSql().FormatSql();
					returnValue = configMySqlClient;
					break;

				case "System.Data.SqlClient":
					var configSqlClient = FluentNH.Cfg.Db.MsSqlConfiguration.MsSql2008
						.ConnectionString(connectionStringSettings.ConnectionString)
						.MaxFetchDepth(this.MaxFetchDepth)
						.IsolationLevel(this.DefaultIsolationLevel);
					if(this.EnabledDiagnostics)
						configSqlClient = configSqlClient.ShowSql().FormatSql();
					returnValue = configSqlClient;
					break;

				case "System.Data.DB2Client":
					var configDB2Client = FluentNH.Cfg.Db.DB2Configuration.Standard
						.ConnectionString(connectionStringSettings.ConnectionString)						
						.MaxFetchDepth(this.MaxFetchDepth)
						.IsolationLevel(this.DefaultIsolationLevel);
					if(this.EnabledDiagnostics)
						configDB2Client = configDB2Client.ShowSql().FormatSql();
					returnValue = configDB2Client;
					break;

				default:
					throw new ConfigurationErrorsException("A ConnectionString não possui ProviderName configurado. Os valores possívels são: 'MySql.Data.MySqlClient', 'System.Data.DB2Client' e 'System.Data.SqlClient'.");
			}
			return returnValue;
		}

		private ConnectionStringSettings GetProviderName()
		{
			ConnectionStringSettings connStrSettings = null;

			if (this.ConStrConfigDiscovery == null)
				throw new ConfigurationErrorsException(string.Format("Não foi possível identificar a ConnectionString"));

			connStrSettings = this.ConStrConfigDiscovery.GetConnectionString();

			if (connStrSettings == null)
				throw new ConfigurationErrorsException("Não foi possível identificar a ConnectionString");

			string providerName = connStrSettings.ProviderName;
			if (string.IsNullOrWhiteSpace(providerName))
				throw new ConfigurationErrorsException("A ConnectionString não possui ProviderName configurado. Os valores possívels são: 'MySql.Data.MySqlClient', 'System.Data.DB2Client' e 'System.Data.SqlClient'.");

			return connStrSettings;
		}

	}
}
