namespace LibellusLibrary.Utils.IO
{
	public static class IOUtils
	{
		/// <summary>
		/// Makes reader seek to position.
		/// </summary>
		/// <param name="reader">This reader.</param>
		/// <param name="position">Position to seek to.</param>
		public static void FSeek(this BinaryReader reader, long position)
		{
			reader.BaseStream.Position = position;
		}
		/// <summary>
		/// Gets reader's position.
		/// </summary>
		/// <param name="reader">This reader.</param>
		public static long FTell(this BinaryReader reader)
		{
			return reader.BaseStream.Position;
		}
		/// <summary>
		/// Advances reader's position.
		/// </summary>
		/// <param name="reader">This reader.</param>
		/// <param name="position">Number of bytes to seek ahead.</param>
		public static void FSkip(this BinaryReader reader, long position)
		{
			reader.BaseStream.Position += position;
		}
		/// <summary>
		/// Makes writer seek to position.
		/// </summary>
		/// <param name="writer">This writer.</param>
		/// <param name="position">Position to seek to.</param>
		public static void FSeek(this BinaryWriter writer, long position)
		{
			writer.BaseStream.Position = position;
		}
		/// <summary>
		/// Gets writer's position.
		/// </summary>
		/// <param name="reader">This writer.</param>
		public static long FTell(this BinaryWriter writer)
		{
			return writer.BaseStream.Position;
		}
		/// <summary>
		/// Advances writer's position.
		/// </summary>
		/// <param name="reader">This writer.</param>
		/// <param name="position">Number of bytes to seek ahead.</param>
		public static void FSkip(this BinaryWriter writer, long position)
		{
			writer.BaseStream.Position += position;
		}
	}
}
