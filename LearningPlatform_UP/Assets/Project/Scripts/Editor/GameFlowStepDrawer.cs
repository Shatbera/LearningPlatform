using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameFlowStep))]
public class GameFlowStepDrawer : PropertyDrawer
{
    private const float Spacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty typeProperty = property.FindPropertyRelative("_type");
        SerializedProperty visibleProperty = GetVisibleProperty(property, typeProperty);

        Rect foldoutRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GetLabel(label, typeProperty), true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            Rect fieldRect = new(
                position.x,
                foldoutRect.yMax + Spacing,
                position.width,
                EditorGUI.GetPropertyHeight(typeProperty, true));

            EditorGUI.PropertyField(fieldRect, typeProperty);

            if (visibleProperty != null)
            {
                fieldRect.y = fieldRect.yMax + Spacing;
                fieldRect.height = EditorGUI.GetPropertyHeight(visibleProperty, true);
                EditorGUI.PropertyField(fieldRect, visibleProperty, true);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        if (!property.isExpanded)
        {
            return height;
        }

        SerializedProperty typeProperty = property.FindPropertyRelative("_type");
        SerializedProperty visibleProperty = GetVisibleProperty(property, typeProperty);

        height += Spacing + EditorGUI.GetPropertyHeight(typeProperty, true);

        if (visibleProperty != null)
        {
            height += Spacing + EditorGUI.GetPropertyHeight(visibleProperty, true);
        }

        return height;
    }

    private static GUIContent GetLabel(GUIContent label, SerializedProperty typeProperty)
    {
        if (typeProperty == null || typeProperty.enumValueIndex < 0 || typeProperty.enumValueIndex >= typeProperty.enumDisplayNames.Length)
        {
            return label;
        }

        return new GUIContent($"{label.text} - {typeProperty.enumDisplayNames[typeProperty.enumValueIndex]}");
    }

    private static SerializedProperty GetVisibleProperty(SerializedProperty stepProperty, SerializedProperty typeProperty)
    {
        if (typeProperty == null)
        {
            return null;
        }

        var type = (GameFlowStepType)typeProperty.enumValueIndex;
        return type switch
        {
            GameFlowStepType.WaitForEvent => stepProperty.FindPropertyRelative("_event"),
            GameFlowStepType.RaiseEvent => stepProperty.FindPropertyRelative("_event"),
            GameFlowStepType.Dialogue => stepProperty.FindPropertyRelative("_dialogue"),
            GameFlowStepType.Delay => stepProperty.FindPropertyRelative("_delaySeconds"),
            GameFlowStepType.StartMission => stepProperty.FindPropertyRelative("_mission"),
            GameFlowStepType.WaitForMissionComplete => stepProperty.FindPropertyRelative("_mission"),
            _ => null
        };
    }
}
