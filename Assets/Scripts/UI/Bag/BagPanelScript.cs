using LitJson;
using System.Collections;
using System.Collections.Generic;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

public class BagPanelScript : MonoBehaviour
{
    private UIWarpContent uiWarpContent;
    public static BagPanelScript Instance = null;
    public bool m_isNeedReqNet = true;
    public GameObject NoProp;

    public static GameObject create(bool isNeedReqNet)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/BagPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<BagPanelScript>().m_isNeedReqNet = isNeedReqNet;

        return obj;
    }

    // Use this for initialization
    void Start ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        uiWarpContent = gameObject.transform.GetComponentInChildren<UIWarpContent>();
        uiWarpContent.onInitializeItem = onInitializeItem;
        // 拉取背包数据
        if (m_isNeedReqNet)
        {
            LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
            LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
        }
        else
        {
            UpdateUI();
            if (UserData.propData.Count == 0)
            {
                NoProp.transform.localScale = Vector3.one;
            }
            else
            {
                NoProp.transform.localScale = Vector3.zero;
            }
        }
    }

    public void deleteItem(int dataindex)
    {
        uiWarpContent.DelItem(dataindex);
    }

    public void UpdateUI()
    {
        if (Instance != null)
        {
            for (int i = UserData.propData.Count - 1; i >= 0; i--)
            {
                deleteItem(i);
            }

            uiWarpContent.Init(UserData.propData.Count);
            if (UserData.propData.Count == 0)
            {
                NoProp.transform.localScale = Vector3.one;
            }
            else
            {
                NoProp.transform.localScale = Vector3.zero;
            }
        }
    }

    private void onInitializeItem(GameObject go, int dataindex)
    {

        Text propName = go.transform.Find("PropName").GetComponent<Text>();
        Image propImage = go.transform.Find("PropImage").GetComponent<Image>();
        propName.text = UserData.propData[dataindex].prop_name + "*" + UserData.propData[dataindex].prop_num;
        propImage.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/" + UserData.propData[dataindex].prop_icon);


        Button button = go.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate()
        {
            // 显示道具详情
            PropDetailPanelScript.create(UserData.propData[dataindex].prop_id, this);
        });
    }
    
    public void onReceive_GetUserBag(string data)
    {
        {
            JsonData jsonData = JsonMapper.ToObject(data);
            var code = (int)jsonData["code"];
            if (code == (int)Consts.Code.Code_OK)
            {
                UserData.propData = JsonMapper.ToObject<List<UserPropData>>(jsonData["prop_list"].ToString());
                for (int i = 0; i < PropData.getInstance().getPropInfoList().Count; i++)
                {
                    PropInfo propInfo = PropData.getInstance().getPropInfoList()[i];
                    for (int j = 0; j < UserData.propData.Count; j++)
                    {
                        UserPropData userPropData = UserData.propData[j];
                        if (propInfo.m_id == userPropData.prop_id)
                        {
                            userPropData.prop_icon = propInfo.m_icon;
                            userPropData.prop_name = propInfo.m_name;
                        }
                    }

                }
            }
            else
            {
                ToastScript.createToast("用户背包数据错误");
                return;
            }
        }

        UpdateUI();

       

    }
}
