using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.TemplateWizard;

namespace Aleph1.Skeletons.CustomWizard
{
	public class BeforeWizardWebAPI : IWizard
	{
		private Solution2 solution;
		public static bool IsNetCoreSelected { get; set; }

		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			replacementsDictionary.EnrichTemplateVariables();

			// Show framework selection dialog
			using (var form = new FrameworkSelectionForm())
			{
				if (form.ShowDialog() == DialogResult.OK)
				{
					IsNetCoreSelected = form.IsNetCore;
					replacementsDictionary["$IsNetCore$"] = form.IsNetCore ? "true" : "false";
					replacementsDictionary["$TargetFramework$"] = form.IsNetCore ? "net8.0" : "net48";
				}
				else
				{
					// User cancelled, throw to abort template creation
					throw new WizardCancelledException("Template creation was cancelled by the user.");
				}
			}

			// Close new solution
			solution = (Solution2)((DTE)automationObject).Solution;
			solution.Close();

			// Delete old directory(in my case VS creating it) and change destination
			string oldDestinationDirectory = replacementsDictionary["$destinationdirectory$"];
			if (Directory.Exists(oldDestinationDirectory))
			{
				Directory.Delete(oldDestinationDirectory, true);
			}

			string newDestinationDirectory = Path.Combine($"{oldDestinationDirectory}", @"..\");
			replacementsDictionary["$destinationdirectory$"] = Path.GetFullPath(newDestinationDirectory);
		}
		public void RunFinished()
		{
			solution.Close();
		}

		public void BeforeOpeningFile(ProjectItem projectItem)
		{
		}

		public void ProjectFinishedGenerating(Project project)
		{
		}

		public void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
		}

		public bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}
	}
}