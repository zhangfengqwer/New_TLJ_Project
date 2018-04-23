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
    public GameObject m_changeNum = null;

    public int m_useNum = 1;

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
        OtherData.s_propDetailPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "Start", null, null);
            return;
        }
    }

    public void setPropId(int prop_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "setPropId"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "setPropId", null, prop_id);
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

            // 一元话费：显示数量调节对象
            if (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_1yuanhuafei)
            {
                int myHuaFei_1 = GameUtil.getMyPropNumById((int)TLJCommon.Consts.Prop.Prop_1yuanhuafei);
                if (myHuaFei_1 >= 10)
                {
                    GameUtil.showGameObject(m_changeNum);
                    m_useNum = 10;

                    m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = false;
                }
                else if (myHuaFei_1 >= 5)
                {
                    GameUtil.showGameObject(m_changeNum);
                    m_useNum = 5;

                    m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = false;
                }

                m_changeNum.transform.Find("Text_num").GetComponent<Text>().text = m_useNum.ToString();
            }

            if ((m_propInfo.m_id == 111) ||
                (m_propInfo.m_id == 112) ||
                (m_propInfo.m_id == 113))
            {
                GameObject obj = new GameObject();
                Text text = obj.AddComponent<Text>();
                text.text = "完成一次对局方可使用";
                text.fontSize = 20;
                CommonUtil.setFontColor(text, 0, 0, 0);
                text.font = transform.Find("Image_bg/Text_desc").GetComponent<Text>().font;
                text.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 30);

                obj.transform.SetParent(transform.Find("Image_bg"));
                obj.transform.localPosition = new Vector3(26, 30, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void onClickUseProp()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "onClickUseProp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "onClickUseProp", null, null);
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
            UseHuaFeiPanelScript.create(m_propInfo,m_useNum);
        }
        //徽章
        else if (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_huizhang)
        {
            ShopPanelScript.create(3);
        }
        //话费碎片
        else if ((m_propInfo.m_id == 120) || (m_propInfo.m_id == 121) || (m_propInfo.m_id == 122))
        {
            Activity.create(4);
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

    public void onClickJian()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "onClickJian"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "onClickJian", null, null);
            return;
        }

        // 一元话费
        if (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_1yuanhuafei)
        {
            int myHuaFei_1 = GameUtil.getMyPropNumById((int)TLJCommon.Consts.Prop.Prop_1yuanhuafei);

            if (m_useNum == 10)
            {
                m_useNum = 5;

                m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = true;
            }
            else if (m_useNum == 5)
            {
                m_useNum = 1;
                    
                m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = true;
                m_changeNum.transform.Find("Button_jian").GetComponent<Button>().interactable = false;
            }

            m_changeNum.transform.Find("Text_num").GetComponent<Text>().text = m_useNum.ToString();
        }
    }

    public void onClickJia()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "onClickJia"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "onClickJia", null, null);
            return;
        }

        // 一元话费
        if (m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_1yuanhuafei)
        {
            int myHuaFei_1 = GameUtil.getMyPropNumById((int)TLJCommon.Consts.Prop.Prop_1yuanhuafei);

            if (m_useNum == 1)
            {
                if (myHuaFei_1 >= 5)
                {
                    m_useNum = 5;

                    if (myHuaFei_1 < 10)
                    {
                        m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = false;
                    }

                    m_changeNum.transform.Find("Button_jian").GetComponent<Button>().interactable = true;
                }
            }
            else if (m_useNum == 5)
            {
                if (myHuaFei_1 >= 10)
                {
                    m_useNum = 10;

                    m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = false;
                    m_changeNum.transform.Find("Button_jian").GetComponent<Button>().interactable = true;
                }
            }

            m_changeNum.transform.Find("Text_num").GetComponent<Text>().text = m_useNum.ToString();
        }
    }

    public void onReceive_UseProp(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "onReceive_UseProp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "onReceive_UseProp", null, data);
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
        else
        {
            ToastScript.createToast("使用失败");
        }
    }

    public void useProp(int id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "useProp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "useProp", null, id);
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

            // 10元、20元京东卡
            case 130:
            case 131:
                {
                    ToastScript.createToast("请去邮箱查看");
                }
                break;
        }
    }

    public void UpdatePropData()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("PropDetailPanelScript_hotfix", "UpdatePropData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.PropDetailPanelScript_hotfix", "UpdatePropData", null, null);
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
