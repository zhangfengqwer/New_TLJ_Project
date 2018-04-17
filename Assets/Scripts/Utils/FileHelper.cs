using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public class FileHelper
{
    /// <summary>
    /// 拷贝oldlab的文件到newlab下面
    /// </summary>
    /// <param name="sourcePath">lab文件所在目录(@"~\labs\oldlab")</param>
    /// <param name="savePath">保存的目标目录(@"~\labs\newlab")</param>
    /// <returns>返回:true-拷贝成功;false:拷贝失败</returns>
    public static bool CopyOldLabFilesToNewLab(string sourcePath, string savePath)
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        #region //拷贝labs文件夹到savePath下

        try
        {
            string[] labDirs = Directory.GetDirectories(sourcePath); //目录
            string[] labFiles = Directory.GetFiles(sourcePath); //文件
            if (labFiles.Length > 0)
            {
                for (int i = 0; i < labFiles.Length; i++)
                {
                    if (Path.GetFileName(labFiles[i]) != ".lab") //排除.lab文件
                    {
                        File.Copy(sourcePath + "\\" + Path.GetFileName(labFiles[i]),
                            savePath + "\\" + Path.GetFileName(labFiles[i]), true);
                    }
                }
            }

            if (labDirs.Length > 0)
            {
                for (int j = 0; j < labDirs.Length; j++)
                {
                    Directory.GetDirectories(sourcePath + "\\" + Path.GetFileName(labDirs[j]));

                    //递归调用
                    CopyOldLabFilesToNewLab(sourcePath + "\\" + Path.GetFileName(labDirs[j]),
                        savePath + "\\" + Path.GetFileName(labDirs[j]));
                }
            }
        }
        catch (Exception)
        {
            return false;
        }

        #endregion

        return true;
    }
}