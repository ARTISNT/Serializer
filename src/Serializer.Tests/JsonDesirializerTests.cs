using Serializer.Deserialization;
using Serializer.Serialization;

namespace Serializer.Tests;

public class JsonDesirializerTests
{
    [Theory]
    [InlineData("13", 13)]
    [InlineData("1.3", 1.3)]
    [InlineData("\"hello\"", "hello")]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void DeserializeValue_ShouldReturnExpectedValue(
        string json,
        object expected)
    {
        var result = JsonDesirializer.Desiriailize(
            expected.GetType(),
            json);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void DesirializeObjectFromJson_ShouldReturnExpectedDesirilizedType()
    {
        var testObject = new TestClass
        {
            Age = 13,
            Name = "Alex"
        };

        var serializedValue = JsonSerializer.Serialize(testObject);
        var desirializeValue = JsonDesirializer.Desiriailize<TestClass>(serializedValue);

        Assert.Equal(testObject.Name, desirializeValue.Name);
        Assert.Equal(testObject.Age, desirializeValue.Age);
    }

    [Fact]
    public void DesirializeObjectsCollection_ShouldReturnExpectedDesirilizedTypeCollection()
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
        var desirializeValue = JsonDesirializer.Desiriailize<TestClass[]>(serializedValue);

        Assert.Equal(testArray[0].Age, desirializeValue[0].Age);
        Assert.Equal(testArray[0].Name, desirializeValue[0].Name);

        Assert.Equal(testArray[1].Age, desirializeValue[1].Age);
        Assert.Equal(testArray[1].Name, desirializeValue[1].Name);
    }
}
