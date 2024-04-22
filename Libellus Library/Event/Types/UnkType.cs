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

			reader.BaseStream.Position += 0x8;
			uint cnt = reader.ReadUInt32();
			if (cnt== 0)
			{
				reader.BaseStream.Position = OriginalPos;
				PmdData_RawData empty = new();
				empty.Data = new();
				return empty;
			}
			reader.BaseStream.Position = OriginalPos;
			
			if (!PmdTypeFactory.IsSerialized((PmdTypeID)reader.ReadUInt32()) && (typeIDs.Contains(PmdTypeID.Message) || typeIDs.Contains(PmdTypeID.Unit)))
			{
				reader.BaseStream.Position = OriginalPos;
				return null;
			}
			
			PmdData_RawData type = new();
			reader.BaseStream.Position = OriginalPos + 0x4;
			uint size = reader.ReadUInt32();

			reader.BaseStream.Position = OriginalPos + 0xC;
			reader.BaseStream.Position = (long)reader.ReadUInt32();
			for(int i = 0; i < cnt; i++)
			{
				type.Data.Add(reader.ReadBytes((int)size));
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
			foreach (var data in Data)
			{
				writer.Write(data);
			}

			return;
		}
		internal override int GetCount()
		{
			return Data.Count;
		}
		internal override int GetSize()
		{
			if(Data.Count == 0)
			{
				return 0;
			}
			// Here we assume that all the data has the same length
			return Data[0].Length;
		}
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
