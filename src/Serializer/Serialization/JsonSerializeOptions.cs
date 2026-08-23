namespace Serializer.Serialization;

public class JsonSerializerOptions
{
    public bool SerializeEnumAsString { get; set; }
    public bool ThrowOnInvalidNumber { get; set; }
    public bool IgnoreNullProperties { get; set; }
}
