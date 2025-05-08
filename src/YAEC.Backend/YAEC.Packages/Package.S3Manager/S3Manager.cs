using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Package.S3Manager.Models;

namespace Package.S3Manager;

public interface IS3Manager
{
    Task MakeBucketAsync(CancellationToken ct = default);
    
    Task<UploadObjectResponse> UploadObjectAsync(UploadObjectRequest request, CancellationToken ct = default);
    
    Task<ReadStreamObjectResponse> ReadStreamObjectAsync(string key, CancellationToken ct = default);

    Task DeleteObjectAsync(string key, CancellationToken ct = default);
}

public class S3Manager : IS3Manager
{
    private readonly AmazonS3Client _client;

    private readonly S3Options _options;
    
    private readonly ILogger<S3Manager> _logger;
    
    public S3Manager(S3Options options, ILogger<S3Manager> logger)
    {
        _logger = logger;
        _options = options;
        _client = new AmazonS3Client(
            _options.AccessKey,
            _options.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = _options.ServiceUrl,
                ForcePathStyle = true
            });
    }
    
    public async Task MakeBucketAsync(CancellationToken ct = default)
    {
        var exists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_client, _options.BucketName);
        if (exists) return;
        await _client.PutBucketAsync(new PutBucketRequest
        {
            BucketName = _options.BucketName,
        }, ct);
    }

    public async Task<UploadObjectResponse> UploadObjectAsync(UploadObjectRequest request,
        CancellationToken ct = default)
    {
        var fileNameId = $"{Guid.NewGuid()}{Path.GetExtension(request.OriginalFileName)}";
        var key = $"{DateTime.UtcNow:yyyy/MM/dd}/{fileNameId}";
        try
        {
            await _client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = key,
                InputStream = request.Stream
            }, ct);
            return new UploadObjectResponse
            {
                Key = key,
                FileName = fileNameId,
            };
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return null!;
        }
    }

    public async Task<ReadStreamObjectResponse> ReadStreamObjectAsync(string key, CancellationToken ct = default)
    {
        try
        {
            var result = await _client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _options.BucketName,
                Key = key
            }, ct);
            return new ReadStreamObjectResponse
            {
                Stream = result.ResponseStream,
                ContentType = result.Headers.ContentType,
                ContentLength = result.Headers.ContentLength,
            };
        }
        catch (Exception ex)
        {
            HandleException(ex);
            return null!;
        }
    }

    public async Task DeleteObjectAsync(string key, CancellationToken ct = default)
    {
        try
        {
            await _client.DeleteObjectAsync(new DeleteObjectRequest
            {
                Key = key,
                BucketName = _options.BucketName
            }, ct);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private static void HandleException(Exception ex)
    {
        if (ex is not AmazonS3Exception aEx) throw ex;
        throw new S3Exceptions($"{aEx.ErrorCode} - {aEx.Message}");
    }
}