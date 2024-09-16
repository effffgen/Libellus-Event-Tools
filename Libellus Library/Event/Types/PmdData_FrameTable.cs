using LibellusLibrary.Event.Types.Frame;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_FrameTable : PmdDataType, ITypeCreator, IVersionable
	{
		public PmdDataType CreateFromVersion(uint version, BinaryReader reader)
		{
			return version switch
			{
				4 => new PmdData_SMT3FrameTable(reader, version),
				9 => new PmdData_DDSFrameTable(reader, version),
				10 or 11 or 12 => new PmdData_P3FrameTable(reader, version),
				_ => throw new NotImplementedException()
			};
		}

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				4 => new PmdData_SMT3FrameTable(),
				9 => new PmdData_DDSFrameTable(),
				10 or 11 or 12 => new PmdData_P3FrameTable(),
				_ => throw new NotImplementedException()
			};
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;
			PmdDataType frameTbl = CreateFromVersion(version, reader);
			reader.BaseStream.Position = OriginalPos;
			return frameTbl;
		}
	}

	public class PmdData_SMT3FrameTable : PmdDataType
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonConverter(typeof(SMT3FrameReader))]
		public List<PmdTargetType> Frames { get; set; }

		public PmdData_SMT3FrameTable()
		{
			return;
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public PmdData_SMT3FrameTable(BinaryReader reader, uint version)
		{
			reader.BaseStream.Position += 0x8;
			uint count = reader.ReadUInt32();
			reader.BaseStream.Position = (long)reader.ReadUInt32();
			PmdFrameFactory factory = new();
			Frames = factory.ReadDataTypes(reader, count, version);
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach (PmdTargetType frame in Frames)
			{
				frame.WriteFrame(writer);
			}
		}
		internal override int GetCount() => Frames.Count;
		internal override int GetSize() => 0x10;
	}

	public class PmdData_DDSFrameTable : PmdDataType
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonConverter(typeof(DDSFrameReader))]
		public List<PmdTargetType> Frames { get; set; }

		public PmdData_DDSFrameTable()
		{
			return;
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public PmdData_DDSFrameTable(BinaryReader reader, uint version)
		{
			reader.BaseStream.Position += 0x8;
			uint count = reader.ReadUInt32();
			reader.BaseStream.Position = (long)reader.ReadUInt32();
			PmdFrameFactory factory = new();
			Frames = factory.ReadDataTypes(reader, count, version);
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach (PmdTargetType frame in Frames)
			{
				frame.WriteFrame(writer);
			}
		}
		internal override int GetCount() => Frames.Count;
		internal override int GetSize() => 0x2C;
	}

	public class PmdData_P3FrameTable : PmdDataType
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonConverter(typeof(P3FrameReader))]
		public List<PmdTargetType> Frames { get; set; }

		public PmdData_P3FrameTable()
		{
			return;
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public PmdData_P3FrameTable(BinaryReader reader, uint version)
		{
			reader.BaseStream.Position += 0x8;
			uint count = reader.ReadUInt32();
			reader.BaseStream.Position = (long)reader.ReadUInt32();
			PmdFrameFactory factory = new();
			Frames = factory.ReadDataTypes(reader, count, version);
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach (PmdTargetType frame in Frames)
			{
				frame.WriteFrame(writer);
			}
		}
		internal override int GetCount() => Frames.Count;
		internal override int GetSize() => 0x3C;
	}
}
