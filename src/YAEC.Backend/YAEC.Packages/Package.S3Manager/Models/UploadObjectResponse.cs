namespace Package.S3Manager.Models;

public class UploadObjectResponse
{
    public string Key { get; set; } = null!;

    public string FileName { get; set; } = null!;
    
    public long ContentLength { get; set; }
}