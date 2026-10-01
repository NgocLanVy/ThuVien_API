using WebAPI_simple.Models.Domain;

namespace WebAPI_simple.Repositories
{
    public interface IImageRepository
    {
        Image Upload(Image image);
        List<Image> GetAllInfoImages();
        (byte[], string, string) DownloadFile(int Id);
    }
}