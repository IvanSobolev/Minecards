using Backend.Services.Interfaces;
using Minio;
using Minio.DataModel.Args;

namespace Backend.Services.Implementation;

public class MinioFileStorageService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _internalEndpoint;
    private readonly string _publicEndpoint;

    public MinioFileStorageService(IConfiguration configuration)
    {
        _internalEndpoint = configuration["Minio:Endpoint"] 
                            ?? throw new ArgumentNullException(nameof(configuration), "Minio:Endpoint is not configured.");
            
        _publicEndpoint = configuration["Minio:PublicEndpoint"] 
                          ?? throw new ArgumentNullException(nameof(configuration), "Minio:PublicEndpoint is not configured.");

        var accessKey = configuration["Minio:AccessKey"];
        var secretKey = configuration["Minio:SecretKey"];

        _minioClient = new MinioClient()
            .WithEndpoint(_internalEndpoint) 
            .WithCredentials(accessKey, secretKey)
            .Build();
    }

    public async Task<string> UploadFileAsync(IFormFile file, string bucketName)
    {
        var objectName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
        bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs);
        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs);
            var policy = $$"""
                           {
                               "Version": "2012-10-17",
                               "Statement": [
                                   {
                                       "Effect": "Allow",
                                       "Principal": { "AWS": ["*"] },
                                       "Action": ["s3:GetObject"],
                                       "Resource": ["arn:aws:s3:::{{bucketName}}/*"]
                                   }
                               ]
                           }
                           """;
            var setPolicyArgs = new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policy);
            await _minioClient.SetPolicyAsync(setPolicyArgs);
        }

        await using var stream = file.OpenReadStream();
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(putObjectArgs);

        return $"{_publicEndpoint}/{bucketName}/{objectName}";
    }
}