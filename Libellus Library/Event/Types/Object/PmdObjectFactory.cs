using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibellusLibrary.Event.Types.Object
{
	public class PmdObjectFactory
	{
		public List<PmdObject> ReadObjects(BinaryReader reader, uint typeTableCount)
		{
			// var typeIDs = ReadTypes(reader, typeTableCount);
			List<PmdObject> objects = new();
			for (int i = 0; i < typeTableCount; i++)
			{
				PmdObject currentObject = new PmdObject();
				currentObject.ReadObject(reader);
				objects.Add(currentObject);
			}
			/*foreach (var type in typeIDs)
			{
				PmdObject currentObject = new PmdObject();
				currentObject.ReadObject(reader);
				objects.Add(currentObject);
			}*/

			return objects;
		}

		/*private List<PmdObjectID> ReadTypes(BinaryReader reader, uint typeTableCount)
		{
			long originalpos = reader.BaseStream.Position;
			List<PmdObjectID> types = new List<PmdObjectID>();

			for (int i = 0; i < typeTableCount; i++)
			{
				types.Add((PmdObjectID)reader.ReadUInt16());
				reader.BaseStream.Position += 0x20;
			}
			reader.BaseStream.Position = originalpos;
			return types;
		}*/
	}
}
