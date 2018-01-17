using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_chat_List_Script : MonoBehaviour {

    public ChatPanelScript m_parentScript;
    
    public Text m_text_content;
    public Button m_button_item;

    public ChatText m_chatText;

    // Use this for initialization
    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Item_chat_List_Script_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Item_chat_List_Script_hotfix", "Start", null, null);
            return;
        }
    }

    public void setChatData(ChatText chatText)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Item_chat_List_Script_hotfix", "setChatData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Item_chat_List_Script_hotfix", "setChatData", null, chatText);
            return;
        }

        m_chatText = chatText;
        
        m_text_content.text = m_chatText.m_text;
    }

    public ChatText getChatText()
    {
        return m_chatText;
    }
    
    public void onClickItem()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Item_chat_List_Script_hotfix", "onClickItem"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Item_chat_List_Script_hotfix", "onClickItem", null, null);
            return;
        }

        LogUtil.Log("聊天："+ m_chatText.m_text);
        m_parentScript.reqChat(m_chatText);
    }
}
