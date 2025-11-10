using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio.TemplateWizard;

namespace Aleph1.Skeletons.CustomWizard
{
	public class AfterWizardWebAPI : IWizard
	{
		private DTE dte;
		private string destinationDirectory;
		private string solutionName;
		private bool isNetCore;

		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			dte = (DTE)automationObject;
			destinationDirectory = replacementsDictionary["$destinationdirectory$"];
			solutionName = replacementsDictionary["$safeprojectname$"];
			isNetCore = BeforeWizardWebAPI.IsNetCoreSelected;
		}

		public void RunFinished()
		{
			// Open "old" solution
			string pathToOldSolution = Path.Combine(destinationDirectory, solutionName + ".sln");

			Solution2 solution = (Solution2)dte.Solution;
			solution.Open(pathToOldSolution);

			if (isNetCore)
			{
				// Convert WebAPI project to .NET Core
				ConvertToNetCore(solution);
			}

			solution.Properties.Item("StartupProject").Value = solutionName + ".WebAPI";
			solution.SolutionBuild.Clean(true);
			solution.SolutionBuild.Build();
		}

		private void ConvertToNetCore(Solution2 solution)
		{
			string webApiProjectName = solutionName + ".WebAPI";
			Project webApiProject = solution.FindProjectInSolution(webApiProjectName);

			if (webApiProject == null)
			{
				return;
			}

			string projectPath = webApiProject.FullName;
			string projectDirectory = Path.GetDirectoryName(projectPath);

			// Convert .csproj to SDK-style format
			ConvertProjectFileToNetCore(projectPath);

			// Update Web.config to appsettings.json structure (optional - can be done manually)
			// For now, we'll leave Web.config but note that .NET Core uses appsettings.json
		}

		private void ConvertProjectFileToNetCore(string projectPath)
		{
			if (!File.Exists(projectPath))
			{
				return;
			}

			string projectContent = File.ReadAllText(projectPath);

			// Check if already SDK-style
			if (projectContent.Contains("<Project Sdk="))
			{
				// Already SDK-style, just update target framework
				projectContent = Regex.Replace(projectContent, 
					@"<TargetFrameworkVersion>v\d+\.\d+</TargetFrameworkVersion>", 
					"<TargetFramework>net8.0</TargetFramework>");
			}
			else
			{
				// Convert old-style to SDK-style
				// This is a simplified conversion - full conversion would be more complex
				// For now, we'll create a basic SDK-style project file
				var sb = new StringBuilder();
				sb.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk.Web\">");
				sb.AppendLine();
				sb.AppendLine("  <PropertyGroup>");
				sb.AppendLine("    <TargetFramework>net8.0</TargetFramework>");
				sb.AppendLine("    <Nullable>enable</Nullable>");
				sb.AppendLine("    <ImplicitUsings>enable</ImplicitUsings>");
				sb.AppendLine("  </PropertyGroup>");
				sb.AppendLine();
				sb.AppendLine("</Project>");

				projectContent = sb.ToString();
			}

			File.WriteAllText(projectPath, projectContent);
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