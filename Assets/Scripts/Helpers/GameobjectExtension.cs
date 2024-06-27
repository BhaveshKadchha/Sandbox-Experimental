using UnityEngine;

public static class GameobjectExtension
{
    public static T GetOrAdd<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (!component) component = obj.AddComponent<T>();
        
        return component;
    }

    public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
}