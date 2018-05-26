using Newtonsoft.Json;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Linq;

public class Vec2Converter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var data = serializer.Deserialize<Dictionary<Vector2Int, Collection>>(reader);
        return data;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Dictionary<Vector2Int, Collection>);
    }
}

[TypeConverter(typeof(Vector2IntConverter))]
public class Vector2IntConverter : TypeConverter
{
    // Overrides the CanConvertFrom method of TypeConverter.
    // The ITypeDescriptorContext interface provides the context for the
    // conversion. Typically, this interface is used at design time to 
    // provide information about the design-time container.
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }
    // Overrides the ConvertFrom method of TypeConverter.
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if(value is string) return JsonConvert.DeserializeObject<Vector2Int>(value.ToString());
        return base.ConvertFrom(context, culture, value);
    }
    // Overrides the ConvertTo method of TypeConverter.
    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(string)) return JsonConvert.SerializeObject(value);
        return base.ConvertTo(context, culture, value, destinationType);
    }
}

[TypeConverter(typeof(Vector2IntConverter))]
public class Location
{
    public int x;
    public int y;
}

public class DeepDictionaryConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (typeof(IDictionary).IsAssignableFrom(objectType) ||
                TypeImplementsGenericInterface(objectType, typeof(IDictionary<,>)));
    }

    private static bool TypeImplementsGenericInterface(Type concreteType, Type interfaceType)
    {
        return concreteType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Type type = value.GetType();
        IEnumerable keys = (IEnumerable)type.GetProperty("Keys").GetValue(value, null);
        IEnumerable values = (IEnumerable)type.GetProperty("Values").GetValue(value, null);
        IEnumerator valueEnumerator = values.GetEnumerator();

        writer.WriteStartArray();
        foreach (object key in keys)
        {
            valueEnumerator.MoveNext();

            writer.WriteStartArray();
            serializer.Serialize(writer, key);
            serializer.Serialize(writer, valueEnumerator.Current);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}