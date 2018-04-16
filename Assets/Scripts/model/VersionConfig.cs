using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

public class VersionConfig
{
    public int Version;
    public int TotalSize;
    public List<FileVersionInfo> FileVersionInfos = new List<FileVersionInfo>();
}

public class FileVersionInfo
{
    public string File;
    public string MD5;
    public int Size;
}