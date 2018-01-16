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
    public PropInfo m_propInfo = null;

    public static GameObject create(int prop_id, BagPanelScript parentScript)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/PropDetailPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<PropDetailPanelScript>().setPropId(prop_id);
        obj.GetComponent<PropDetailPanelScript>().m_parentScript = parentScript;
        return obj;
    }

    private void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript", "Start", null, null);
            return;
        }
    }

    public void setPropId(int prop_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript", "setPropId"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript", "setPropId", null, prop_id);
            return;
        }

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
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript", "onClickUseProp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript", "onClickUseProp", null, null);
            return;
        }

        // 喇叭
        if (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_laba)
        {
            LaBaPanelScript.create();
        }
        // 话费
        else if ((m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_1yuanhuafei) || (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_5yuanhuafei) || (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_10yuanhuafei))
        {
            UseHuaFeiPanelScript.create(m_propInfo);
        }
        //徽章
        else if (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_huizhang)
        {
            ShopPanelScript.create(3);
        }
        // 其他
        else
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<UsePropRequest>().SetPropId(m_propInfo.m_id);
            LogicEnginerScript.Instance.GetComponent<UsePropRequest>().CallBack = onReceive_UseProp;
            LogicEnginerScript.Instance.GetComponent<UsePropRequest>().OnRequest();
        }
    }

    public void onReceive_UseProp(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript", "onReceive_UseProp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript", "onReceive_UseProp", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];
        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("使用成功");

            useProp(m_propInfo.m_id);
            UpdatePropData();
        }
    }

    public void useProp(int id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript", "useProp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript", "useProp", null, id);
            return;
        }

        switch (id)
        {
            // 记牌器
            case (int)TLJCommon.Consts.Prop.Prop_jipaiqi:
                {
                    // 增加到buff数据里
                    {
                        bool isFind = false;
                        for (int i = 0; i < UserData.buffData.Count; i++)
                        {
                            if ((UserData.buffData[i].prop_id == (int)TLJCommon.Consts.Prop.Prop_jipaiqi))
                            {
                                isFind = true;
                                ++UserData.buffData[i].buff_num;
                                break;
                            }
                        }

                        if (!isFind)
                        {
                            UserData.buffData.Add(new BuffData((int)TLJCommon.Consts.Prop.Prop_jipaiqi, 1));
                        }
                    }

                    if (OtherData.s_gameScript != null)
                    {
                        OtherData.s_gameScript.useProp_jipaiqi();
                    }
                }
                break;

            // 加倍卡
            case (int)TLJCommon.Consts.Prop.Prop_jiabeika:
                {
                    // 不用处理
                }
                break;

            // 鸡蛋
            case (int)TLJCommon.Consts.Prop.Prop_jidan:
                {
                    // 不用处理
                }
                break;

            // 鲜花
            case (int)TLJCommon.Consts.Prop.Prop_xianhua:
                {
                    // 不用处理
                }
                break;

            // 出牌发光
            case (int)TLJCommon.Consts.Prop.Prop_chupaifaguang:
                {
                    // 不用处理
                }
                break;

            // 喇叭
            case (int)TLJCommon.Consts.Prop.Prop_laba:
                {

                }
                break;

            // 蓝钻石
            case (int)TLJCommon.Consts.Prop.Prop_lanzuanshi:
                {
                    // 不用处理
                }
                break;

            // 逃跑率清零卡
            case (int)TLJCommon.Consts.Prop.Prop_taopaolvqinglin:
                {
                    UserData.gameData.runCount = 0;
                }
                break;

            // 魅力值修正卡
            case (int)TLJCommon.Consts.Prop.Prop_meilizhixiuzheng:
                {
                    UserData.gameData.meiliZhi = 0;
                }
                break;

            // 徽章
            case (int)TLJCommon.Consts.Prop.Prop_huizhang:
                {
                    // 不用处理
                }
                break;
        }
    }

    public void UpdatePropData()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript", "UpdatePropData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript", "UpdatePropData", null, null);
            return;
        }

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
