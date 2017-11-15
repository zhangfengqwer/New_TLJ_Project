using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropDetailPanelScript : MonoBehaviour {

    public BagPanelScript m_parentScript;

    public Text m_text_name;
    public Text m_text_desc;
    public Image m_image_icon;
    public Button m_button_use;
    PropInfo m_propInfo = null;

    public static GameObject create(int prop_id, BagPanelScript parentScript)
    {
       
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PropDetailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<PropDetailPanelScript>().setPropId(prop_id);
        obj.GetComponent<PropDetailPanelScript>().m_parentScript = parentScript;
        return obj;
    }

    public void setPropId(int prop_id)
    {
        m_propInfo = PropData.getInstance().getPropInfoById(prop_id);

        if (m_propInfo != null)
        {
            m_text_name.text = m_propInfo.m_name;
            m_text_desc.text = m_propInfo.m_desc;
            CommonUtil.setImageSprite(m_image_icon, GameUtil.getPropIconPath(m_propInfo.m_id));

            if (m_propInfo.m_type != 0)
            {
                m_button_use.transform.localScale = new Vector3(0,0,0);
            }
        }
    }

    public void onClickUseProp()
    {
        // 喇叭
        if (m_propInfo.m_id == 106)
        {
            LaBaPanelScript.create(GameObject.Find("Canvas").GetComponent<MainScript>());
        }
        // 话费
        else if ((m_propInfo.m_id == 111) || (m_propInfo.m_id == 112))
        {
//            ToastScript.createToast("话费充值接口暂未开放,敬请期待");
//            return;
            UseHuaFeiPanelScript.create(m_propInfo);
        }
        // 其他
        else
        {

            LogicEnginerScript.Instance.GetComponent<UsePropRequest>().SetPropId(m_propInfo.m_id);
            LogicEnginerScript.Instance.GetComponent<UsePropRequest>().CallBack = onReceive_UseProp;
            LogicEnginerScript.Instance.GetComponent<UsePropRequest>().OnRequest();
        }
    }

    public void onReceive_UseProp(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("使用成功");

            useProp(m_propInfo.m_id);
            UpdatePropData();
        }
    }

    void useProp(int id)
    {
        switch (id)
        {
            // 记牌器
            case 101:
                {
                    // 不用处理
                }
                break;

            // 加倍卡
            case 102:
                {
                    // 不用处理
                }
                break;

            // 鸡蛋
            case 103:
                {
                    // 不用处理
                }
                break;

            // 鲜花
            case 104:
                {
                    // 不用处理
                }
                break;

            // 出牌发光
            case 105:
                {
                    // 不用处理
                }
                break;

            // 喇叭
            case 106:
                {

                }
                break;

            // 蓝钻石
            case 107:
                {
                    // 不用处理
                }
                break;

            // 逃跑率清零卡
            case 108:
                {
                    UserData.gameData.runCount = 0;
                }
                break;

            // 魅力值修正卡
            case 109:
                {
                    UserData.gameData.meiliZhi = 0;
                }
                break;

            // 徽章
            case 110:
                {
                    // 不用处理
                }
                break;
        }
    }

    void UpdatePropData()
    {
        List<UserPropData> userPropDatas = UserData.propData;

        for (int i = 0; i < userPropDatas.Count; i++)
        {
            UserPropData Prop = userPropDatas[i];
            if (m_propInfo.m_id == Prop.prop_id)
            {
                Prop.prop_num--;
                if (Prop.prop_num == 0)
                {
                    userPropDatas.Remove(Prop);
                    BagPanelScript.Instance.deleteItem(i);
                    Destroy(this.gameObject);
                    return;
                }
            }
        }

        BagPanelScript.Instance.UpdateUI();
    }
}
