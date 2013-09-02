using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Oragon.CodeGen.ConsoleApp
{
	public partial class DetailForm : Form
	{
		public DetailForm()
		{
			InitializeComponent();
		}

		public DetailForm(string message)
			:this()
		{
			this.txtInfo.Text = message;
		}

		private void DetailForm_Load(object sender, EventArgs e)
		{
			this.txtInfo.Select(0, 0);
		}
	}
}
