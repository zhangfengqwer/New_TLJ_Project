using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_chat_List_Script : MonoBehaviour {

    public ChatPanelScript m_parentScript;
    
    public Text m_text_content;
    public Button m_button_item;

    ChatText m_chatText;

    // Use this for initialization
    void Start()
    {

    }

    public void setChatData(ChatText chatText)
    {
        m_chatText = chatText;
        
        m_text_content.text = m_chatText.m_text;
    }

    public ChatText getChatText()
    {
        return m_chatText;
    }
    
    public void onClickItem()
    {
        LogUtil.Log("聊天："+ m_chatText.m_text);
        m_parentScript.reqChat(m_chatText);
    }
}
