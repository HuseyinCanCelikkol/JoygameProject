using System.Text.Json;

namespace JoygameProject.Application.Extensions
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T model)
        {
            return JsonSerializer.Serialize(model);
        }
        public static T FromJson<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
