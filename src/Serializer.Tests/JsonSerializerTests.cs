using Serializer.Serialization;

namespace Serializer.Tests;

public class JsonSerializerTests
{
    [Theory]
    [InlineData(12, "12")]
    [InlineData(1.2, "1.2")]
    [InlineData(1.2f, "1.2")]
    [InlineData("hello", "\"hello\"")]
    [InlineData("hello😀", "\"hello\uD83D\uDE00\"")]
    public void SerializeValue_ShouldReturnExpectedJson(object input, string expectedJson)
    {
        var serializedValue = JsonSerializer.Serialize(input);
        Assert.Equal(expectedJson, serializedValue);
    }

    [Fact]
    public void SerializedObject_ShouldRerurnExpectedJson()
    {
        var testClass = new TestClass() { Name = "Alex", Age = 12 };
        var serializedValue = JsonSerializer.Serialize(testClass);

        var expectedJson = """{"Name":"Alex","Age":12}""";

        Assert.Equal(expectedJson, serializedValue);
    }

    [Fact]
    public void SerializeObjectsCollection_ShouldReturnExpectedJson()
    {
        var testArray = new TestClass[]
        {
            new ()
            {
                Age = 13,
                Name = "Alex"
            },
            new ()
            {
                Age = 15,
                Name = "Sasha"
            }
        };

        var serializedValue = JsonSerializer.Serialize(testArray);
        var expectedJson = """[{"Name":"Alex","Age":13},{"Name":"Sasha","Age":15}]""";

        Assert.Equal(expectedJson, serializedValue);
    }
}
