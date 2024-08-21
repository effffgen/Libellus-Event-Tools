using LibellusLibrary.Event.Types.Frame;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_FrameTable : PmdDataType, ITypeCreator
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonConverter(typeof(PmdFrameReader))]
		public List<PmdTargetType> Frames { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				10 or 11 or 12 => new PmdData_FrameTable(),
				_ => new PmdData_RawData()
			};
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;

			reader.BaseStream.Position += 0x4;
			int size = reader.ReadInt32();
			uint count = reader.ReadUInt32();
			reader.BaseStream.Position = (long)reader.ReadUInt32();

			if (version != 10 && version != 11 && version != 12)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"Version {version} FrameTable is not supported! Reading data to Hex array...");
				Console.ResetColor();
				PmdData_RawData unsupportedFrameTable = new();
				for (int i = 0; i < count; i++)
				{
					unsupportedFrameTable.Data.Add(reader.ReadBytes(size));
				}
				reader.BaseStream.Position = OriginalPos;
				return unsupportedFrameTable;
			}

			PmdFrameFactory factory = new();
			Frames = factory.ReadDataTypes(reader, count);

			reader.BaseStream.Position = OriginalPos;
			return this;
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach(PmdTargetType frame in Frames)
			{
				frame.WriteFrame(writer);
			}
		}
		internal override int GetCount() => Frames.Count;
		internal override int GetSize() => 0x3C; // TODO: 0x2C for v9 PMD
	}
}
