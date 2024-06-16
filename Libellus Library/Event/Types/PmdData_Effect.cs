using System.Text.Json.Serialization;
using LibellusLibrary.Utils;
using LibellusLibrary.Utils.IO;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_Effect : PmdDataType, ITypeCreator, IExternalFile, IReferenceType
	{
		[JsonInclude]
		public List<Pmd_EffectDef> Effects { get; set; }

		public PmdDataType? CreateType(uint version)
		{
			return new PmdData_Effect();
		}

		public async Task LoadExternalFile(string directory)
		{
			foreach(Pmd_EffectDef Effect in Effects)
			{
				Effect.EffectData = await File.ReadAllBytesAsync(Path.Combine(directory, Effect.FileName));
			}
		}

		public async Task SaveExternalFile(string directory)
		{
			foreach (Pmd_EffectDef Effect in Effects)
			{
				await File.WriteAllBytesAsync(Path.Combine(directory, Effect.FileName), Effect.EffectData);
			}
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			Effects ??= new();
			long OriginalPos = reader.BaseStream.Position;

			reader.BaseStream.Position = OriginalPos + 0x8;
			uint count = reader.ReadUInt32();

			reader.BaseStream.Position = OriginalPos + 0xC;
			reader.BaseStream.Position = (long)reader.ReadUInt32();
			long EffectStart = reader.FTell();
			int effSizeRemainder = typeFactory.GetEffectsLength(reader);
			for(int i = 0; i < count; i++)
			{
				// Initialize Pmd_EffectDef object
				reader.FSeek(EffectStart + i * 0x10);
				Pmd_EffectDef Effect = new();
				Effect.FileName = typeFactory.GetNameTable(reader)[reader.ReadInt32()];
				int offset = reader.ReadInt32();
				Effect.Field08 = reader.ReadUInt32();
				Effect.Field0C = reader.ReadUInt32();
				// Calculate Effect.EffectData size
				int eplSize = effSizeRemainder;
				if (i < (int)count - 1)
				{
					reader.FSeek(EffectStart + ((i + 1) * 0x10) + 4);
					eplSize = reader.ReadInt32() - offset;
					effSizeRemainder -= eplSize;
				}
				reader.FSeek(offset);
				Effect.EffectData = reader.ReadBytes(eplSize);
				Effects.Add(Effect);
			}

			reader.BaseStream.Position = OriginalPos;
			return this;
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			long start = writer.FTell();
			long dataOffset = writer.FTell() + 0x10 * Effects.Count;

			for(int i = 0; i < Effects.Count; i++)
			{
				writer.FSeek(start + i * 0x10);
				Effects[i].WriteEffect(writer, dataOffset);
				dataOffset += Effects[i].EffectData.Length;
			}
		}

		public void SetReferences(PmdBuilder pmdBuilder)
		{
			foreach(Pmd_EffectDef Effect in Effects)
			{
				Effect.SetReferences(pmdBuilder);
			}
		}
		
		// Get total size of all EPL files
		public int GetDataSize()
		{
			int size = 0;
			foreach (Pmd_EffectDef Effect in Effects)
			{
				size += Effect.EffectData.Length;
			}
			return size;
		}

		internal override int GetCount() => Effects.Count;
		internal override int GetSize() => 0x10;
	}

	internal class Pmd_EffectDef : IReferenceType
	{
		public string FileName { get; set; }
		public byte[] EffectData = Array.Empty<byte>();

		public uint Field08 { get; set; }
		public uint Field0C { get; set; }

		public int NameIndex;

		public void WriteEffect(BinaryWriter writer, long EffectDataOffset)
		{
			writer.Write(NameIndex);
			writer.Write((int)EffectDataOffset);
			writer.Write(Field08);
			writer.Write(Field0C);
			writer.FSeek(EffectDataOffset);
			writer.Write(EffectData);
		}

		public void SetReferences(PmdBuilder pmdBuilder)
		{
			byte[] temp = Text.StringtoASCII8(FileName);
			System.Array.Resize(ref temp, 32);
			NameIndex = pmdBuilder.AddReference(PmdTypeID.Name, temp);
		}
	}
}
