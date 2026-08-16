using System.Collections;
using System.Globalization;

namespace Serializer;

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

    public static string Serialize(object? value)
    {
        var jsonWriter = new JsonWriter();

        Serialize(value, jsonWriter);

        return jsonWriter.GetJson.ToString();
    }

    private static void Serialize(object value, JsonWriter jsonWriter)
    {

        if (value is null)
        {
            jsonWriter.WriteNull();
            return;
        }

        if (value is Enum enumVal)
        {
            jsonWriter.WriteEnum(enumVal);
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
            SerializeNumber(value, jsonWriter);
            return;
        }

        if (value is IEnumerable enumerable && value is not string)
        {
            SerializeCollection(enumerable, jsonWriter);
            return;
        }

        SerializeObject(value, jsonWriter);
    }

    private static void SerializeNumber(object value, JsonWriter jsonWriter)
    {
        if (value is double d)
        {
            if (double.IsNaN(d) || double.IsInfinity(d))
            {
                jsonWriter.WriteNull();
                return;
            }
        }

        if (value is float f)
        {
            if (float.IsNaN(f) || float.IsInfinity(f))
            {
                jsonWriter.WriteNull();
                return;
            }
        }
        jsonWriter.WriteNumber(Convert.ToString(value, CultureInfo.InvariantCulture));
    }

    private static void SerializeCollection(IEnumerable collection, JsonWriter jsonWriter)
    {
        bool isFirst = true;

        jsonWriter.WriteStartOfArray();
        foreach (var element in collection)
        {
            if (!isFirst)
                jsonWriter.WriteComma();

            isFirst = false;

            Serialize(element, jsonWriter);
        }
        jsonWriter.WriteEndOfArray();
    }

    private static void SerializeObject(object value, JsonWriter jsonWriter)
    {
        bool isFirst = true;
        jsonWriter.WriteStartOfObject();

        var properties = value.GetType().GetProperties();
        foreach (var property in properties)
        {
            if (!isFirst)
                jsonWriter.WriteComma();

            isFirst = false;

            jsonWriter.WritePropertyName(property.Name);
            Serialize(property.GetValue(value), jsonWriter);
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
}
