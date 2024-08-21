namespace LibellusLibrary.Event.Types
{
	public class PmdTypeFactory
	{
		private List<PmdDataType> _types = new();
		private List<string> _names = new();
		private int _effectsLength = -1;
		public List<PmdTypeID> typeIDs = new();

		public List<PmdDataType> ReadDataTypes(BinaryReader reader, uint typeTableCount, uint version)
		{
			_types = new List<PmdDataType>();
			typeIDs = ReadTypes(reader, typeTableCount);
			foreach(PmdTypeID type in typeIDs)
			{
				ITypeCreator typecreator = GetTypeCreator(type);

				PmdDataType? dataType = typecreator.ReadType(reader, version, typeIDs, this);
				if(dataType != null) {
					dataType.Type = type;
					_types.Add(dataType);
				}
				reader.BaseStream.Position += 0x10;
			}

			return _types;
		}

		public static ITypeCreator GetTypeCreator(PmdTypeID Type) => Type switch
		{
			PmdTypeID.CutInfo => new PmdData_CutInfo(),
			PmdTypeID.Stage => new PmdData_Stage(),
			PmdTypeID.Unit => new PmdData_Unit(),
			PmdTypeID.FrameTable => new PmdData_FrameTable(),
			PmdTypeID.Message => new PmdData_Message(),
			PmdTypeID.Effect => new PmdData_Effect(),
			PmdTypeID.SLight => new PmdData_SLight(),
			PmdTypeID.ObjectTable => new PmdData_ObjectTable(),
			PmdTypeID.BezierTable => new PmdData_BezierTable(),
			_ => new UnkType()
		};

		public static bool IsSerialized(PmdTypeID type) => type switch
		{
			PmdTypeID.EffectData => false,
			PmdTypeID.UnitData => false,
			PmdTypeID.Name => false,
			_ => true
		};

		private List<PmdTypeID> ReadTypes(BinaryReader reader, uint typeTableCount)
		{
			long originalpos = reader.BaseStream.Position;
			List<PmdTypeID> types = new();

			for(int i = 0; i < typeTableCount; i++)
			{
				types.Add((PmdTypeID)reader.ReadUInt32());
				reader.BaseStream.Position += 0x0C;
			}
			reader.BaseStream.Position = originalpos;
			return types;
		}

		public List<string> GetNameTable(BinaryReader reader)
		{
			// Return list early if we already have it, else create and return it
			if (_names.Any())
			{
				return _names;
			}
			
			long originalpos = reader.BaseStream.Position;
			for(int i = 0; i < typeIDs.Count; i++)
			{
				if (typeIDs[i] != PmdTypeID.Name)
				{
					continue;
				}
				reader.BaseStream.Position = 0x20 + (0x10 * i) + 0x8;
				uint nameCount = reader.ReadUInt32();
				reader.BaseStream.Position = reader.ReadUInt32();

				for (int j = 0; j < nameCount; j++)
				{
					string data = new(reader.ReadChars(32));
					_names.Add(data.Replace("\0", string.Empty));
				}
			}
			reader.BaseStream.Position = originalpos;
			return _names;
		}
		
		public int GetEffectsLength(BinaryReader reader)
		{
			// If we already know the length, return it
			if (_effectsLength != -1)
			{
				return _effectsLength;
			}
			long originalpos = reader.BaseStream.Position;
			int effDataID = typeIDs.IndexOf(PmdTypeID.EffectData);
			if (effDataID == -1)
			{
				return effDataID; // return -1
			}
			reader.BaseStream.Position = 0x20 + (0x10 * effDataID) + 0x4;
			// We only get the size property since count is always `1` for EffectData table
			_effectsLength = reader.ReadInt32();
			reader.BaseStream.Position = originalpos;
			return _effectsLength;
		}
	}
}
