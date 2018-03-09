using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AndroidTool  : EditorWindow{


    [MenuItem("Tools/生成Apk")]
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

    public class CreateAssetBundles
    {
        [MenuItem("Tools/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            string assetBundleDirectory = "Assets/StreamingAssets";
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        }

    }
}
