using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibellusLibrary.Event.Types.Object
{
	internal class PmdObjectReader : JsonConverter<List<PmdObject>>
	{
		public override List<PmdObject>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			// TODO: is the abstractType stuff even necessary?
			List<PmdObject> objects = new();

			reader.Read();
			List<PmdObject> abstractTypes = new();

			var abstractReader = reader;

			while (abstractReader.TokenType != JsonTokenType.EndArray)
			{
				PmdObject abstractType = JsonSerializer.Deserialize<PmdObject>(ref abstractReader, options)!;
				abstractTypes.Add(abstractType);
				abstractReader.Read();

			}

			foreach (PmdObject abstractType in abstractTypes)
			{
				Type trueDataType = new PmdObject().GetType();
				objects.Add((PmdObject)JsonSerializer.Deserialize(ref reader, trueDataType, options)!);
				reader.Read();
			}
			//reader.Read();
			return objects;
		}

		public override void Write(Utf8JsonWriter writer, List<PmdObject> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (PmdObject data in value)
			{
				writer.WriteRawValue(JsonSerializer.Serialize<object>(data, options));
			}
			writer.WriteEndArray();

		}
	}
}
