using LibellusLibrary.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static LibellusLibrary.Event.Types.Frame.PmdFrameFactory;

namespace LibellusLibrary.Event.Types.Frame
{
	public class PmdTargetType
	{
		[JsonPropertyOrder(-100)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PmdTargetTypeID TargetType { get; set; }
		[JsonPropertyOrder(-99)]
		public ushort StartFrame { get; set; }
		[JsonPropertyOrder(-98)]
		public ushort Length { get; set; }
		[JsonPropertyOrder(-97)]
		public short NameIndex { get; set; }
		[JsonPropertyOrder(-96)]
		public byte FBNResID { get; set; }
		[JsonPropertyOrder(-95)]
		public byte Field09 { get; set; }
		[JsonPropertyOrder(-94)]
		public short Field0A { get; set; }
		[JsonPropertyOrder(-93)]
		public PmdFlags Flags { get; set; }

		public void ReadFrame(BinaryReader reader)
		{
			TargetType = (PmdTargetTypeID)reader.ReadUInt16();
			StartFrame = reader.ReadUInt16();
			Length = reader.ReadUInt16();
			NameIndex = reader.ReadInt16();
			FBNResID = reader.ReadByte();
			Field09 = reader.ReadByte();
			Field0A = reader.ReadInt16();
			Flags = new PmdFlags();
			Flags.ReadData(reader);
			ReadData(reader);
		}
		public void WriteFrame(BinaryWriter writer)
		{
			writer.Write((ushort)TargetType);
			writer.Write((ushort)StartFrame);
			writer.Write((ushort)Length);
			writer.Write((short)NameIndex);
			writer.Write((byte)FBNResID);
			writer.Write((byte)Field09);
			writer.Write((short)Field0A);
			Flags.WriteData(writer);
			WriteData(writer);
		}
		protected virtual void ReadData(BinaryReader reader) => throw new InvalidOperationException();
		protected virtual void WriteData(BinaryWriter writer) => throw new InvalidOperationException();
	}

	internal class PmdTarget_Unknown : PmdTargetType
	{
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; }
		protected override void ReadData(BinaryReader reader)
		{
			Data = reader.ReadBytes(0x28);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer?.Write(Data);
		}
	}
}
