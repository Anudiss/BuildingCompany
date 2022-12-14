using System.Linq;

namespace BuildingCompany.Connection
{
    public partial class SystemImage
    {
        public static byte[] GetImageByName(string name) =>
            DatabaseContext.Entities.SystemImage.Local.FirstOrDefault(image => image.Name == name)?.Data;
    }
}
