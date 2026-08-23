namespace Serializer.Deserialization;

public enum JsonToken
{
    None,

    StartObject,
    EndObject,

    StartArray,
    EndArray,

    PropertyName,

    String,
    Number,

    True,
    False,
    Null
}
