namespace Package.S3Manager.Models;

public class UploadObjectRequest
{
    public string OriginalFileName { get; set; } = null!;
    
    public Stream Stream { get; set; } = null!;
}