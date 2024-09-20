namespace LibellusLibrary.Event.Types.Frame
{
	internal interface ITargetVarying
	{
		/// <summary>
		/// Gets the appropriate variant of a given frame function by reading binary data.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public PmdTargetType GetVariant(BinaryReader reader);
		/// <summary>
		/// Gets the appropriate variant of a given frame function by instance data.
		/// </summary>
		/// <returns></returns>
		public PmdTargetType GetVariant();
	}
}
