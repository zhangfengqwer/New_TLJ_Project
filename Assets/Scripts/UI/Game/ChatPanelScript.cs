using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanelScript : MonoBehaviour {

    public GameScript m_parentScript;

    public GameObject m_listView_chat;
    public ListViewScript m_ListViewScript_chat;

    public GameObject m_listView_emoji;
    public ListViewScript m_ListViewScript_emoji;

    public Image m_tab_bg;
    public Button m_button_chat;
    public Button m_button_biaoqing;

    public bool m_canChat = true;

    public static GameObject create(GameScript parent)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/ChatPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<ChatPanelScript>().m_parentScript = parent;

        return obj;
    }

    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_ListViewScript_chat = m_listView_chat.GetComponent<ListViewScript>();
        m_ListViewScript_emoji = m_listView_emoji.GetComponent<ListViewScript>();

        loadChat();
    }

    public void loadChat()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatPanelScript_hotfix", "loadChat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatPanelScript_hotfix", "loadChat", null, null);
            return;
        }

        {
            m_tab_bg.transform.localPosition = new Vector3(-83, 0, 0);
            m_button_chat.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/btn_chat_xuanze", typeof(Sprite)) as Sprite;
            m_button_biaoqing.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/btn_emogi_weixuanze", typeof(Sprite)) as Sprite;
        }

        m_ListViewScript_chat.clear();

        GameUtil.showGameObject(m_listView_chat);
        GameUtil.hideGameObject(m_listView_emoji);

        for (int i = 0; i < ChatData.getInstance().getChatTextList().Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_chat_List") as GameObject;
            GameObject obj = MonoBehaviour.Instantiate(prefab);
            obj.GetComponent<Item_chat_List_Script>().m_parentScript = this;
            obj.GetComponent<Item_chat_List_Script>().setChatData(ChatData.getInstance().getChatTextList()[i]);

            m_ListViewScript_chat.addItem(obj);
        }

        m_ListViewScript_chat.addItemEnd();
    }

    public void loadBiaoQing()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatPanelScript_hotfix", "loadBiaoQing"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatPanelScript_hotfix", "loadBiaoQing", null, null);
            return;
        }

        {
            {
                m_tab_bg.transform.localPosition = new Vector3(83, 0, 0);
                m_button_chat.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/btn_chat_weixuanze", typeof(Sprite)) as Sprite;
                m_button_biaoqing.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("Sprites/Game/btn_emogi_xuanze", typeof(Sprite)) as Sprite;
            }
        }

        m_ListViewScript_emoji.clear();

        GameUtil.hideGameObject(m_listView_chat);
        GameUtil.showGameObject(m_listView_emoji);

        {
            GameObject obj = new GameObject();
            for (int i = 0; i < 16; i++)
            {
                GameObject button = new GameObject();
                button.AddComponent<Image>();
                button.AddComponent<Button>();
                button.transform.name = i.ToString();

                button.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    int id = int.Parse(button.transform.name);
                    reqChat_emoji(id + 1);
                });

                string path = "Sprites/Emoji/Expression-" + (i + 1) + "_1";
                CommonUtil.setImageSprite(button.GetComponent<Image>(), path);
                button.GetComponent<Image>().SetNativeSize();

                if (i % 6 == 0)
                {
                    obj = new GameObject();
                    m_ListViewScript_emoji.addItem(obj);
                }

                button.transform.SetParent(obj.transform);

                button.transform.localPosition = new Vector3(-250 + (i % 6) * 100, 0, 0);
            }

            m_ListViewScript_emoji.addItemEnd();
        }
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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatPanelScript_hotfix", "reqChat"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatPanelScript_hotfix", "reqChat", null, chatText);
            return;
        }

        if (m_canChat)
        {
            m_canChat = false;
            m_parentScript.reqChat(1,chatText.m_id);

            Invoke("onInvoke",4);
        }
        else
        {
            ToastScript.createToast("请隔3秒再发送");
        }
    }

    public void reqChat_emoji(int id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("ChatPanelScript_hotfix", "reqChat_emoji"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.ChatPanelScript_hotfix", "reqChat_emoji", null, id);
            return;
        }

        if (m_canChat)
        {
            m_canChat = false;
            m_parentScript.reqChat(2, id);

            Invoke("onInvoke", 2);
        }
        else
        {
            ToastScript.createToast("请隔2秒再发送");
        }
    }

    public void onInvoke()
    {
        m_canChat = true;
    }
}
