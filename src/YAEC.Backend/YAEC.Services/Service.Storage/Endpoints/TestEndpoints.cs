using Package.OpenApi.MinimalApi;
using Xabe.FFmpeg;

namespace Service.Storage.Endpoints;

public class TestEndpoints : IEndpoints
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/test")
            .WithTags("Test");

        group.MapPost("/ffmpeg/process-video", async 
            (IFormFile file, IWebHostEnvironment webHostEnvironment, CancellationToken cancellationToken, ILogger<TestEndpoints> logger) =>
            {
                if (file.Length == 0) return Results.Ok();
                var directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "Conversions");
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                var videoName = $"{Guid.NewGuid():N}-{file.FileName}";
                var fullPath = Path.Combine(directoryPath, videoName);
                await using (Stream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream, cancellationToken);
                }
                
                var mediaInfo = await FFmpeg.GetMediaInfo(fullPath, cancellationToken);
                var conversion = FFmpeg.Conversions.New();
                conversion.AddParameter($"-i \"{fullPath}\"");
                conversion.AddParameter("-hide_banner");
                conversion.AddParameter("-r 30");
                conversion.AddParameter("-crf 28");
                conversion.AddParameter("-c:a aac");
                conversion.AddParameter("-b:a 128k");
                conversion.AddParameter($"-hls_segment_filename \"{Path.Combine(directoryPath, $"ffmpeg-{videoName}_%03d.ts")}\"");
                conversion.AddParameter($"\"{Path.Combine(directoryPath, $"ffmpeg-{videoName}.m3u8")}\"");
                conversion.OnProgress += (_, args) =>
                {
                    var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);
                    logger.LogInformation("{Percent}%: {@Args}", percent, args);
                };
                await conversion.Start(cancellationToken);
                
                return Results.Ok(new
                {
                    FullPath = fullPath,
                    MediaInfo = mediaInfo
                });
            })
            .DisableAntiforgery()
            .MapToApiVersion(1);
    }
}