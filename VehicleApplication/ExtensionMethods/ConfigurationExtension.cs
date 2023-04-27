namespace VehicleApplication.ExtensionMethods;

public static class ConfigurationExtension
{
    public static T VerifyGet<T>(this IConfiguration cfg, string key)
    {
        var value = cfg.GetValue<T>(key);
        if (value == null || (typeof(T)==typeof(string) && string.IsNullOrEmpty(key)))
        {
            throw new ArgumentException($"Missing configuration value: {key}");
        }

        return value;
    }
    
    public static T VerifyGetEnumerable<T>(this IConfiguration cfg, string key)
    {
        var value = cfg.GetSection(key).Get<T>();
        if (value == null)
        {
            throw new ArgumentException($"Unable to get configuration value: {key}");
        }

        return value;
    }
}