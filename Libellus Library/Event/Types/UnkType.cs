using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class UnkType : ITypeCreator
	{
		public PmdDataType? CreateType(uint version)
		{
			return new PmdData_RawData();
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;
			if (!PmdTypeFactory.IsSerialized((PmdTypeID)reader.ReadUInt32()) && (typeIDs.Contains(PmdTypeID.Message) || typeIDs.Contains(PmdTypeID.Unit) || typeIDs.Contains(PmdTypeID.Effect)))
			{
				reader.BaseStream.Position = OriginalPos;
				return null;
			}

			reader.BaseStream.Position = OriginalPos + 0x4;
			int size = reader.ReadInt32();
			uint cnt = reader.ReadUInt32();
			if (cnt == 0)
			{
				reader.BaseStream.Position = OriginalPos;
				PmdData_RawData empty = new();
				empty.Data = new();
				return empty;
			}

			PmdData_RawData type = new();
			reader.BaseStream.Position = (long)reader.ReadUInt32();
			for(int i = 0; i < cnt; i++)
			{
				type.Data.Add(reader.ReadBytes(size));
			}

			reader.BaseStream.Position = OriginalPos;
			return type;
		}
	}

	internal class PmdData_RawData : PmdDataType
	{
		[JsonConverter(typeof(JSON.ListByteArrayToHexArray))]
		public List<byte[]> Data { get; set; }

		public PmdData_RawData()
		{
			Data = new List<byte[]>();
		}
		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach (byte[] rawData in Data)
			{
				writer.Write(rawData);
			}

			return;
		}
		internal override int GetCount() => Data.Count;
		// Return 0 if count is zero, else assume all data is same length as first
		internal override int GetSize() => Data.Count == 0 ? 0 : Data[0].Length;
	}
	// Workaround for json to not die
	internal class PmdRawData
	{
		[JsonConverter(typeof(JSON.ByteArrayToHexArray))]
		public byte[] RawData { get; set; }
		public PmdRawData(byte[] data)
		{
			RawData = data;
		}
	}
}
