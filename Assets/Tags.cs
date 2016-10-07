using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

/// http://stackoverflow.com/questions/424366/c-sharp-string-enums

//use :
//string valueOfAuthenticationMethod = StringEnum.GetStringValue(Tags.PLATFORM);

public enum Tags
{
    [StringValue("LeftWallPlatform")]
    LEFT_WALL,
    [StringValue("RightWallPlatform")]
    RIGHT_WALL,
    [StringValue("Platform")]
    PLATFORM,
    [StringValue("Roof")]
    ROOF,
    [StringValue("Spike")]
    SPIKE
}

public class StringValue : System.Attribute
{
    private readonly string _value;

    public StringValue(string value)
    {
        _value = value;
    }

    public string Value
    {
        get { return _value; }
    }

}

public static class StringEnum
{
    public static string GetStringValue(Enum value)
    {
        string output = null;
        Type type = value.GetType();

        FieldInfo fi = type.GetField(value.ToString());
        StringValue[] attrs =
           fi.GetCustomAttributes(typeof(StringValue),
                                   false) as StringValue[];
        if (attrs.Length > 0)
        {
            output = attrs[0].Value;
        }

        return output;
    }
}