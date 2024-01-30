using UnityEngine;


/// <summary>
/// Attribute that utilizes the GeneratedDropDownMenuPropertyDrawer to draw a drop down menu on the inspector
/// </summary>
public class GeneratedDropDownMenuAttribute : PropertyAttribute
{
    public string FunctionName { get; private set; }
    public bool UseDefaultBehaviorIfEmpty { get; private set; }
    public System.Type Type { get; private set; }

    public GeneratedDropDownMenuAttribute(string functionName, bool useDefaultBehaviorIfEmpty = false)
    {
        FunctionName = functionName;
        UseDefaultBehaviorIfEmpty = useDefaultBehaviorIfEmpty;
    }

    public GeneratedDropDownMenuAttribute(string functionName, System.Type type, bool useDefaultBehaviorIfEmpty = false)
    {
        FunctionName = functionName;
        Type = type;
        UseDefaultBehaviorIfEmpty = useDefaultBehaviorIfEmpty;
    }
}
