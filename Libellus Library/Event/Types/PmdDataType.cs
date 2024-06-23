using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	public class PmdDataType
	{
		[JsonPropertyOrder(-1)]
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public PmdTypeID Type { get; set; }

		internal virtual void SaveData(PmdBuilder builder, BinaryWriter writer) => throw new NotImplementedException(this.Type.ToString());
		internal virtual int GetCount() => throw new NotImplementedException(this.Type.ToString());
		internal virtual int GetSize() => 0; // Data size doesnt matter for certain types
	}

	public enum PmdTypeID
	{
		CutInfo = 0,
		Name = 1,
		Stage = 2,
		Unit = 3,
		FrameTable = 4,
		Camera = 5,
		Message = 6,
		Effect = 7,
		EffectData = 8,
		UnitData = 9,
		F1 = 10,
		F2 = 11,
		FTB = 12,
		SLight = 13,
		SFog = 14,
		Blur2 = 15,
		MultBlur = 16,
		DistBlur = 17,
		Filter = 18,
		MultFilter = 19,
		RipBlur = 20,
		ObjectTable = 21,

		RainData = 25,
		BezierTable = 26,
		RefTable = 27,
		MAX = 28
	}

}
