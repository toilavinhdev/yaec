using System.Text.Json;
using System.Text.Json.Serialization;

namespace Package.Shared.Extensions;

public static class JsonExtensions
{
    private static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public static string ToJson<T>(this T input)
    {
        return JsonSerializer.Serialize(input, DefaultOptions);
    } 

    public static T ToObject<T>(this string input) where T : class, new()
    {
        return JsonSerializer.Deserialize<T>(input, DefaultOptions) ?? new T();
    }
}