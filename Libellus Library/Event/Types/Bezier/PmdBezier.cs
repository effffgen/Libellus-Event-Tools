using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Bezier
{
	public class PmdBezier
	{
		[JsonPropertyOrder(-100)]
		public uint UNK00 { get; set; }
		[JsonPropertyOrder(-99)]
		public float[] Data { get; set; } = new float[75];
		
		public void ReadBezier(BinaryReader reader)
		{
			UNK00 = reader.ReadUInt32();
			for (int i = 0; i < 75; i++)
			{
				Data[i] = reader.ReadSingle();
			}
		}

		public void WriteBezier(BinaryWriter writer)
		{ 
			writer.Write(UNK00);
			for (int i = 0; i < 75; i++)
			{
				writer.Write(Data[i]);
			}
		}
	}
}