using UnityEngine;


/// <summary>
/// Property drawer to expose a property of an object
/// </summary>
public class EmbeddedPropertyAttribute : PropertyAttribute
{
    public readonly string PropertyName;

    public EmbeddedPropertyAttribute(string propertyName)
    {
        PropertyName = propertyName;
    }
}
