using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FXHolder : MonoBehaviour
{
    [SerializeReference] public IEffect effect;
    void OnEnable()
    {
        // if(!startUp || effect == null)
        //     return;
        
        effect.PlayAsync(gameObject, 3);
    }

    void OnDestroy()
    {
        effect.StopFX();
    }
}

[CustomEditor(typeof(FXHolder))]
public class EffectUserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FXHolder effectUser = (FXHolder)target;

        if (effectUser.effect == null)
        {
            if (GUILayout.Button("Add Effect"))
            {
                ShowEffectSelectionMenu(effectUser);
            }
        }
        else
        {
            EditorGUILayout.LabelField("Effect Type", effectUser.effect.GetType().Name);

            if (GUILayout.Button("Change Effect"))
            {
                ShowEffectSelectionMenu(effectUser);
            }

            if (GUILayout.Button("Remove Effect"))
            {
                effectUser.effect = null;
            }

            // Отобразить свойства объекта, если они есть
            EditorGUILayout.Space();
            SerializedProperty effectProperty = serializedObject.FindProperty("effect");
            if (effectProperty != null)
            {
                EditorGUILayout.PropertyField(effectProperty, true);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void ShowEffectSelectionMenu(FXHolder effectUser)
    {
        GenericMenu menu = new GenericMenu();

        // Получить все типы, реализующие IEffect
        Type interfaceType = typeof(IEffect);
        Assembly assembly = Assembly.GetAssembly(interfaceType);
        Type[] types = assembly.GetTypes().Where(t => !t.IsAbstract && interfaceType.IsAssignableFrom(t)).ToArray();

        foreach (Type type in types)
        {
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                effectUser.effect = Activator.CreateInstance(type) as IEffect;
                EditorUtility.SetDirty(effectUser);
            });
        }

        menu.ShowAsContext();
    }
}
