using System;
using System.Windows.Forms;

namespace Aleph1.Skeletons.CustomWizard
{
	/// <summary>Form for selecting target framework (.NET Framework or .NET Core)</summary>
	internal partial class FrameworkSelectionForm : Form
	{
		public bool IsNetCore { get; private set; }

		public FrameworkSelectionForm()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.Text = "Select Target Framework";
			this.Size = new System.Drawing.Size(400, 200);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;

			var lblTitle = new Label
			{
				Text = "Choose the target framework for your WebAPI project:",
				Location = new System.Drawing.Point(20, 20),
				Size = new System.Drawing.Size(350, 30),
				Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular)
			};

			var rbNetFramework = new RadioButton
			{
				Text = ".NET Framework 4.8 (ASP.NET Web API)",
				Location = new System.Drawing.Point(20, 60),
				Size = new System.Drawing.Size(350, 25),
				Checked = true,
				Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular)
			};

			var rbNetCore = new RadioButton
			{
				Text = ".NET 8.0 (ASP.NET Core)",
				Location = new System.Drawing.Point(20, 90),
				Size = new System.Drawing.Size(350, 25),
				Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular)
			};

			var btnOK = new Button
			{
				Text = "OK",
				DialogResult = DialogResult.OK,
				Location = new System.Drawing.Point(200, 130),
				Size = new System.Drawing.Size(75, 30)
			};

			var btnCancel = new Button
			{
				Text = "Cancel",
				DialogResult = DialogResult.Cancel,
				Location = new System.Drawing.Point(285, 130),
				Size = new System.Drawing.Size(75, 30)
			};

			btnOK.Click += (sender, e) =>
			{
				this.IsNetCore = rbNetCore.Checked;
				this.DialogResult = DialogResult.OK;
				this.Close();
			};

			this.Controls.Add(lblTitle);
			this.Controls.Add(rbNetFramework);
			this.Controls.Add(rbNetCore);
			this.Controls.Add(btnOK);
			this.Controls.Add(btnCancel);

			this.AcceptButton = btnOK;
			this.CancelButton = btnCancel;
	}
	}
}
