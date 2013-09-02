namespace Oragon.CodeGen.ConsoleApp
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.templateStatusDataGridView = new System.Windows.Forms.DataGridView();
			this.btnExecutar = new System.Windows.Forms.Button();
			this.IconColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.templatePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.outputPathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.t4TemplateBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.templateStatusDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.t4TemplateBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// templateStatusDataGridView
			// 
			this.templateStatusDataGridView.AllowUserToAddRows = false;
			this.templateStatusDataGridView.AllowUserToDeleteRows = false;
			this.templateStatusDataGridView.AllowUserToResizeRows = false;
			this.templateStatusDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.templateStatusDataGridView.AutoGenerateColumns = false;
			this.templateStatusDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.templateStatusDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.templateStatusDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.templatePathDataGridViewTextBoxColumn,
            this.outputPathDataGridViewTextBoxColumn,
            this.IconColumn});
			this.templateStatusDataGridView.DataSource = this.t4TemplateBindingSource;
			this.templateStatusDataGridView.Location = new System.Drawing.Point(12, 12);
			this.templateStatusDataGridView.Name = "templateStatusDataGridView";
			this.templateStatusDataGridView.RowHeadersVisible = false;
			this.templateStatusDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.templateStatusDataGridView.Size = new System.Drawing.Size(668, 170);
			this.templateStatusDataGridView.TabIndex = 0;
			this.templateStatusDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
			// 
			// btnExecutar
			// 
			this.btnExecutar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExecutar.Location = new System.Drawing.Point(605, 188);
			this.btnExecutar.Name = "btnExecutar";
			this.btnExecutar.Size = new System.Drawing.Size(75, 23);
			this.btnExecutar.TabIndex = 1;
			this.btnExecutar.Text = "Executar";
			this.btnExecutar.UseVisualStyleBackColor = true;
			this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
			// 
			// IconColumn
			// 
			this.IconColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.IconColumn.FillWeight = 30.34337F;
			this.IconColumn.HeaderText = "Status";
			this.IconColumn.Image = global::Oragon.CodeGen.ConsoleApp.Properties.Resources.paused;
			this.IconColumn.Name = "IconColumn";
			this.IconColumn.ReadOnly = true;
			this.IconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.IconColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.IconColumn.Width = 45;
			// 
			// templatePathDataGridViewTextBoxColumn
			// 
			this.templatePathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.templatePathDataGridViewTextBoxColumn.DataPropertyName = "TemplatePath";
			this.templatePathDataGridViewTextBoxColumn.FillWeight = 44.45038F;
			this.templatePathDataGridViewTextBoxColumn.HeaderText = "TemplatePath";
			this.templatePathDataGridViewTextBoxColumn.Name = "templatePathDataGridViewTextBoxColumn";
			this.templatePathDataGridViewTextBoxColumn.Width = 98;
			// 
			// outputPathDataGridViewTextBoxColumn
			// 
			this.outputPathDataGridViewTextBoxColumn.DataPropertyName = "OutputPath";
			this.outputPathDataGridViewTextBoxColumn.FillWeight = 124.4611F;
			this.outputPathDataGridViewTextBoxColumn.HeaderText = "OutputPath";
			this.outputPathDataGridViewTextBoxColumn.Name = "outputPathDataGridViewTextBoxColumn";
			// 
			// t4TemplateBindingSource
			// 
			this.t4TemplateBindingSource.AllowNew = false;
			this.t4TemplateBindingSource.DataSource = typeof(Oragon.CodeGen.Templating.T4Template);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(692, 223);
			this.Controls.Add(this.btnExecutar);
			this.Controls.Add(this.templateStatusDataGridView);
			this.Icon = global::Oragon.CodeGen.ConsoleApp.Properties.Resources.Main;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Oragon T4 CodeGen";
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.templateStatusDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.t4TemplateBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView templateStatusDataGridView;
		private System.Windows.Forms.BindingSource t4TemplateBindingSource;
		private System.Windows.Forms.Button btnExecutar;
		private System.Windows.Forms.DataGridViewTextBoxColumn templatePathDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn outputPathDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewImageColumn IconColumn;
	}
}