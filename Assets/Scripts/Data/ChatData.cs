using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ChatData
{
    static ChatData s_instance = null;

    List<ChatText> m_chatTextList = new List<ChatText>();

    public static ChatData getInstance()
    {
        if (s_instance == null)
        {
            s_instance = new ChatData();
        }

        return s_instance;
    }

    public void init()
    {
        m_chatTextList.Clear();

        string jsonData = Resources.Load("Entity/chat").ToString();

        JsonData jd = JsonMapper.ToObject(jsonData);

        for (int i = 0; i < jd.Count; i++)
        {
            ChatText temp = new ChatText();

            temp.m_id = (int)jd[i]["id"];
            temp.m_text = (string)jd[i]["text"];

            m_chatTextList.Add(temp);
        }
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