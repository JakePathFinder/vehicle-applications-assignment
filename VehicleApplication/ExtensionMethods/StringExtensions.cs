namespace VehicleApplication.ExtensionMethods;

public static class StringExtensions
{
    public static string Sanitize(this string name)
    {
        return string.IsNullOrEmpty(name) ? name : name.Trim().ToLowerInvariant();
    }
}