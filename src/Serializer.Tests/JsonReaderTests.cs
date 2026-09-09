using Serializer.Deserialization;

namespace Serializer.Tests;

public class JsonReaderTests
{
    [Theory]
    [InlineData("{", JsonToken.StartObject)]
    [InlineData("}", JsonToken.EndObject)]
    [InlineData("[", JsonToken.StartArray)]
    [InlineData("]", JsonToken.EndArray)]
    [InlineData("\"hello\"", JsonToken.String)]
    [InlineData("12", JsonToken.Number)]
    [InlineData("true", JsonToken.True)]
    [InlineData("false", JsonToken.False)]
    [InlineData("null", JsonToken.Null)]
    public void Read_ShouldGetCorrectToken(
        string json,
        JsonToken expectedToken)
    {
        var jsonReader = new JsonReader(json);

        jsonReader.Read();

        Assert.Equal(expectedToken, jsonReader.TokenType);
    }

    [Theory]
    [InlineData("12e")]
    [InlineData("01")]
    [InlineData("-")]
    [InlineData("1,23")]
    public void ReadJson_ShouldReturnError(string json)
    {
        var jsonReader = new JsonReader(json);

        Assert.Throws<FormatException>(() => jsonReader.Read());
    }
}
