namespace LibellusLibrary.Event.Types
{
	internal interface IExternalFile
	{
		public Task SaveExternalFile(string directory);
		public Task LoadExternalFile(string directory);
		public int GetTotalFileSize();
	}
}
