using LibellusLibrary.Event;

namespace LibellusEventTool
{
	class Program
	{
		/// <summary>
		/// Controls whether to convert all PMD or JSON files contained within a passed folder and it's subfolders.
		/// </summary>
		private static bool _recurse = false;
		static async Task Main(string[] args)
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.FileVersion ?? "Null Version"; // Address CS8600 warning
			Console.WriteLine($"Welcome to LEET!\nLibellus Event Editing Tool: v{version}\nNow with better syntax!\n");

			_recurse = args.Contains("-r", StringComparer.OrdinalIgnoreCase);
			bool noConfirmation = args.Contains("-no-confirm", StringComparer.OrdinalIgnoreCase);
			int numberPaths = args.ToList().FindAll(value => !value.StartsWith('-')).Count;
			if (numberPaths < 1)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Not enough args!");
				Console.ResetColor();
				if (!noConfirmation)
				{
					Console.WriteLine("Press any button to exit.");
					Console.ReadKey();
				}

				return;
			}
			await ConvertPaths(args);
			if (!noConfirmation)
			{
				Console.WriteLine("Press Any Button To Exit.");
				Console.ReadKey();
			}
		}

		private static async Task ConvertPaths(string[] paths)
		{
			foreach (string file in paths)
			{
				string ext = Path.GetExtension(file).ToLower();
				if (ext == ".pm1" || ext == ".pm2" || ext == ".pm3")
				{
					Console.WriteLine($"Coverting to Json: {file}");
					PmdReader reader = new();
					PolyMovieData pmd = await reader.ReadPmd(file);
					// the "!" in Path.GetDirectoryName(file)! indicates null forgiveness; should be safe & addresses CS8604
					string folder = Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileNameWithoutExtension(file));
					await pmd.ExtractPmd(folder, Path.GetFileName(file));
				}
				else if (ext == ".json")
				{
					Console.WriteLine($"Coverting to PMD: {file}");
					PolyMovieData pmd = await PolyMovieData.LoadPmd(file);
					pmd.SavePmd($"{file}.PM{pmd.MagicCode[3]}");
				}
				else if (Directory.Exists(file))
				{
					await ConvertPaths(Directory.GetFiles(file, "*", _recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
				}
			}
		}
	}
}