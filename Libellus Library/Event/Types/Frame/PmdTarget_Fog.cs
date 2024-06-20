using LibellusLibrary.JSON;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class PmdTarget_Fog : PmdTargetType
	{
		[JsonPropertyOrder(-92)]
		public FogColours? SkyboxColour { get; set; } // Controls colour of Skybox fog

		[JsonPropertyOrder(-92)]
		public FogColours? FogColour { get; set; } // Controls colour of fog (affects 3D objects)

		[JsonPropertyOrder(-92)]
		public float FogNear { get; set; } // Controls how close to the camera the fog begins

		[JsonPropertyOrder(-92)]
		public float FogFar { get; set; } // Controls how far away until the fog ends

		[JsonPropertyOrder(-91)]
		[JsonConverter(typeof(ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		protected override void ReadData(BinaryReader reader)
		{
			SkyboxColour ??= new();
			SkyboxColour.Red = reader.ReadByte();
			SkyboxColour.Green = reader.ReadByte();
			SkyboxColour.Blue = reader.ReadByte();
			SkyboxColour.Alpha = reader.ReadByte(); // Possibly unused/nonfunctional?
			FogColour ??= new();
			FogColour.Red = reader.ReadByte();
			FogColour.Green = reader.ReadByte();
			FogColour.Blue = reader.ReadByte();
			FogColour.Alpha = reader.ReadByte();
			FogNear = reader.ReadSingle();
			FogFar = reader.ReadSingle();
			Data = reader.ReadBytes(24);
		}

		protected override void WriteData(BinaryWriter writer)
		{
			writer.Write(SkyboxColour!.Red);
			writer.Write(SkyboxColour!.Green);
			writer.Write(SkyboxColour!.Blue);
			writer.Write(SkyboxColour!.Alpha);
			writer.Write(FogColour!.Red);
			writer.Write(FogColour!.Green);
			writer.Write(FogColour!.Blue);
			writer.Write(FogColour!.Alpha);
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
