using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_Stage : PmdDataType, ITypeCreator
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public PmdData_StageDef StageData { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public PmdDataType? CreateType(uint version)
		{
			return new PmdData_Stage();
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;

			reader.BaseStream.Position = OriginalPos + 0x8;
			uint count = reader.ReadUInt32();
			if (count == 0)
			{
				reader.BaseStream.Position = OriginalPos;
				return this;
			}
			reader.BaseStream.Position = (long)reader.ReadUInt32();

			StageData = new();
			StageData.ReadStage(reader);

			reader.BaseStream.Position = OriginalPos;
			return this;
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			if (StageData is not null)
			{
				StageData.WriteStage(writer);
			}
		}

		internal override int GetCount() => StageData is null ? 0 : 1;
		internal override int GetSize() => 0x10;
	}

	public class PmdData_StageDef
	{
		[JsonConverter(typeof(JSON.ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();
		public uint EventMajor { get; set; }
		public uint EventMinor { get; set; }

		public void ReadStage(BinaryReader reader)
		{
			Data = reader.ReadBytes(8);
			EventMajor = reader.ReadUInt32();
			EventMinor = reader.ReadUInt32();
		}

		public void WriteStage(BinaryWriter writer)
		{
			writer.Write(Data);
			writer.Write(EventMajor);
			writer.Write(EventMinor);
		}
	}
}
