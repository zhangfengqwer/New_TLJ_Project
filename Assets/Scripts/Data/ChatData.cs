using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChatData
{
    public static ChatData s_instance = null;

    public List<ChatText> m_chatTextList = new List<ChatText>();

    public static ChatData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new ChatData();
        }

        return s_instance;
    }

    public void reqNet()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatData", "reqNet"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatData", "reqNet", null, null);
            return;
        }

        UnityWebReqUtil.Instance.Get(OtherData.getWebUrl() + "chat.json", httpCallBack);
    }

    public void httpCallBack(string tag, string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatData", "httpCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatData", "httpCallBack", null, tag, data);
            return;
        }

        try
        {
            // 读取配置文件
            {
                init(data);
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("获取聊天配置文件出错：" + ex.Message);
            OtherData.s_getNetEntityFile.GetFileFail("chat.json");

            //throw ex;
        }
    }

    public void init(string jsonData)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatData", "init"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatData", "init", null, jsonData);
            return;
        }

        m_chatTextList.Clear();

        // string jsonData = Resources.Load("Entity/chat").ToString();

        JsonData jd = JsonMapper.ToObject(jsonData);

        for (int i = 0; i < jd.Count; i++)
        {
            ChatText temp = new ChatText();

            temp.m_id = (int)jd[i]["id"];
            temp.m_text = (string)jd[i]["text"];

            m_chatTextList.Add(temp);
        }

        OtherData.s_getNetEntityFile.GetFileSuccess("chat.json");
    }

    public List<ChatText> getChatTextList()
    {
        return m_chatTextList;
    }

    public ChatText getChatTextById(int id)
    {
        ChatText chatText = null;

        for (int i = 0; i < m_chatTextList.Count; i++)
        {
            if (m_chatTextList[i].m_id == id)
            {
                chatText = m_chatTextList[i];
                break;
            }
        }

        return chatText;
    }
}

public class ChatText
{
    public int m_id = 0;
    public string m_text = "";
}