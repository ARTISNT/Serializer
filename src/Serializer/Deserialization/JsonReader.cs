using System.Text;

namespace Serializer.Deserialization;

public class JsonReader(string json)
{
    private readonly string _json = json;
    private int _position;

    public JsonToken TokenType { get; private set; }
    public string? Value { get; private set; }

    public bool Read()
    {
        Value = null;
        while (_position < _json.Length)
        {
            SkipWhitespaceAndSeparators();

            if (_position >= _json.Length)
                return false;

            switch (_json[_position])
            {
                case '{':
                    TokenType = JsonToken.StartObject;
                    _position++;
                    return true;

                case '}':
                    TokenType = JsonToken.EndObject;
                    _position++;
                    return true;

                case '[':
                    TokenType = JsonToken.StartArray;
                    _position++;
                    return true;

                case ']':
                    TokenType = JsonToken.EndArray;
                    _position++;
                    return true;

                case '"':
                    TokenType = JsonToken.String;

                    _position++;
                    var value = ReadString();

                    if (value is null)
                        return false;

                    Value = value;

                    if (IsNextCharacterColon())
                        TokenType = JsonToken.PropertyName;

                    return true;

                case >= '0' and <= '9':
                case '-':
                    Value = ReadNumber();
                    return true;

                case 'n':
                    if (!Match("null"))
                        return false;

                    TokenType = JsonToken.Null;
                    return true;

                case 't':
                    if (!Match("true"))
                        return false;

                    TokenType = JsonToken.True;
                    return true;

                case 'f':
                    if (!Match("false"))
                        return false;

                    TokenType = JsonToken.False;
                    return true;

                default:
                    return false;
            }
        }
        return false;
    }

    private string ReadNumber()
    {
        var start = _position;

        if (_json[_position] == '-')
        {
            _position++;

            if (_position >= _json.Length)
                throw new FormatException("Invalid number.");
        }

        if (_json[_position] == '0')
        {
            _position++;

            if (_position < _json.Length && char.IsDigit(_json[_position]))
                throw new FormatException("Leading zero is not allowed.");
        }
        else if (_json[_position] >= '1' && _json[_position] <= '9')
        {
            while (_position < _json.Length &&
                   char.IsDigit(_json[_position]))
            {
                _position++;
            }
        }
        else
        {
            throw new FormatException("Invalid number.");
        }

        if (_position < _json.Length && _json[_position] == '.')
        {
            _position++;

            if (_position >= _json.Length ||
                !char.IsDigit(_json[_position]))
            {
                throw new FormatException("Invalid fraction.");
            }

            while (_position < _json.Length &&
                   char.IsDigit(_json[_position]))
            {
                _position++;
            }
        }

        if (_position < _json.Length &&
            (_json[_position] == 'e' || _json[_position] == 'E'))
        {
            _position++;

            if (_position < _json.Length &&
                (_json[_position] == '+' || _json[_position] == '-'))
            {
                _position++;
            }

            if (_position >= _json.Length ||
                !char.IsDigit(_json[_position]))
            {
                throw new FormatException("Invalid exponent.");
            }

            while (_position < _json.Length &&
                   char.IsDigit(_json[_position]))
            {
                _position++;
            }
        }

        TokenType = JsonToken.Number;

        return _json[start.._position];
    }

    private bool Match(string value)
    {
        if (_json.AsSpan(_position).StartsWith(value))
        {
            _position += value.Length;
            return true;
        }

        return false;
    }

    private string? ReadString()
    {
        var value = new StringBuilder();

        while (_position < _json.Length)
        {
            char current = _json[_position++];

            if (current == '"')
                return value.ToString();

            if (current != '\\')
            {
                if (current < 0x20)
                    return null;

                value.Append(current);
                continue;
            }

            if (_position >= _json.Length)
                return null;

            char escaped = _json[_position++];

            if (escaped == 'u')
            {
                if (!TryReadUnicodeEscape(value))
                    return null;

                continue;
            }

            char? toAppend = escaped switch
            {
                '"' => '"',
                '\\' => '\\',
                '/' => '/',
                'b' => '\b',
                'f' => '\f',
                'n' => '\n',
                'r' => '\r',
                't' => '\t',
                _ => null
            };

            if (toAppend is null)
                return null;

            value.Append(toAppend.Value);
        }

        return null;
    }

    private bool TryReadUnicodeEscape(StringBuilder value)
    {
        if (_position + 4 > _json.Length)
            return false;

        int unicodeValue = 0;

        for (int i = 0; i < 4; i++)
        {
            char c = _json[_position++];

            int digit;

            if (c >= '0' && c <= '9')
                digit = c - '0';
            else if (c >= 'a' && c <= 'f')
                digit = c - 'a' + 10;
            else if (c >= 'A' && c <= 'F')
                digit = c - 'A' + 10;
            else
                return false;

            unicodeValue = (unicodeValue << 4) | digit;
        }

        value.Append((char)unicodeValue);

        return true;
    }

    private void SkipWhitespaceAndSeparators()
    {
        while (_position < _json.Length)
        {
            char c = _json[_position];
            if (char.IsWhiteSpace(c) || c == ',' || c == ':')
            {
                _position++;
            }
            else
            {
                break;
            }
        }
    }

    private bool IsNextCharacterColon()
    {
        int tempPos = _position;
        while (tempPos < _json.Length)
        {
            if (!char.IsWhiteSpace(_json[tempPos]))
            {

                return _json[tempPos] == ':';
            }
            tempPos++;
        }
        return false;
    }
}
