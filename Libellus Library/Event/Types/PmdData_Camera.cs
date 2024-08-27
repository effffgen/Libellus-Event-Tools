using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_Camera : PmdDataType, ITypeCreator
	{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		[JsonInclude]
		public List<Pmd_CameraDef> Cameras { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				9 => new PmdData_Camera(),
				_ => new PmdData_RawData(),
			};
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;

			reader.BaseStream.Position = OriginalPos + 0x8;
			uint count = reader.ReadUInt32();

			reader.BaseStream.Position = OriginalPos + 0xC;
			reader.BaseStream.Position = (long)reader.ReadUInt32();

			Cameras = new();
			for (int i = 0; i < count; i++)
			{
				Pmd_CameraDef currentCamera = new();
				currentCamera.ReadCamera(reader);
				Cameras.Add(currentCamera);
			}

			reader.BaseStream.Position = OriginalPos;
			return this;
		}
		
		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			foreach(Pmd_CameraDef cam in Cameras)
			{
				cam.WriteCamera(writer);
			}
		}
		internal override int GetCount() => Cameras.Count;
		internal override int GetSize() => 0x10;
	}

	internal class Pmd_CameraDef
	{
		[JsonPropertyOrder(-100)]
		public byte CameraIndex { get; set; } // Datatype not known for certain, only a guess for now

		[JsonPropertyOrder(-99)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();
		
		public void ReadCamera(BinaryReader reader)
		{
			CameraIndex = reader.ReadByte();
			Data = reader.ReadBytes(0xF);
		}

		public void WriteCamera(BinaryWriter writer)
		{
			writer.Write(CameraIndex);
			writer.Write(Data);
		}
	}
}
