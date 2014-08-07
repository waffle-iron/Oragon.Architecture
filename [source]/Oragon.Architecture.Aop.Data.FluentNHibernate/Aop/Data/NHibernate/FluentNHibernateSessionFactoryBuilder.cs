using System;
using System.Configuration;
using FluentNH = FluentNHibernate;
using NH = NHibernate;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	/// <summary>
	///     Responsável por inicializar a configuraçãio do NHibernate e disponibilizar um SessionFactory pra a aplicação
	/// </summary>
	public abstract class FluentNHibernateSessionFactoryBuilder : AbstractSessionFactoryBuilder
	{
		#region Public Constructors

		public FluentNHibernateSessionFactoryBuilder()
		{
		}

		#endregion Public Constructors

		#region Protected Methods

		/// <summary>
		///     Principal método privado, realiza a criação do SessionFactory e este não deve ser criado novamente até que o domínio de aplicação seja finalizado.
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
					throw new ConfigurationErrorsException(string.Format("Cannot load the Type '{0}', defined in TypeNames property of FluentNHibernateSessionFactoryBuilder", typeName));
				configuration.Mappings(it =>
				{
					it.FluentMappings.AddFromAssembly(typeInfo.Assembly);
					it.HbmMappings.AddFromAssembly(typeInfo.Assembly);
				});
			}

			NH.ISessionFactory sessionFactory = configuration.BuildSessionFactory();
			return sessionFactory;
		}

		#endregion Protected Methods

		#region Private Methods

		private FluentNH.Cfg.Db.IPersistenceConfigurer ConfigureDB2(ConnectionStringSettings connectionStringSettings)
		{
			var configDB2Client = FluentNH.Cfg.Db.DB2Configuration.Standard
							   .ConnectionString(connectionStringSettings.ConnectionString)
							   .MaxFetchDepth(this.MaxFetchDepth)
							   .IsolationLevel(this.DefaultIsolationLevel);
			if (this.EnabledDiagnostics)
				configDB2Client = configDB2Client.ShowSql().FormatSql();
			FluentNH.Cfg.Db.IPersistenceConfigurer returnValue = configDB2Client;
			return returnValue;
		}

		private FluentNH.Cfg.Db.IPersistenceConfigurer ConfigureForMySQL(ConnectionStringSettings connectionStringSettings)
		{
			var configMySqlClient = FluentNH.Cfg.Db.MySQLConfiguration.Standard
							   .ConnectionString(connectionStringSettings.ConnectionString)
							   .MaxFetchDepth(this.MaxFetchDepth)
							   .IsolationLevel(this.DefaultIsolationLevel);
			if (this.EnabledDiagnostics)
				configMySqlClient = configMySqlClient.ShowSql().FormatSql();
			FluentNH.Cfg.Db.IPersistenceConfigurer returnValue = configMySqlClient;
			return returnValue;
		}

		private FluentNH.Cfg.Db.IPersistenceConfigurer ConfigureSQLServer(ConnectionStringSettings connectionStringSettings)
		{
			var configSqlClient = FluentNH.Cfg.Db.MsSqlConfiguration.MsSql2008
							   .ConnectionString(connectionStringSettings.ConnectionString)
							   .MaxFetchDepth(this.MaxFetchDepth)
							   .IsolationLevel(this.DefaultIsolationLevel);
			if (this.EnabledDiagnostics)
				configSqlClient = configSqlClient.ShowSql().FormatSql();
			FluentNH.Cfg.Db.IPersistenceConfigurer returnValue = configSqlClient;
			return returnValue;
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
					returnValue = this.ConfigureForMySQL(connectionStringSettings);
					break;

				case "System.Data.SqlClient":
					returnValue = this.ConfigureSQLServer(connectionStringSettings);
					break;

				case "System.Data.DB2Client":
					returnValue = this.ConfigureDB2(connectionStringSettings);
					break;

				default:
					throw new ConfigurationErrorsException("This ConnectionString dont have ProviderName defined. Use 'MySql.Data.MySqlClient', 'System.Data.DB2Client' or 'System.Data.SqlClient'.");
			}
			return returnValue;
		}

		private ConnectionStringSettings GetProviderName()
		{
			ConnectionStringSettings connStrSettings = null;

			if (this.ConnectionStringDiscoverer == null)
				throw new ConfigurationErrorsException(string.Format("ConnectionStringDiscoverer is not set"));

			connStrSettings = this.ConnectionStringDiscoverer.GetConnectionString();

			if (connStrSettings == null)
				throw new ConfigurationErrorsException("Cannot be found any ConnectionString");

			string providerName = connStrSettings.ProviderName;
			if (string.IsNullOrWhiteSpace(providerName))
				throw new ConfigurationErrorsException("This ConnectionString dont have ProviderName defined. Use 'MySql.Data.MySqlClient', 'System.Data.DB2Client' or 'System.Data.SqlClient'.");

			return connStrSettings;
		}

		#endregion Private Methods
	}
}