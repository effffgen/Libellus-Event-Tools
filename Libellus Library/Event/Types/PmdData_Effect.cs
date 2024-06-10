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
			foreach(var Effect in Effects)
			{
				Effect.EffectData = await File.ReadAllBytesAsync(directory + "\\" + Effect.FileName);
			}
		}

		public async Task SaveExternalFile(string directory)
		{
			foreach (var Effect in Effects)
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
			for(int i = 0; i < count; i++)
			{
				reader.FSeek(EffectStart + i * 0x10);
				Pmd_EffectDef Effect = new();
				Effect.ReadEffect(reader, typeFactory);
				Effects.Add(Effect);
			}
			for (int i = (int)count - 1; i >= 0; i--)
			{
				if (i == (int)count - 1)
				{
					reader.FSeek(EffectStart + (i * 0x10) + 4);
					Effects[i].ReadData(reader, typeFactory.GetEffectsLength(reader));
				}
				else
				{
					reader.FSeek(EffectStart + ((i + 1) * 0x10) + 4);
					int nextFileOffset = reader.ReadInt32();
					reader.FSeek(EffectStart + (i * 0x10) + 4);
					Effects[i].ReadData(reader, nextFileOffset);
				}
			}

			reader.BaseStream.Position = OriginalPos;
			return this;
		}

		internal override void SaveData(PmdBuilder builder,BinaryWriter writer)
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

		internal override int GetCount() => Effects.Count;
		internal override int GetSize() => 0x10;
	}
	internal class Pmd_EffectDef: IReferenceType
	{
		public string FileName { get; set; }
		public byte[] EffectData = Array.Empty<byte>();

		public uint UNK08 { get; set; }
		public uint UNK0C { get; set; }

		public int NameIndex;

		public void ReadEffect(BinaryReader reader, PmdTypeFactory typeFactory)
		{
			FileName = typeFactory.GetNameTable(reader)[reader.ReadInt32()]; // FileName == NameIndex
			reader.BaseStream.Position += 4; // skip offset for now
			UNK08 = reader.ReadUInt32();
			UNK0C = reader.ReadUInt32();
		}
		public void ReadData(BinaryReader reader, int nextOffset)
		{
			int offset = reader.ReadInt32();
			reader.FSeek(offset);
			EffectData = reader.ReadBytes(nextOffset - offset);
		}
		public void WriteEffect(BinaryWriter writer, long EffectDataOffset)
		{
			writer.Write(NameIndex);
			writer.Write((int)EffectDataOffset);
			writer.Write(UNK08);
			writer.Write(UNK0C);
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
