using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNetEntityFile : MonoBehaviour
{
    List<FileInfo> m_fileList = new List<FileInfo>();

    private void Awake()
    {
        OtherData.s_getNetEntityFile = this;
    }

    // Use this for initialization
    void Start ()
    {
        m_fileList.Add(new FileInfo("NetConfig.json"));
        m_fileList.Add(new FileInfo("prop.json"));
        m_fileList.Add(new FileInfo("chat.json"));
        m_fileList.Add(new FileInfo("hudong.json"));
        //m_fileList.Add(new FileInfo("stopwords.txt"));
        m_fileList.Add(new FileInfo("VipRewardData.json"));

        if (OtherData.s_isTest)
        {
            ToastScript.createToast("这是测试包");
        }
        else
        {
            LogUtil.Log("这是正式包");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnDestroy()
    {
        OtherData.s_getNetEntityFile = null;
    }

    // 获取数值表
    public void getNetFile()
    {
        Invoke("onInvoke",6);

        // 恢复初始状态
        {
            for (int i = 0; i < m_fileList.Count; i++)
            {
                m_fileList[i].m_fileGetState = FileInfo.FileGetState.FileGetState_NoStart;
            }
        }

        // 拉取数值表
        {
            NetLoading.getInstance().Show();

            NetConfig.reqNetConfig();
            PropData.getInstance().reqNet();
            ChatData.getInstance().reqNet();
            HuDongData.getInstance().reqNet();
            SensitiveWordUtil.reqNet();
            VipData.reqNet();
        }
    }

    void onInvoke()
    {
        NetLoading.getInstance().Close();

        NetErrorPanelScript.getInstance().Show();
        NetErrorPanelScript.getInstance().setOnClickButton(onClick_retryGetNetFile);
        NetErrorPanelScript.getInstance().setContentText("获取配置文件超时，请重新获取");
    }

    public void GetFileSuccess(string fileName)
    {
        for (int i = 0; i < m_fileList.Count; i++)
        {
            if (m_fileList[i].m_fileName.CompareTo(fileName) == 0)
            {
                m_fileList[i].m_fileGetState = FileInfo.FileGetState.FileGetState_GetSuccess;
                break;
            }
        }

        {
            bool hasGetAllFile = true;
            for (int i = 0; i < m_fileList.Count; i++)
            {
                if (m_fileList[i].m_fileGetState != FileInfo.FileGetState.FileGetState_GetSuccess)
                {
                    hasGetAllFile = false;
                    break;
                }
            }

            // 全部获取完毕：成功
            if (hasGetAllFile)
            {
                CancelInvoke("onInvoke");

                OtherData.s_loginScript.onGetAllNetFile();
            }
        }
    }

    public void GetFileFail(string fileName)
    {
        {
            for (int i = 0; i < m_fileList.Count; i++)
            {
                if (m_fileList[i].m_fileName.CompareTo(fileName) == 0)
                {
                    m_fileList[i].m_fileGetState = FileInfo.FileGetState.FileGetState_GetFail;
                    break;
                }
            }
        }

        {
            bool hasAllEnd = true;
            for (int i = 0; i < m_fileList.Count; i++)
            {
                if (m_fileList[i].m_fileGetState == FileInfo.FileGetState.FileGetState_NoStart)
                {
                    hasAllEnd = false;
                    break;
                }
            }

            // 全部获取完毕:有成功的有失败的
            if (hasAllEnd)
            {
                NetLoading.getInstance().Close();

                NetErrorPanelScript.getInstance().Show();
                NetErrorPanelScript.getInstance().setOnClickButton(onClick_retryGetNetFile);
                NetErrorPanelScript.getInstance().setContentText("获取配置文件失败，请重新获取");

                CancelInvoke("onInvoke");
            }
        }
    }

    void onClick_retryGetNetFile()
    {
        NetErrorPanelScript.getInstance().Close();

        getNetFile();
    }
}

public class FileInfo
{
    public enum FileGetState
    {
        FileGetState_NoStart,
        FileGetState_GetSuccess,
        FileGetState_GetFail
    }

    public string m_fileName = "";
    public FileGetState m_fileGetState = FileGetState.FileGetState_NoStart;

    public FileInfo(string fileName)
    {
        m_fileName = fileName;
    }
}
