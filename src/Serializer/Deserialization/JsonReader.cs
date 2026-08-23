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
                    _position++;

                    var value = new StringBuilder();

                    while (_position < _json.Length)
                    {
                        char current = _json[_position];
                        if (current == '"')
                        {
                            _position++;
                            break;
                        }

                        value.Append(current);
                        _position++;
                    }

                    Value = value.ToString();

                    if (IsNextCharacterColon())
                        TokenType = JsonToken.PropertyName;
                    else
                        TokenType = JsonToken.String;

                    return true;

                case >= (char)0x0030 and <= (char)0x0039:

                    var sb = new StringBuilder();

                    while (_position < _json.Length)
                    {
                        char current = _json[_position];
                        if (current is ',' or ']' or '}')
                            break;

                        sb.Append(current);
                        _position++;
                    }
                    Value = sb.ToString();

                    TokenType = JsonToken.Number;

                    return true;

                case 'n':
                    TokenType = JsonToken.Null;
                    Value = null;
                    _position += 4;
                    return true;

                case 't':
                    TokenType = JsonToken.True;
                    Value = null;
                    _position += 4;
                    return true;

                case 'f':
                    TokenType = JsonToken.False;
                    Value = null;
                    _position += 5;
                    return true;


                default:
                    return false;
            }
        }
        return false;
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
