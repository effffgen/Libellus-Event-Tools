using System.Text;

namespace LibellusLibrary.Utils
{
	public static class Text
	{
		public static string ASCII8ToString(byte[] ASCIIData)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var e = Encoding.GetEncoding("437");
			return e.GetString(ASCIIData);
		}

		public static byte[] StringtoASCII8(string text)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var e = Encoding.GetEncoding("437");
			return e.GetBytes(text);
		}
	}
}
