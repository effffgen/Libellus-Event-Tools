using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Ctlcam : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		public ushort Field14 { get; set; } // Possibly used to define UNIT to parent to (even in MOVE_DIRECT)?
		
		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public CameraModeEnum CameraMode { get; set; }

		[JsonPropertyOrder(-90)]
		public ushort ActiveSelUnit { get; set; }
		
		[JsonPropertyOrder(-89)]
		public short ActiveViewLength { get; set; } // limited to 200 - 3000 by editor

		[JsonPropertyOrder(-88)]
		public float CameraX { get; set; }
		
		[JsonPropertyOrder(-87)]
		public float CameraY { get; set; }
		
		[JsonPropertyOrder(-86)]
		public float CameraZ { get; set; }
		
		[JsonPropertyOrder(-85)]
		public float CameraPitch { get; set; }
		
		[JsonPropertyOrder(-84)]
		public float CameraYaw { get; set; }
		
		[JsonPropertyOrder(-83)]
		public float CameraFOV { get; set; }
		
		[JsonPropertyOrder(-82)]
		public sbyte CurveID { get; set; } // only exposed in editor when CameraMode is MOVE_DIRECT

		[JsonPropertyOrder(-81)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		internal enum CameraModeEnum : ushort
		{
			MOVE_DIRECT = 0,
			ACTIVE = 1,
		}

		protected override void ReadData(BinaryReader reader)
		{
			Field14 = reader.ReadUInt16();
			CameraMode = (CameraModeEnum)reader.ReadUInt16();
			ActiveSelUnit = reader.ReadUInt16();
			ActiveViewLength = reader.ReadInt16();
			CameraX = reader.ReadSingle();
			CameraY = reader.ReadSingle();
			CameraZ = reader.ReadSingle();
			CameraPitch = reader.ReadSingle();
			CameraYaw = reader.ReadSingle();
			CameraFOV = reader.ReadSingle();
			CurveID = reader.ReadSByte();
			Data = reader.ReadBytes(7);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Field14);
			writer.Write((ushort)CameraMode);
			writer.Write(ActiveSelUnit);
			writer.Write(ActiveViewLength);
			writer.Write(CameraX);
			writer.Write(CameraY);
			writer.Write(CameraZ);
			writer.Write(CameraPitch);
			writer.Write(CameraYaw);
			writer.Write(CameraFOV);
			writer.Write(CurveID);
			writer.Write(Data);
		}
	}
}
