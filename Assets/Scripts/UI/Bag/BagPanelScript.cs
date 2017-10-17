using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelScript : MonoBehaviour {
    private UIWarpContent uiWarpContent;

    public static BagPanelScript Instance;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BagPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas").transform);
        return obj;
    }
    // Use this for initialization
    void Start ()
    {
        // 拉取背包
        {
            LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
            LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
        }
    }

    public void deleteItem(int dataindex)
    {
        uiWarpContent.DelItem(dataindex);
    }

    public void UpdateUI()
    {
        for (int i = 0; i < UserData.propData.Count; i++)
        {
            deleteItem(i);
        }
        uiWarpContent.Init(UserData.propData.Count);
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {
        var find = go.transform.Find("Text");
        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            // 显示道具详情
            PropDetailPanelScript.create(UserData.propData[dataindex].prop_id, this);
        });

        find.GetComponent<Text>().text = UserData.propData[dataindex].prop_id + "x"+ UserData.propData[dataindex].prop_num;
    }

    public void onReceive_GetUserBag(string data)
    {
        {
            JsonData jsonData = JsonMapper.ToObject(data);
            var code = (int)jsonData["code"];
            if (code == (int)Consts.Code.Code_OK)
            {
                UserData.propData = JsonMapper.ToObject<List<UserPropData>>(jsonData["prop_list"].ToString());
            }
            else
            {
                ToastScript.createToast("用户背包数据错误");
                return;
            }
        }

        if (Instance == null)
        {
            Instance = this;
        }
        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
        uiWarpContent.onInitializeItem = onInitializeItem;
        uiWarpContent.Init(UserData.propData.Count);
    }
}
