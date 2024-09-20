namespace LibellusLibrary.Event.Types.Frame
{
	internal interface ITargetVarying
	{
		public PmdTargetType GetVariant(BinaryReader reader);
		public PmdTargetType GetVariant();
	}
}
