using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.Domain;
using Oragon.CodeGen.MetaData.DataBase;
using Oragon.CodeGen.MetaData.Plugin;

namespace Oragon.CodeGen.MetaData
{
	public enum MetadataAnalyserStrategy
	{
		RichModel,
		TableModel
	}

	internal class MetadataAnalyser
	{
		public MetadataAnalyser() { }

		public List<string> TableIgnorePatterns { get; set; }
		public List<string> ColumnIgnorePatterns { get; set; }

		public bool? CanInsert { get; set; }
		public bool? CanUpdate { get; set; }
		public bool? CanDelete { get; set; }


		public MetadataAnalyserStrategy Strategy { get; set; }

		public Func<IColumn, bool> ColumnFilterExpression;


		public DomainModel AnalyseDataBase(Oragon.CodeGen.MetaData.DataBase.IDatabase dbToAnalyse)
		{
			this.ColumnFilterExpression = (currentColumn =>
					(this.ColumnIgnorePatterns == null)
					||
					(this.ColumnIgnorePatterns.Count == 0)
					||
					(!this.ColumnIgnorePatterns.Any(currentPattern => Spring.Util.PatternMatchUtils.SimpleMatch(currentPattern, currentColumn.Name)))
			);

			DomainModel model = new DomainModel()
			{
				CanInsert = this.CanInsert == null ? true : this.CanInsert.Value,
				CanUpdate = this.CanUpdate == null ? true : this.CanUpdate.Value,
				CanDelete = this.CanDelete == null ? true : this.CanDelete.Value
			};
			model.NameStretegy = (INameStretegy)Spring.Context.Support.ContextRegistry.GetContext().GetObject("NameStretegy");

			foreach (ITable currentTable in dbToAnalyse.Tables)
			{
				bool tableIgnoreMatchResult = this.TableIgnorePatterns.Any(it => Spring.Util.PatternMatchUtils.SimpleMatch(it, currentTable.Name));

				if (
					(!tableIgnoreMatchResult)
					&&
					(
						(this.IsManyToManySimpleTable(currentTable) == false)
						||
						(this.Strategy == MetadataAnalyserStrategy.TableModel)
					)
					&&
					(currentTable.PrimaryKeys.Count > 0))
				{
					Entity newEntity = new Entity(model, currentTable);
					List<IColumn> columns = newEntity.Table.Columns
						.ToList()
						.Where(this.ColumnFilterExpression)
						.ToList();

					if (this.Strategy == MetadataAnalyserStrategy.RichModel)
						this.BuildAssociationProperties(newEntity, columns);
					this.BuildValueTypeProperties(newEntity, columns);
					model.Entities.Add(newEntity);
					if (columns.Count > 0)
						throw new Exception("Colunas não puderam ser mapeadas.");
				}
			}
			return model;
		}

		private bool IsManyToManySimpleTable(ITable table)
		{
			//Todas as colunas estão na PK
			bool challenge1 = table.Columns.Where(this.ColumnFilterExpression).All(it => it.IsInPrimaryKey);
			//Todos os campos estão em FK`s
			bool challenge2 = table.Columns.Where(this.ColumnFilterExpression).All(it => it.IsInForeignKey);
			//Todas as FK`s são de outras tabelas para esta
			bool challenge3 = table.ForeignKeys.All(it => it.ForeignTable == table);
			//Só possui 2 FK`s
			bool challenge4 = table.ForeignKeys.Count == 2;

			return challenge1 && challenge2 && challenge3 && challenge4;
		}

		private void BuildValueTypeProperties(Entity entity, List<IColumn> avaibleColumns)
		{
			List<IColumn> columns = avaibleColumns.ToList();
			foreach (IColumn currentColumn in columns)
			{
				ValueTypeProperty newProperty = new ValueTypeProperty(entity, currentColumn);
				entity.Properties.Add(newProperty);
				avaibleColumns.Remove(currentColumn);
			}
		}

