using System.Globalization;
using System.Reflection;

namespace Serializer.Deserialization;

public class JsonDesirializer
{
    public static T? Desiriailize<T>(string json)
    {
        var jsonReader = new JsonReader(json);

        jsonReader.Read();

        return (T)DeserializeValue(typeof(T), jsonReader);
    }

    public static object? Desiriailize(Type targetType, string json)
    {
        var jsonReader = new JsonReader(json);

        jsonReader.Read();

        return DeserializeValue(targetType, jsonReader);
    }

    private static object? DesiriailizeObject(JsonReader reader, Type targetType)
    {
        object outputObject = Activator.CreateInstance(targetType)
            ?? throw new NullReferenceException();

        PropertyInfo? property = null;
        object? convertedValue;

        while (reader.Read())
        {
            if (reader.TokenType is JsonToken.EndObject)
                return outputObject;

            if (reader.TokenType is JsonToken.PropertyName)
            {
                property = targetType.GetProperty(reader.Value!)
                    ?? throw new FormatException($"Property {reader.Value!} not found");

                continue;
            }

            if (property is null)
                continue;

            convertedValue = DeserializeValue(property.PropertyType, reader);
            property.SetValue(outputObject, convertedValue);
        }

        return outputObject;
    }

    private static object? DesirializeCollection(JsonReader reader, Type targetType)
    {
        Type elementType = targetType.IsArray
            ? targetType.GetElementType()
            : targetType.GetGenericArguments()[0];

        Type listType = typeof(List<>).MakeGenericType(elementType);

        var list = Activator.CreateInstance(listType);

        var methodAdd = listType.GetMethod("Add");

        while (reader.Read())
        {
            if (reader.TokenType is JsonToken.EndArray)
                break;

            methodAdd!.Invoke(list, [DeserializeValue(elementType, reader)]);
        }

        if (targetType.IsArray)
        {
            return listType.GetMethod("ToArray")!.Invoke(list, null)!;
        }

        return list;
    }

    private static object? ConvertValue(
        JsonReader reader,
        Type targetType)
    {
        if (targetType.IsEnum)
        {
            return reader.TokenType switch
            {
                JsonToken.String => Enum.Parse(
                    targetType, reader.Value!, true),

                JsonToken.Number => Enum.ToObject(targetType, Convert.ToInt32(reader.Value,
                    CultureInfo.InvariantCulture)),

                _ => throw new FormatException(
                    $"Cannot convert token {reader.TokenType} to {targetType.Name}.")
            };
        }

        return reader.TokenType switch
        {
            JsonToken.String =>
                Convert.ChangeType(reader.Value, targetType),

            JsonToken.Number =>
                Convert.ChangeType(
                    reader.Value,
                    targetType,
                    CultureInfo.InvariantCulture),

            JsonToken.True or JsonToken.False
                when targetType == typeof(bool)
                    => reader.TokenType == JsonToken.True,

            JsonToken.Null when
                !targetType.IsValueType ||
                Nullable.GetUnderlyingType(targetType) != null
                    => null,

            _ => throw new FormatException(
                $"Cannot convert token {reader.TokenType} to {targetType.Name}.")
        };
    }

    private static object? DeserializeValue(Type targetType, JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonToken.StartObject =>
                DesiriailizeObject(reader, targetType),

            JsonToken.StartArray =>
                DesirializeCollection(reader, targetType),

            JsonToken.String or
                  JsonToken.Number or
                  JsonToken.True or
                  JsonToken.False or
                  JsonToken.Null =>
                      ConvertValue(reader, targetType),

            _ => throw new FormatException(
                $"Unexpected token {reader.TokenType}.")
        };
    }


}
