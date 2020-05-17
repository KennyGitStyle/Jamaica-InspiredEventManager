using Jam_Inspired_Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Jam_Inspired_Application.Interfaces
{
    public interface IPhotoAccessor
    {
        PhotoUploadResult AddPhoto(IFormFile file);
        string DeletePhoto(string publicId);
    }
}