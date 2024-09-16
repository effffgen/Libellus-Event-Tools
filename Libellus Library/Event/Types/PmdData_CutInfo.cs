using System.Text.Json.Serialization;

namespace LibellusLibrary.Event.Types
{
	internal class PmdData_CutInfo : PmdDataType, ITypeCreator, IVersionable
	{
		// TODO: Better account for versionability
		public PmdDataType CreateFromVersion(uint version, BinaryReader reader)
		{
			return version switch {
				4 or 9 => new PmdData_DDSCutInfo(reader),
				10 or 11 or 12 => new PmdData_P3CutInfo(reader),
				_ => throw new NotImplementedException(),
			};
		}

		public PmdDataType? CreateType(uint version)
		{
			return version switch
			{
				4 or 9 => new PmdData_DDSCutInfo(),
				10 or 11 or 12 => new PmdData_P3CutInfo(),
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

	public class PmdData_DDSCutInfo : PmdDataType
	{
		public int FirstFrame { get; set; }
		public int LastFrame { get; set; }
		public int TotalFrame { get; set; }

		[JsonConverter(typeof(JSON.ByteArrayToHexArray))]
		public byte[] Data { get; set; } = Array.Empty<byte>();

		public PmdData_DDSCutInfo()
		{
			return;
		}
		public PmdData_DDSCutInfo(BinaryReader reader)
		{
			FirstFrame = reader.ReadInt32();
			LastFrame = reader.ReadInt32();
			TotalFrame = reader.ReadInt32();
			Data = reader.ReadBytes(4);
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{
			writer.Write(FirstFrame);
			writer.Write(LastFrame);
			writer.Write(TotalFrame);
			writer.Write(Data);
		}
		internal override int GetCount() => 1;
		internal override int GetSize() => 0x10;
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

		public byte PC_CONTROL { get; set; } // Dunno what this does exactly; has no obvious effect?
		public byte COUNT_IDX { get; set; } // Dunno what this does exactly; has no obvious effect? DONTUSE (0) to 8 in editor

		public short FieldFBN { get; set; }
		public short FieldENV { get; set; }

		public short SPU_Sound { get; set; } // SPU SOUND/EVENT SPUFILE LOAD = TRUE/FALSE in editor

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public P3CutFlags Flags { get; set; }
		
		// Values correspond to bits of Flags field;
		// Bit 0b1000 appears to be forced on by game even if set to 0 by file?
		public enum P3CutFlags : int
		{
			LOAD_VOICE = 1,
			MESSAGE_GAMEOVER = 2,
			LDVOICE_MSGGAMEOVER = 3,
			BATTLE_BLUR = 4,
			LDVOICE_BTLBLUR = 5,
			MSGGAMEOVER_BTLBLUR = 6,
			LDVOICE_MSGGAMEOVER_BTLBLUR = 7,
			UNKNOWN08 = 8,
			LDVOICE_UNK08 = 9,
			MSGGAMEOVER_UNK08 = 10,
			LDVOICE_MSGGAMEOVER_UNK08 = 11,
			BTLBLUR_UNK08 = 12,
			LDVOICE_BTLBLUR_UNK08 = 13,
			MSGGAMEOVER_BTLBLUR_UNK08 = 14,
			LDVOICE_MSGGAMEOVER_BTLBLUR_UNK08 = 15
		}

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
			PC_CONTROL = reader.ReadByte();
			COUNT_IDX = reader.ReadByte();
			FieldFBN = reader.ReadInt16();
			FieldENV = reader.ReadInt16();
			SPU_Sound = reader.ReadInt16();
			Flags = (P3CutFlags)reader.ReadInt32();
		}

		internal override void SaveData(PmdBuilder builder, BinaryWriter writer)
		{ 
			writer.Write(FirstFrame);
			writer.Write(LastFrame);
			writer.Write(TotalFrame);
			writer.Write(Reserve1);
			writer.Write(FieldMajorID);
			writer.Write(FieldMinorID);
			writer.Write(PC_CONTROL);
			writer.Write(COUNT_IDX);
			writer.Write(FieldFBN);
			writer.Write(FieldENV);
			writer.Write(SPU_Sound);
			writer.Write((int)Flags);
		}
		internal override int GetCount() => 1;
		internal override int GetSize() => 0x24;
	}
}
