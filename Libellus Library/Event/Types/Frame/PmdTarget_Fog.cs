using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Fog : P3TargetType
	{
		[JsonPropertyOrder(-92)]
		public uint Field14 { get; set; }

		[JsonPropertyOrder(-91)]
		public FogColours? FogColour { get; set; } // Controls colour of fog (affects 3D objects)
		[JsonPropertyOrder(-90)]
		public FogColours? SkyboxColour { get; set; } // Controls colour of Skybox fog

		[JsonPropertyOrder(-89)]
		public float FogNear { get; set; } // Controls how close to the camera the fog begins
		[JsonPropertyOrder(-88)]
		public float FogFar { get; set; } // Controls how far away until the fog ends

		[JsonPropertyOrder(-87)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			Field14 = reader.ReadUInt32();
			FogColour ??= new();
			FogColour.Red = reader.ReadByte();
			FogColour.Green = reader.ReadByte();
			FogColour.Blue = reader.ReadByte();
			FogColour.Alpha = reader.ReadByte();
			SkyboxColour ??= new();
			SkyboxColour.Red = reader.ReadByte();
			SkyboxColour.Green = reader.ReadByte();
			SkyboxColour.Blue = reader.ReadByte();
			SkyboxColour.Alpha = reader.ReadByte(); // Possibly unused/nonfunctional?
			FogNear = reader.ReadSingle();
			FogFar = reader.ReadSingle();
			Data = reader.ReadBytes(20);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(Field14);
			writer.Write(FogColour!.Red);
			writer.Write(FogColour!.Green);
			writer.Write(FogColour!.Blue);
			writer.Write(FogColour!.Alpha);
			writer.Write(SkyboxColour!.Red);
			writer.Write(SkyboxColour!.Green);
			writer.Write(SkyboxColour!.Blue);
			writer.Write(SkyboxColour!.Alpha);
			writer.Write(FogNear);
			writer.Write(FogFar);
			writer.Write(Data);
		}
	}

	internal class FogColours
	{
		public byte Red { get; set; }
		public byte Green { get; set; }
		public byte Blue { get; set; }
		public byte Alpha { get; set; }
	}
}
