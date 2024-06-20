namespace LibellusLibrary.Event.Types
{
	internal class PmdData_CutInfo : PmdDataType, ITypeCreator, IVersionable
	{
		// TODO: Better account for versionability
		public PmdDataType CreateFromVersion(uint version, BinaryReader reader)
		{
			return version switch {
				10 => new PmdData_P3CutInfo(reader),
				12 => new PmdData_P3CutInfo(reader),
				_ => throw new NotImplementedException(),
			};
		}

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				10 => new PmdData_P3CutInfo(),
				12 => new PmdData_P3CutInfo(),
				_ => throw new NotImplementedException(),
			};
		}

		public PmdDataType? ReadType(BinaryReader reader, uint version, List<PmdTypeID> typeIDs, PmdTypeFactory typeFactory)
		{
			long OriginalPos = reader.BaseStream.Position;

			reader.BaseStream.Position = OriginalPos + 0xC;
			reader.BaseStream.Position = (long)reader.ReadUInt32();

			PmdDataType cut = CreateFromVersion(version, reader);
			reader.BaseStream.Position = OriginalPos;
			return cut;
		}
	}

	public class PmdData_P3CutInfo : PmdDataType
	{
		public int FirstFrame { get; set; }
		public int LastFrame { get; set; }
		public int TotalFrame { get; set; }

		// Normally Id remove this but im not sure if its actually not used
		public int Reserve1 { get; set; }

		public int FieldMajorID { get; set; }
		public int FieldMinorID { get; set; }

		public short Unknown1 { get; set; }

		public short FieldFBN { get; set; }
		public int FieldENV { get; set; }

		public int Flags { get; set; }

		public PmdData_P3CutInfo()
		{
			return;
		}
		public PmdData_P3CutInfo(BinaryReader reader)
		{
			FirstFrame = reader.ReadInt32();
			LastFrame = reader.ReadInt32();
			TotalFrame = reader.ReadInt32();
			Reserve1 = reader.ReadInt32();
			FieldMajorID = reader.ReadInt32();
			FieldMinorID = reader.ReadInt32();
			Unknown1 = reader.ReadInt16();
			FieldFBN = reader.ReadInt16();
			FieldENV = reader.ReadInt32();
			Flags = reader.ReadInt32();
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{ 
			writer.Write(FirstFrame);
			writer.Write(LastFrame);
			writer.Write(TotalFrame);
			writer.Write(Reserve1);
			writer.Write(FieldMajorID);
			writer.Write(FieldMinorID);
			writer.Write(Unknown1);
			writer.Write(FieldFBN);
			writer.Write(FieldENV);
			writer.Write(Flags);
		}
		internal override int GetCount() => 1;
		internal override int GetSize() => 0x24;
	}
}
