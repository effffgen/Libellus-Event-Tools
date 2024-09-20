using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Frame
{
	internal class SMT3FrameReader : P3FrameReader
	{
		// TODO: Do SMT3/v4 framefuncs have variants?
		public override List<PmdTargetType>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<PmdTargetType> frames = new();

			reader.Read();
			List<PmdTargetType> abstractTypes = ReadAbstractTargets(reader, options);

			foreach (PmdTargetType abstractType in abstractTypes)
			{
				Type trueDataType = PmdFrameFactory.GetSMT3FrameType(abstractType.TargetType).GetType();
				frames.Add((PmdTargetType)JsonSerializer.Deserialize(ref reader, trueDataType, options)!);
				reader.Read();
			}

			return frames;
		}
	}

	// TODO: Do DDS/v9 framefuncs have variants?
	internal class DDSFrameReader : P3FrameReader
	{
		public override List<PmdTargetType>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<PmdTargetType> frames = new();

			reader.Read();
			List<PmdTargetType> abstractTypes = ReadAbstractTargets(reader, options);
			
			foreach (PmdTargetType abstractType in abstractTypes)
			{
				Type trueDataType = PmdFrameFactory.GetDDSFrameType(abstractType.TargetType).GetType();
				frames.Add((PmdTargetType)JsonSerializer.Deserialize(ref reader, trueDataType, options)!);
				reader.Read();
			}

			return frames;
		}
	}

	internal class P3FrameReader : JsonConverter<List<PmdTargetType>>
	{
		public override List<PmdTargetType>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<PmdTargetType> frames = new();

			reader.Read();
			List<PmdTargetType> abstractTypes = ReadAbstractTargets(reader, options);

			foreach (PmdTargetType abstractType in abstractTypes)
			{
				Type trueDataType = PmdFrameFactory.GetP3FrameType(abstractType.TargetType).GetType();
				if (typeof(ITargetVarying).IsAssignableFrom(trueDataType))
				{
					Utf8JsonReader variantReader = reader;
					trueDataType = ((ITargetVarying)JsonSerializer.Deserialize(ref variantReader, trueDataType, options)!).GetVariant().GetType();
				}
				frames.Add((PmdTargetType)JsonSerializer.Deserialize(ref reader, trueDataType, options)!);
				reader.Read();
			}

			return frames;
		}

		public override void Write(Utf8JsonWriter writer, List<PmdTargetType> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (PmdTargetType data in value)
			{
				writer.WriteRawValue(JsonSerializer.Serialize<object>(data, options));
			}
			writer.WriteEndArray();

		}

		public static List<PmdTargetType> ReadAbstractTargets(Utf8JsonReader abstractReader, JsonSerializerOptions options)
		{
			List<PmdTargetType> abstractTypes = new();
			while (abstractReader.TokenType != JsonTokenType.EndArray)
			{
				PmdTargetType abstractType = JsonSerializer.Deserialize<PmdTargetType>(ref abstractReader, options)!;
				abstractTypes.Add(abstractType);
				abstractReader.Read();
			}
			return abstractTypes;
		}
	}
}
