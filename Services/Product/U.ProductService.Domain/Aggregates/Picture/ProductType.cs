
using U.ProductService.Domain.SeedWork;

namespace U.ProductService.Domain
{
	public class MimeType : Enumeration
	{
		public static MimeType Jpg = new MimeType(1, "Jpg");
		public static MimeType Mp4 = new MimeType(2, "Mp4");
		public static MimeType Avi = new MimeType(3, "Avi");
		public static MimeType Bitmap = new MimeType(3, "Bitmap");

		public MimeType(int id, string name)
			: base(id, name)
		{
		}
	}
}
