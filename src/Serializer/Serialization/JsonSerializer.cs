using System.Collections;

namespace Serializer.Serialization;

public class JsonSerializer
{
    private static readonly HashSet<Type> NumericTypes =
    [
        typeof(byte), typeof(sbyte),
        typeof(short), typeof(ushort),
        typeof(int), typeof(uint),
        typeof(long), typeof(ulong),
        typeof(float), typeof(double), typeof(decimal),
        typeof(Int128), typeof(UInt128), typeof(Half)
    ];

    public static string Serialize(object? value,
                                   JsonSerializerOptions? jsonSerializerOption = null)
    {
        jsonSerializerOption ??= new JsonSerializerOptions();

        var jsonWriter = new JsonWriter();

        Serialize(value, jsonWriter, jsonSerializerOption);

        return jsonWriter.GetJson;
    }

    private static void Serialize(object value,
                                  JsonWriter jsonWriter,
                                  JsonSerializerOptions jsonSerializerOption)
    {
        if (value is null)
        {
            jsonWriter.WriteNull();
            return;
        }

        if (value is Enum enumVal)
        {
            SerializeEnum(enumVal, jsonWriter, jsonSerializerOption);
            return;
        }

        if (value is bool logicalVal)
        {
            jsonWriter.WriteBool(logicalVal);
            return;
        }

        if (value is string str)
        {
            jsonWriter.WriteString(str);
            return;
        }

        if (IsNumeric(value))
        {
            SerializeNumber(value, jsonWriter, jsonSerializerOption);
            return;
        }

        if (value is IEnumerable enumerable && value is not string)
        {
            SerializeCollection(enumerable, jsonWriter, jsonSerializerOption);
            return;
        }

        SerializeObject(value, jsonWriter, jsonSerializerOption);
    }

    private static void SerializeNumber(object value,
                                        JsonWriter jsonWriter,
                                        JsonSerializerOptions jsonSerializerOption)
    {
        if (value is double d)
        {
            if (double.IsNaN(d) || double.IsInfinity(d))
            {
                if (jsonSerializerOption.ThrowOnInvalidNumber)
                    throw new ArgumentException(
                        "NaN and Infinity are not valid JSON numbers.",
                        nameof(value));

                jsonWriter.WriteNull();
                return;
            }
        }

        if (value is float f)
        {
            if (float.IsNaN(f) || float.IsInfinity(f))
            {
                if (jsonSerializerOption.ThrowOnInvalidNumber)
                    throw new ArgumentException(
                        "NaN and Infinity are not valid JSON numbers.",
                        nameof(value));

                jsonWriter.WriteNull();
                return;
            }
        }
        jsonWriter.WriteNumber(value);
    }

    private static void SerializeCollection(IEnumerable collection,
                                            JsonWriter jsonWriter,
                                            JsonSerializerOptions options)
    {
        bool isFirst = true;

        jsonWriter.WriteStartOfArray();
        foreach (var element in collection)
        {
            if (!isFirst)
                jsonWriter.WriteComma();

            isFirst = false;

            Serialize(element, jsonWriter, options);
        }
        jsonWriter.WriteEndOfArray();
    }

    private static void SerializeObject(object value,
                                        JsonWriter jsonWriter,
                                        JsonSerializerOptions jsonSerializerOptions)
    {
        bool isFirst = true;
        jsonWriter.WriteStartOfObject();

        var properties = value.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(value);

            if (propertyValue is null && jsonSerializerOptions.IgnoreNullProperties)
                continue;

            if (!isFirst)
                jsonWriter.WriteComma();

            isFirst = false;

            if(value == propertyValue)
                throw new ArgumentException("Recurring call", nameof(value));

            jsonWriter.WritePropertyName(property.Name);
            Serialize(propertyValue, jsonWriter, jsonSerializerOptions);
        }
        jsonWriter.WriteEndOfObject();
    }

    private static bool IsNumeric(object obj)
    {
        if (obj is null)
            return false;

        Type type = Nullable.GetUnderlyingType(obj.GetType())
                    ?? obj.GetType();

        return NumericTypes.Contains(type);
    }

    private static void SerializeEnum(Enum val,
                                      JsonWriter jsonWriter,
                                      JsonSerializerOptions jsonSerializerOption)
    {
        if (jsonSerializerOption.SerializeEnumAsString)
        {
            jsonWriter.WriteString(val.ToString());
            return;
        }
        jsonWriter.WriteNumber(Convert.ToInt32(val));
    }
}
