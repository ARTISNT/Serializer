using System.Globalization;
using System.Reflection;

namespace Serializer.Deserialization;

public class JsonDesirializer
{
    public static T Desiriailize<T>(string json)
    {
        var jsonReader = new JsonReader(json);
        object outputObject = Activator.CreateInstance<T>()
            ?? throw new NullReferenceException();

        PropertyInfo? property = null;
        while (jsonReader.Read())
        {
            if (jsonReader.TokenType == JsonToken.PropertyName)
            {
                property = typeof(T).GetProperty(jsonReader.Value)
                    ?? throw new FormatException($"Property {jsonReader.Value!} not found");

                continue;
            }

            if (property is null)
                continue;

            if (jsonReader.TokenType is JsonToken.StartObject or JsonToken.EndObject)
                continue;

            var convertedValue = ConvertValue(jsonReader, property.PropertyType);

            property.SetValue(outputObject, convertedValue);
        }

        return (T)outputObject;
    }

    private static object? ConvertValue(
        JsonReader reader,
        Type targetType)
    {
        return reader.TokenType switch
        {
            JsonToken.String =>
                Convert.ChangeType(reader.Value, targetType),

            JsonToken.Number =>
                Convert.ChangeType(
                    reader.Value,
                    targetType,
                    CultureInfo.InvariantCulture),

            JsonToken.True => true,
            JsonToken.False => false,

            JsonToken.Null => null,

            _ => throw new FormatException(
                $"Cannot convert token {reader.TokenType} to {targetType.Name}.")
        };
    }
}
