using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LibellusLibrary.Event.Types.Bezier
{
	internal class PmdBezierReader : JsonConverter<List<PmdBezier>>
	{
		public override List<PmdBezier>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			// TODO: is the abstractType stuff even necessary?
			List<PmdBezier> Beziers = new();

			reader.Read();
			List<PmdBezier> abstractTypes = new();

			var abstractReader = reader;

			while (abstractReader.TokenType != JsonTokenType.EndArray)
			{
				PmdBezier abstractType = JsonSerializer.Deserialize<PmdBezier>(ref abstractReader, options)!;
				abstractTypes.Add(abstractType);
				abstractReader.Read();

			}

			foreach (PmdBezier abstractType in abstractTypes)
			{
				Type trueDataType = new PmdBezier().GetType();
				Beziers.Add((PmdBezier)JsonSerializer.Deserialize(ref reader, trueDataType, options)!);
				reader.Read();
			}
			//reader.Read();
			return Beziers;
		}

		public override void Write(Utf8JsonWriter writer, List<PmdBezier> value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			foreach (PmdBezier data in value)
			{
				writer.WriteRawValue(JsonSerializer.Serialize<object>(data, options));
			}
			writer.WriteEndArray();

		}
	}
}
