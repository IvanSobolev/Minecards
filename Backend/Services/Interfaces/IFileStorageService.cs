using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string bucketName);
}