using System.Globalization;
using System.Text;

namespace Serializer;

public class JsonWriter
{
    private readonly StringBuilder _json = new();

    public string GetJson => _json.ToString();

    public void WriteStartOfObject()
    {
        _json.Append('{');
    }

    public void WriteEndOfObject()
    {
        _json.Append('}');
    }

    public void WritePropertyName(string propertyName)
    {
        WriteString(propertyName);
        _json.Append(':');
    }

    public void WriteBool(bool val)
    {
        _json.Append(val ? "true" : "false");
    }

    public void WriteString(string sourceString)
    {
        _json.Append('"');
        foreach (var c in sourceString)
        {
            switch (c)
            {
                case '"':
                    _json.Append("\\\"");
                    break;

                case '\\':
                    _json.Append("\\\\");
                    break;

                case '\b':
                    _json.Append("\\b");
                    break;

                case '\f':
                    _json.Append("\\f");
                    break;

                case '\n':
                    _json.Append("\\n");
                    break;

                case '\r':
                    _json.Append("\\r");
                    break;

                case '\t':
                    _json.Append("\\t");
                    break;

                default:
                    if (c < 0x20)
                    {
                        _json.Append("\\u");
                        _json.Append(((int)c).ToString("X4"));
                    }
                    else
                    {
                        _json.Append(c);
                    }
                    break;
            }

        }

        _json.Append('"');
    }

    public void WriteNumber(object value)
    {
        _json.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
    }

    public void WriteStartOfArray()
    {
        _json.Append('[');
    }

    public void WriteEndOfArray()
    {
        _json.Append(']');
    }

    public void WriteComma()
    {
        _json.Append(',');
    }

    public void WriteNull()
    {
        _json.Append("null");
    }
}
