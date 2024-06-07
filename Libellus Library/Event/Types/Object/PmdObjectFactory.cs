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
			List<PmdObject> objects = new();

			for (int i = 0; i < typeTableCount; i++)
			{
				PmdObject currentObject = new PmdObject();
				currentObject.ReadObject(reader);
				objects.Add(currentObject);
			}

			return objects;
		}
	}
}
