using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AndroidTool  : EditorWindow
{

    private static string fksjAssetPath = @"E:\fksj\javgame_online\fwsjsdk\src\main\assets";
    private static string exportApkPath = @"C:\Users\Administrator\Desktop\fksj\";
    private const string scenePath = "Assets/Scenes";

    [MenuItem("Tools/生成Apk")]
    public static void ShowWindow()
    {
        string[] scenes = Directory.GetFiles(scenePath);
        List<string> list = new List<string>();
        foreach (var item in scenes)
        {
            if (item.EndsWith(".unity"))
            {
                list.Add(item);
            }
        }

        scenes = new string[]
        {
            "Assets/Scenes/LoginScene.unity", "Assets/Scenes/MainScene.unity",
            "Assets/Scenes/GameScene.unity", "Assets/Scenes/GameScene_doudizhu.unity",
        };
        BuildPipeline.BuildPlayer(scenes, exportApkPath + "fksj.apk", BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("Tools/复制bin到游戏(先生成Apk)")]
    public static void Copy2GameAsset()
    {
        new FastZip().ExtractZip(exportApkPath + "fksj.apk", exportApkPath + "fksj/", "");

        //        FileHelper.DelectDir(fksjAssetPath);

//        bool copyOldLabFilesToNewLab = FileHelper.CopyOldLabFilesToNewLab(@"C:\Users\Administrator\Desktop\fksj\fksj\assets", fksjAssetPath);
//        Debug.Log(copyOldLabFilesToNewLab);
    }
}
