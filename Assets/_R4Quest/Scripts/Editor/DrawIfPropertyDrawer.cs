#if UNITY_EDITOR 
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DrawIfAttribute))]
public class DrawIfPropertyDrawer : PropertyDrawer
{
    DrawIfAttribute drawIf;
    SerializedProperty comparedField;
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!ShowMe(property) && drawIf.panelType == DrawIfAttribute.PanelTypeEnum.ImgTtleTxt)
            return 0f;
   
        // The height of the property should be defaulted to the default height.
        return base.GetPropertyHeight(property, label);
    }
 
    /// <summary>
    /// Errors default to showing the property.
    /// </summary>
    private bool ShowMe(SerializedProperty property)
    {
        drawIf = attribute as DrawIfAttribute;
        // Replace propertyname to the value from the parameter
        string path = property.propertyPath.Contains(".") ? System.IO.Path.ChangeExtension(property.propertyPath, drawIf.comparedPropertyName) : drawIf.comparedPropertyName;
 
        comparedField = property.serializedObject.FindProperty(path);
 
        if (comparedField == null)
        {
            Debug.LogError("Cannot find property with name: " + path);
            return true;
        }
 
        // get the value & compare based on types
        switch (comparedField.type)
        { // Possible extend cases to support your own type
            case "bool":
                return comparedField.boolValue.Equals(drawIf.comparedValue);
            case "Enum":
                return comparedField.enumValueIndex.Equals((int)drawIf.comparedValue);
            default:
                Debug.LogError("Error: " + comparedField.type + " is not supported of " + path);
                return true;
        }
    }
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (ShowMe(property))
        {
            EditorGUI.PropertyField(position, property);
        } 
        else if (drawIf.panelType == DrawIfAttribute.PanelTypeEnum.FourImg)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property);
            GUI.enabled = true;
        }
    }
}
#endif 