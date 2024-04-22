namespace LibellusLibrary.Event.Types
{
	internal interface IVersionable
	{
		public PmdDataType CreateFromVersion(uint version, BinaryReader reader);
	}
}
