using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibellusLibrary.Event.Types.Bezier
{
	public class PmdBezierFactory
	{
		public List<PmdBezier> ReadBeziers(BinaryReader reader, uint typeTableCount)
		{
			List<PmdBezier> Beziers = new();

			for (int i = 0; i < typeTableCount; i++)
			{
				PmdBezier currentBezier = new PmdBezier();
				currentBezier.ReadBezier(reader);
				Beziers.Add(currentBezier);
			}

			return Beziers;
		}
	}
}
