using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Replacer.Business.Workflow;

namespace Replacer
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			this.checkBoxReplaceContent.CheckedChanged += new EventHandler(NotifyUIChange);
			this.checkBoxRenameFileNames.CheckedChanged += new EventHandler(NotifyUIChange);
			this.checkBoxRenameFolders.CheckedChanged += new EventHandler(NotifyUIChange);

			this.textBoxSourceDir.TextChanged += new EventHandler(NotifyUIChange);
			this.textBoxTargetDir.TextChanged += new EventHandler(NotifyUIChange);
			this.textBoxFileNamePattern.TextChanged += new EventHandler(NotifyUIChange);

			this.Load += new EventHandler(NotifyUIChange);


			replaceTermBindingSource.Add(new ReplaceTerm() { Find = "BRQ.SI", Replace = "Oragon" });
			replaceTermBindingSource.Add(new ReplaceTerm() { Find = "BRQ", Replace = "Oragon" });
			replaceTermBindingSource.Add(new ReplaceTerm() { Find = "brq", Replace = "oragon" });

			textBoxSourceDir.Text = @"D:\Projetos\ora\trunk\ArchitectureOragon\";
			textBoxTargetDir.Text = @"D:\Projetos\ora\trunk\Architecture\";

			textBoxFileNamePattern.Text = "*.js;*.cs;*.*proj";
		}



		public void NotifyUIChange(object sender, EventArgs e)
		{
			this.buttonExecute.Enabled = this.CanExecute();
		}

		public bool CanExecute()
		{
			if (checkBoxReplaceContent.Checked == false && checkBoxRenameFileNames.Checked == false && checkBoxRenameFolders.Checked == false)
				return false;

			if (string.IsNullOrEmpty(this.textBoxSourceDir.Text))
				return false;
			else if (System.IO.Directory.Exists(this.textBoxSourceDir.Text) == false)
				return false;

			if (string.IsNullOrEmpty(this.textBoxTargetDir.Text))
				return false;
			else if (new System.IO.DirectoryInfo(this.textBoxTargetDir.Text).Parent.Exists == false)
				return false;

			if (replaceTermBindingSource.Count == 0)
				return false;


			return true;
		}

		private void buttonExecute_Click(object sender, EventArgs e)
		{
			ReplacerWorkflow replacerWorkflow = new ReplacerWorkflow();
			RenameResponse renameResponse = new RenameResponse()
			{
				SourceDir = new System.IO.DirectoryInfo(this.textBoxSourceDir.Text),
				TargetDir = new System.IO.DirectoryInfo(this.textBoxTargetDir.Text),
				FileNamePattern = this.textBoxFileNamePattern.Text,
				ReplaceDirectoryNames = this.checkBoxRenameFolders.Checked,
				ReplaceFileContent = this.checkBoxReplaceContent.Checked,
				ReplaceFileNames = this.checkBoxRenameFileNames.Checked,
				ReplaceTerms = this.replaceTermBindingSource.List.Cast<ReplaceTerm>().ToList()
			};
			replacerWorkflow.Replace(renameResponse);

			MessageBox.Show("Copiado");

			//renameResponse.TargetDir.Delete(true);
			//MessageBox.Show("Excluído");

			Application.Exit();
		}

	}
}
