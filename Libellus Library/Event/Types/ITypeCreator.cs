namespace LibellusLibrary.Event.Types
{
	public interface ITypeCreator
	{
		public PmdDataType? CreateType(uint version);
		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory);
	}
}
