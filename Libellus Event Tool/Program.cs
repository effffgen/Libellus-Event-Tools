using LibellusLibrary.Event;

namespace LibellusEventTool
{
	class Program
	{
		static async Task Main(string[] args)
		{
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
			System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fvi.FileVersion ?? "Null Version"; // Address CS8600 warning
			Console.WriteLine($"Welcome to LEET!\nLibellus Event Editing Tool: v{version}\nNow with better syntax!\n");

			if (args.Length < 1)
			{
				Console.WriteLine("Not enough args!\nPress any button to exit.");
				Console.ReadKey();
				return;
			}
			foreach (string file in args)
			{
				string ext = Path.GetExtension(file).ToLower();
				if (ext == ".pm1" || ext == ".pm2" || ext == ".pm3")
				{
					Console.WriteLine($"Coverting to Json: {file}");
					PmdReader reader = new();
					PolyMovieData pmd = await reader.ReadPmd(file);
					// the "!" in Path.GetDirectoryName(file)! indicates null forgiveness
					// Addresses CS8604 and should be safe considering that if this
					// code path is running, `file` PROBABLY isn't null anyways
					string folder = Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileNameWithoutExtension(file));
					await pmd.ExtractPmd(folder, Path.GetFileName(file));
					continue;
				}

				if (ext == ".json")
				{
					Console.WriteLine($"Coverting to PMD: {file}");
					PolyMovieData pmd = await PolyMovieData.LoadPmd(file);
					string pmdext = $"PM{pmd.MagicCode[3]}";
					pmd.SavePmd(file + "." + pmdext);
				}
			}
			Console.WriteLine("Press Any Button To Exit.");
			Console.ReadKey();
		}
	}
}