		private void BuildAssociationProperties(Entity entity, List<IColumn> avaibleColumns)
		{
			List<IForeignKey> foreignKeys = entity.Table.ForeignKeys.ToList();
			//Precisa ser primeiro
			this.BuildManyToManyProperties(entity, avaibleColumns, foreignKeys);
			this.BuildOneToManyProperties(entity, avaibleColumns, foreignKeys);
			this.BuildManyToOneProperties(entity, avaibleColumns, foreignKeys);
		}

		private void BuildManyToManyProperties(Entity entity, List<IColumn> avaibleColumns, List<IForeignKey> avaibleforeignKeys)
		{
			List<IForeignKey> currentForeignKeys = avaibleforeignKeys.Where(it => it.PrimaryTable == entity.Table && this.IsManyToManySimpleTable(it.ForeignTable)).ToList();
			foreach (IForeignKey currentForeignKey in currentForeignKeys)
			{
				bool ignoreMatch = this.TableIgnorePatterns.Any(it => Spring.Util.PatternMatchUtils.SimpleMatch(it, currentForeignKey.ForeignTable.Name));
				if (ignoreMatch == false)
				{
					IForeignKey leftForeignKey = currentForeignKey;
					IForeignKey rightForeignKey = currentForeignKey.ForeignTable.ForeignKeys.Where(it => it.Name != leftForeignKey.Name).First();

					ManyToManyProperty newProperty = new ManyToManyProperty(entity, leftForeignKey, rightForeignKey);
					entity.Properties.Add(newProperty);
					avaibleforeignKeys.Remove(currentForeignKey);
				}
			}
		}

		private void BuildManyToOneProperties(Entity entity, List<IColumn> avaibleColumns, List<IForeignKey> avaibleforeignKeys)
		{
			List<IForeignKey> currentForeignKeys = avaibleforeignKeys.Where(it => it.ForeignTable == entity.Table).ToList();
			foreach (IForeignKey currentForeignKey in currentForeignKeys)
			{
				bool ignoreMatch = this.TableIgnorePatterns.Any(it => Spring.Util.PatternMatchUtils.SimpleMatch(it, currentForeignKey.PrimaryTable.Name));
				bool failFK = (
					currentForeignKey.PrimaryTable.Name == currentForeignKey.ForeignTable.Name
					&&
					currentForeignKey.PrimaryColumns.Count == 1
					&&
					currentForeignKey.ForeignColumns.Count == 1
					&&
					currentForeignKey.ForeignColumns.First().Name == currentForeignKey.PrimaryColumns.First().Name
					&&
					currentForeignKey.PrimaryColumns.First().IsAutoKey
				);
				if ((ignoreMatch == false) && (failFK == false))
				{
					ManyToOneProperty newProperty = new ManyToOneProperty(entity, currentForeignKey);
					entity.Properties.Add(newProperty);
					avaibleforeignKeys.Remove(currentForeignKey);
					foreach (IColumn column in currentForeignKey.ForeignColumns)
						avaibleColumns.Remove(column);
				}
			}
		}

		private void BuildOneToManyProperties(Entity entity, List<IColumn> avaibleColumns, List<IForeignKey> avaibleforeignKeys)
		{
			List<IForeignKey> currentForeignKeys = avaibleforeignKeys.Where(it => it.PrimaryTable == entity.Table).ToList();
			foreach (IForeignKey currentForeignKey in currentForeignKeys)
			{
				bool ignoreMatch = this.TableIgnorePatterns.Any(it => Spring.Util.PatternMatchUtils.SimpleMatch(it, currentForeignKey.ForeignTable.Name));
				if (ignoreMatch == false)
				{
					OneToManyProperty newProperty = new OneToManyProperty(entity, currentForeignKey);
					entity.Properties.Add(newProperty);
					if (currentForeignKey.ForeignTable != currentForeignKey.PrimaryTable)
						avaibleforeignKeys.Remove(currentForeignKey);
				}

			}
		}





	}
}
