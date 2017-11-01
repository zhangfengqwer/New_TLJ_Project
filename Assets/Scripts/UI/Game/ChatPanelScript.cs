using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanelScript : MonoBehaviour {

    public GameScript m_parentScript;
    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public Button m_button_chat;
    public Button m_button_biaoqing;

    bool m_canChat = true;

    public static GameObject create(GameScript parent)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ChatPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<ChatPanelScript>().m_parentScript = parent;

        return obj;
    }

    void Start()
    {
        ChatData.getInstance().init();
        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        loadChat();
    }

    public void loadChat()
    {
        m_ListViewScript.clear();

        {
            m_button_chat.transform.SetAsLastSibling();
            m_button_chat.transform.Find("Text").GetComponent<Text>().color = Color.yellow;
            m_button_biaoqing.transform.Find("Text").GetComponent<Text>().color = Color.gray;
        }

        for (int i = 0; i < ChatData.getInstance().getChatTextList().Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_chat_List") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<Item_chat_List_Script>().m_parentScript = this;
            obj.GetComponent<Item_chat_List_Script>().setChatData(ChatData.getInstance().getChatTextList()[i]);

            m_ListViewScript.addItem(obj);
        }

        m_ListViewScript.addItemEnd();
    }

    public void loadBiaoQing()
    {
        m_ListViewScript.clear();

        {
            m_button_biaoqing.transform.SetAsLastSibling();
            m_button_biaoqing.transform.Find("Text").GetComponent<Text>().color = Color.yellow;
            m_button_chat.transform.Find("Text").GetComponent<Text>().color = Color.gray;
        }
        //for (int i = 0; i < UserMailData.getInstance().getUserMailDataList().Count; i++)
        //{
        //    GameObject prefab = Resources.Load("Prefabs/UI/Item/Mail_List_Item") as GameObject;
        //    GameObject obj = MonoBehaviour.Instantiate(prefab);
        //    obj.GetComponent<Mail_List_Item_Script>().m_parentScript = this;
        //    obj.GetComponent<Mail_List_Item_Script>().setMailData(UserMailData.getInstance().getUserMailDataList()[i]);

        //    obj.transform.name = UserMailData.getInstance().getUserMailDataList()[i].m_email_id.ToString();

        //    m_ListViewScript.addItem(obj);
        //}

        //m_ListViewScript.addItemEnd();
    }
    
    public void onClickChat()
    {
        loadChat();
    }

    public void onClickBiaoQing()
    {
        loadBiaoQing();
    }

    public void reqChat(ChatText chatText)
    {
        if (m_canChat)
        {
            m_canChat = false;
            m_parentScript.reqChat(chatText.m_id);

            Invoke("onInvoke",4);
        }
        else
        {
            ToastScript.createToast("请隔4秒再发送");
        }
    }

    void onInvoke()
    {
        m_canChat = true;
    }
}
