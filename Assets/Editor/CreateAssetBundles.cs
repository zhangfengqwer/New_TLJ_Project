using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Tools/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "streamingAssets";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

#if UNITY_ANDROID
        assetBundleDirectory += "/android";
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
#endif

#if UNITY_IPHONE
        assetBundleDirectory += "/ios";
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
#endif

#if UNITY_STANDALONE_WIN
        assetBundleDirectory += "/pc";
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
#endif
        GenerateVersionInfo(assetBundleDirectory);


        Debug.Log("打包完成");
    }

    private static void GenerateVersionInfo(string dir)
    {
        VersionConfig versionProto = new VersionConfig();
        GenerateVersionProto(dir, versionProto);

        using (FileStream fileStream = new FileStream(dir + "/Version.txt", FileMode.Create))
        {
            string json = LitJson.JsonMapper.ToJson(versionProto);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            fileStream.Write(bytes, 0, bytes.Length);
        }
    }

    private static void GenerateVersionProto(string dir, VersionConfig versionProto)
    {
        foreach (string file in Directory.GetFiles(dir))
        {
            LogUtil.Log(file);
            if (file.EndsWith(".manifest")) continue;

            string md5 = FileMD5(file);
            System.IO.FileInfo fi = new System.IO.FileInfo(file);

            long length = fi.Length;

            versionProto.FileVersionInfos.Add(new FileVersionInfo
            {
                File = fi.Name,
                MD5 = md5,
                Size = (int)length,
            });
        }
    }

    public static string FileMD5(string filePath)
    {
        byte[] retVal;
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            retVal = md5.ComputeHash(file);
        }

        return ToHex(retVal, "x2");
    }

    public static string ToHex(byte[] bytes, string format)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in bytes)
        {
            stringBuilder.Append(b.ToString(format));
        }

        return stringBuilder.ToString();
    }
}
