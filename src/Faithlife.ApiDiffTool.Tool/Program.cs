using System;
using System.Linq;
using CommandLine;
using Faithlife.FacadeGenerator;

namespace Faithlife.ApiDiffTool
{
	class MainClass
	{
		public static int Main(string[] args)
		{
			return Parser.Default.ParseArguments<Options>(args)
				.MapResult(Run, errors => 1);
		}

		static int Run(Options options)
		{
			var module1 = CecilUtility.ReadModule(options.File1);
			var module2 = CecilUtility.ReadModule(options.File2);

			FacadeModuleProcessor.MakePublicFacade(module1, options.IncludeInternals);
			FacadeModuleProcessor.MakePublicFacade(module2, options.IncludeInternals);

			var changes = ApiDiff.FindChanges(module1, module2);

			if (changes.Count == 0)
				Console.WriteLine("No changes");
			foreach (var changeGroup in changes.GroupBy(x => x.IsBreaking))
			{
				Console.WriteLine("{0} changes:", changeGroup.Key ? "Breaking" : "Non-breaking");
				foreach (var change in changeGroup)
					Console.WriteLine(change.Message);
				Console.WriteLine();
			}

			return 0;
		}

		class Options
		{
			[Value(0, Required = true, HelpText = "Path to assembly file 1.")]
			public string File1 { get; set; }

			[Value(1, Required = true, HelpText = "Path to assembly file 2.")]
			public string File2 { get; set; }

			[Option("includeInternals", HelpText = "Include internal types and members.")]
			public bool IncludeInternals { get; set; }
		}
	}
}
