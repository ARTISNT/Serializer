using Serializer.Serialization;

namespace Serializer.Tests;

public class JsonWriterTests
{
    [Theory]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    public void WriteBool_ShouldWriteCorrectValue(bool input, string expected)
    {
        var writer = new JsonWriter();

        writer.WriteBool(input);

        Assert.Equal(expected, writer.GetJson);
    }

    [Theory]
    [InlineData("hello", "\"hello\"")]
    [InlineData("", "\"\"")]
    [InlineData("hello12", "\"hello12\"")]
    public void WriteString_ShouldWriteJsonString(string input, string expected)
    {
        var writer = new JsonWriter();

        writer.WriteString(input);

        Assert.Equal(expected, writer.GetJson);
    }

    [Theory]
    [InlineData(1, "1")]
    [InlineData(-1, "-1")]
    [InlineData(123, "123")]
    [InlineData(1.5, "1.5")]
    [InlineData(-12.5, "-12.5")]
    public void WriteNumber_ShouldWriteNumber(object input, string expected)
    {
        var writer = new JsonWriter();

        writer.WriteNumber(input);

        Assert.Equal(expected, writer.GetJson);
    }

    [Theory]
    [InlineData("hello\nworld", "\"hello\\nworld\"")]
    [InlineData("hello\"world", "\"hello\\\"world\"")]
    [InlineData("hello\\world", "\"hello\\\\world\"")]
    public void WriteString_ShouldEscapeSpecialCharacters(
    string input,
    string expected)
    {
        var writer = new JsonWriter();

        writer.WriteString(input);

        Assert.Equal(expected, writer.GetJson);
    }
}
