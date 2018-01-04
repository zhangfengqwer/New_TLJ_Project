using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AndroidTool  : EditorWindow{


    [MenuItem("Apk/生成Apk")]
    public static void ShowWindow()
    {
        string[] levels = { "Assets/Scenes/LoginScene.unity", "Assets/Scenes/MainScene.unity", "Assets/Scenes/GameScene.unity" };
        BuildPipeline.BuildPlayer(levels, @"C:\Users\Administrator\Desktop\fksj\fksj.apk", BuildTarget.Android, BuildOptions.None);
    }

//    private void OnGUI()
//    {
//        GUILayout.Label("base setting", EditorStyles.boldLabel);
//        myString = EditorGUILayout.TextField("text field", myString);
//    }
}
