using System.Text.Json;
using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types.Object
{
	internal class PmdObjectReader : JsonConverter<List<PmdObjectType>>
	{
		public override List<PmdObjectType>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			List<PmdObjectType> objects = new();

			reader.Read();
			List<PmdObjectType> abstractTypes = new();

			Utf8JsonReader abstractReader = reader;

			while (abstractReader.TokenType != JsonTokenType.EndArray)
			{
				PmdObjectType abstractType = JsonSerializer.Deserialize<PmdObjectType>(ref abstractReader, options)!;
				abstractTypes.Add(abstractType);
				abstractReader.Read();
			}

			foreach (PmdObjectType abstractType in abstractTypes)
			{
				Type trueDataType = PmdObjectFactory.GetObjectType(abstractType.ObjectID).GetType();
				objects.Add((PmdObjectType)JsonSerializer.Deserialize(ref reader, trueDataType, options)!);
				reader.Read();
			}
			return objects;
		}

		public override void Write(Utf8JsonWriter writer, List<PmdObjectType> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (PmdObjectType data in value)
			{
				writer.WriteRawValue(JsonSerializer.Serialize<object>(data, options));
			}
			writer.WriteEndArray();
		}
	}
}
