using System.Text;

namespace LibellusLibrary.Utils
{
	public static class Text
	{
		public static Encoding GetStringEncoder()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			return Encoding.GetEncoding("437");
		}

		public static string ASCII8ToString(byte[] ASCIIData)
		{
			return GetStringEncoder().GetString(ASCIIData);
		}

		public static byte[] StringtoASCII8(string text)
		{
			return GetStringEncoder().GetBytes(text);
		}
	}
}
