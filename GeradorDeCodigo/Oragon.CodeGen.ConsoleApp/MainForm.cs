using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oragon.CodeGen.Templating;

namespace Oragon.CodeGen.ConsoleApp
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		public MainForm(GenerationContainer generationContainer)
			: this()
		{
			this.GenerationContainer = generationContainer;
			this.GenerationContainer.EndOfProcess += new EventHandler(GenerationContainer_EndOfProcess);
		}


		public GenerationContainer GenerationContainer { get; set; }

		private void MainForm_Load(object sender, EventArgs e)
		{
			foreach (T4Template template in this.GenerationContainer.Templates)
			{
				template.CleanUp += new Action<T4Template>(template_CleanUp);
				template.GenerationStart += new Action<T4Template>(template_GenerationStart);
				template.GenerationSucess += new Action<T4Template>(template_GenerationSucess);
				template.GenerationError += new Action<T4Template, Exception>(template_GenerationError);
			}
			this.t4TemplateBindingSource.DataSource = this.GenerationContainer.Templates;
		}

		void GenerationContainer_EndOfProcess(object sender, EventArgs e)
		{
			System.Windows.Forms.MessageBox.Show(this, "Finalizado", "Fim de Processamento", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
		}

		#region Eventos dos Templates

		void template_CleanUp(T4Template sender)
		{
			DataGridViewImageCell iconCell = this.GetIconCell(sender);
			iconCell.Value = Properties.Resources.paused;
			this.templateStatusDataGridView.InvalidateCell(iconCell);
		}

		void template_GenerationError(T4Template sender, Exception exception)
		{
			DataGridViewImageCell iconCell = this.GetIconCell(sender);
			iconCell.Value = null;
			iconCell.Value = Properties.Resources.error;
			iconCell.ImageLayout = DataGridViewImageCellLayout.Normal;
			this.templateStatusDataGridView.InvalidateCell(iconCell);
		}

		void template_GenerationSucess(T4Template sender)
		{
			DataGridViewImageCell iconCell = this.GetIconCell(sender);
			iconCell.Value = Properties.Resources.ok;
			this.templateStatusDataGridView.InvalidateCell(iconCell);
		}

		void template_GenerationStart(T4Template sender)
		{
			DataGridViewImageCell iconCell = this.GetIconCell(sender);
			iconCell.Value = Properties.Resources.runing;
			this.templateStatusDataGridView.InvalidateCell(iconCell);
		}

		#endregion

		private DataGridViewImageCell GetIconCell(T4Template sender)
		{
			DataGridViewImageCell returnValue = null;
			foreach (DataGridViewRow currentRow in templateStatusDataGridView.Rows)
			{
				if (currentRow.DataBoundItem == sender)
				{
					returnValue = (DataGridViewImageCell)currentRow.Cells["IconColumn"];
					break;
				}
			}
			return returnValue;
		}

		private void btnExecutar_Click(object sender, EventArgs e)
		{
			this.GenerationContainer.CleanUp();
			this.GenerationContainer.Run();
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow currentRow = templateStatusDataGridView.Rows[e.RowIndex];
			DataGridViewImageCell currentCell = currentRow.Cells[e.ColumnIndex] as DataGridViewImageCell;
			if (currentCell != null)
			{
				T4Template template = (T4Template)currentRow.DataBoundItem;

				(new DetailForm(template.Log.ToString())).ShowDialog(this);
			}
		}


	}
}
