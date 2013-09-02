namespace Replacer
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
			this.folderBrowserDialogSource = new System.Windows.Forms.FolderBrowserDialog();
			this.folderBrowserDialogTarget = new System.Windows.Forms.FolderBrowserDialog();
			this.buttonExecute = new System.Windows.Forms.Button();
			this.textBoxSourceDir = new System.Windows.Forms.TextBox();
			this.textBoxTargetDir = new System.Windows.Forms.TextBox();
			this.checkBoxReplaceContent = new System.Windows.Forms.CheckBox();
			this.checkBoxRenameFileNames = new System.Windows.Forms.CheckBox();
			this.checkBoxRenameFolders = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.buttonOpenSourceDialog = new System.Windows.Forms.Button();
			this.buttonOpenTargetDialog = new System.Windows.Forms.Button();
			this.textBoxFileNamePattern = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.findDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.replaceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.replaceTermBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.replaceTermBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// folderBrowserDialogSource
			// 
			this.folderBrowserDialogSource.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.folderBrowserDialogSource.ShowNewFolderButton = false;
			// 
			// folderBrowserDialogTarget
			// 
			this.folderBrowserDialogTarget.RootFolder = System.Environment.SpecialFolder.MyComputer;
			this.folderBrowserDialogTarget.ShowNewFolderButton = false;
			// 
			// buttonExecute
			// 
			this.buttonExecute.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonExecute.Location = new System.Drawing.Point(14, 367);
			this.buttonExecute.Name = "buttonExecute";
			this.buttonExecute.Size = new System.Drawing.Size(1011, 53);
			this.buttonExecute.TabIndex = 0;
			this.buttonExecute.Text = "Execute";
			this.buttonExecute.UseVisualStyleBackColor = true;
			this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
			// 
			// textBoxSourceDir
			// 
			this.textBoxSourceDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxSourceDir.Location = new System.Drawing.Point(280, 9);
			this.textBoxSourceDir.Name = "textBoxSourceDir";
			this.textBoxSourceDir.Size = new System.Drawing.Size(719, 21);
			this.textBoxSourceDir.TabIndex = 1;
			this.textBoxSourceDir.Text = "c:\\temp\\";
			// 
			// textBoxTargetDir
			// 
			this.textBoxTargetDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxTargetDir.Location = new System.Drawing.Point(280, 39);
			this.textBoxTargetDir.Name = "textBoxTargetDir";
			this.textBoxTargetDir.Size = new System.Drawing.Size(719, 21);
			this.textBoxTargetDir.TabIndex = 2;
			this.textBoxTargetDir.Text = "c:\\temp2\\";
			// 
			// checkBoxReplaceContent
			// 
			this.checkBoxReplaceContent.AutoSize = true;
			this.checkBoxReplaceContent.Checked = true;
			this.checkBoxReplaceContent.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxReplaceContent.Location = new System.Drawing.Point(14, 12);
			this.checkBoxReplaceContent.Name = "checkBoxReplaceContent";
			this.checkBoxReplaceContent.Size = new System.Drawing.Size(120, 17);
			this.checkBoxReplaceContent.TabIndex = 3;
			this.checkBoxReplaceContent.Text = "Replace Content";
			this.checkBoxReplaceContent.UseVisualStyleBackColor = true;
			// 
			// checkBoxRenameFileNames
			// 
			this.checkBoxRenameFileNames.AutoSize = true;
			this.checkBoxRenameFileNames.Checked = true;
			this.checkBoxRenameFileNames.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxRenameFileNames.Location = new System.Drawing.Point(14, 35);
			this.checkBoxRenameFileNames.Name = "checkBoxRenameFileNames";
			this.checkBoxRenameFileNames.Size = new System.Drawing.Size(102, 17);
			this.checkBoxRenameFileNames.TabIndex = 4;
			this.checkBoxRenameFileNames.Text = "Rename Files";
			this.checkBoxRenameFileNames.UseVisualStyleBackColor = true;
			// 
			// checkBoxRenameFolders
			// 
			this.checkBoxRenameFolders.AutoSize = true;
			this.checkBoxRenameFolders.Checked = true;
			this.checkBoxRenameFolders.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxRenameFolders.Location = new System.Drawing.Point(14, 58);
			this.checkBoxRenameFolders.Name = "checkBoxRenameFolders";
			this.checkBoxRenameFolders.Size = new System.Drawing.Size(139, 17);
			this.checkBoxRenameFolders.TabIndex = 5;
			this.checkBoxRenameFolders.Text = "Rename Directories";
			this.checkBoxRenameFolders.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(169, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(109, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Source Directory:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(169, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(106, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Target Directory:";
			// 
			// buttonOpenSourceDialog
			// 
			this.buttonOpenSourceDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOpenSourceDialog.Location = new System.Drawing.Point(1007, 7);
			this.buttonOpenSourceDialog.Name = "buttonOpenSourceDialog";
			this.buttonOpenSourceDialog.Size = new System.Drawing.Size(28, 23);
			this.buttonOpenSourceDialog.TabIndex = 8;
			this.buttonOpenSourceDialog.Text = "...";
			this.buttonOpenSourceDialog.UseVisualStyleBackColor = true;
			// 
			// buttonOpenTargetDialog
			// 
			this.buttonOpenTargetDialog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOpenTargetDialog.Location = new System.Drawing.Point(1007, 37);
			this.buttonOpenTargetDialog.Name = "buttonOpenTargetDialog";
			this.buttonOpenTargetDialog.Size = new System.Drawing.Size(28, 23);
			this.buttonOpenTargetDialog.TabIndex = 9;
			this.buttonOpenTargetDialog.Text = "...";
			this.buttonOpenTargetDialog.UseVisualStyleBackColor = true;
			// 
			// textBoxFileNamePattern
			// 
			this.textBoxFileNamePattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxFileNamePattern.Location = new System.Drawing.Point(280, 74);
			this.textBoxFileNamePattern.Name = "textBoxFileNamePattern";
			this.textBoxFileNamePattern.Size = new System.Drawing.Size(719, 21);
			this.textBoxFileNamePattern.TabIndex = 10;
			this.textBoxFileNamePattern.Text = "*.*";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(174, 77);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "FileNamePattern";
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.findDataGridViewTextBoxColumn,
            this.replaceDataGridViewTextBoxColumn});
			this.dataGridView1.DataSource = this.replaceTermBindingSource;
			this.dataGridView1.Location = new System.Drawing.Point(14, 117);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(1011, 244);
			this.dataGridView1.TabIndex = 12;
			// 
			// findDataGridViewTextBoxColumn
			// 
			this.findDataGridViewTextBoxColumn.DataPropertyName = "Find";
			this.findDataGridViewTextBoxColumn.HeaderText = "Find";
			this.findDataGridViewTextBoxColumn.Name = "findDataGridViewTextBoxColumn";
			this.findDataGridViewTextBoxColumn.Width = 200;
			// 
			// replaceDataGridViewTextBoxColumn
			// 
			this.replaceDataGridViewTextBoxColumn.DataPropertyName = "Replace";
			this.replaceDataGridViewTextBoxColumn.HeaderText = "Replace";
			this.replaceDataGridViewTextBoxColumn.Name = "replaceDataGridViewTextBoxColumn";
			this.replaceDataGridViewTextBoxColumn.Width = 200;
			// 
			// replaceTermBindingSource
			// 
			this.replaceTermBindingSource.DataSource = typeof(Replacer.ReplaceTerm);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1039, 432);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBoxFileNamePattern);
			this.Controls.Add(this.buttonOpenTargetDialog);
			this.Controls.Add(this.buttonOpenSourceDialog);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkBoxRenameFolders);
			this.Controls.Add(this.checkBoxRenameFileNames);
			this.Controls.Add(this.checkBoxReplaceContent);
			this.Controls.Add(this.textBoxTargetDir);
			this.Controls.Add(this.textBoxSourceDir);
			this.Controls.Add(this.buttonExecute);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MinimumSize = new System.Drawing.Size(697, 300);
			this.Name = "MainForm";
			this.Text = "Renamer";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.replaceTermBindingSource)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSource;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogTarget;
		private System.Windows.Forms.Button buttonExecute;
		private System.Windows.Forms.TextBox textBoxSourceDir;
		private System.Windows.Forms.TextBox textBoxTargetDir;
		private System.Windows.Forms.CheckBox checkBoxReplaceContent;
		private System.Windows.Forms.CheckBox checkBoxRenameFileNames;
		private System.Windows.Forms.CheckBox checkBoxRenameFolders;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button buttonOpenSourceDialog;
		private System.Windows.Forms.Button buttonOpenTargetDialog;
		private System.Windows.Forms.TextBox textBoxFileNamePattern;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn findDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn replaceDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource replaceTermBindingSource;
	}
}

