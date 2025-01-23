using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class SceneLoaderEditor : Editor
{
    [MenuItem("Scene/Main _F1")]
    public static void SwitchScene()
    {
        SceneLoad("Assets/Scenes/Test.unity");
    }

    static void SceneLoad(string scenePath)
    {
        if (SceneManager.GetActiveScene().isDirty)
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
    }
}
































































