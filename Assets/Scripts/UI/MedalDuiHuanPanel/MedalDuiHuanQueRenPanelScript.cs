using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalDuiHuanQueRenPanelScript : MonoBehaviour {
    public Image m_buyCount;

    public Text m_text_goods_name;
    public Text m_text_goods_desc;
    public Text m_text_goods_num;
    public Image m_text_goods_icon;

    public Button m_button_jian;
    public Button m_button_jia;
    public Button m_button_max;
    public Button m_button_buy;

    public MedalDuiHuanRewardDataContent m_medalDuiHuanRewardData = null;

    public int m_goods_num = 1;
    public int m_goods_buy_maxNum = 10;

    public static GameObject create(int goods_id)
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/MedalDuiHuanQueRenPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        obj.GetComponent<MedalDuiHuanQueRenPanelScript>().setGoodsId(goods_id);

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_medalDuiHuanQueRenPanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "Start", null, null);
            return;
        }

        m_button_jian.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setGoodsId(int goods_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "setGoodsId"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "setGoodsId", null, goods_id);
            return;
        }

        m_medalDuiHuanRewardData = MedalDuiHuanRewardData.getInstance().getMedalDuiHuanRewardDataContentById(goods_id);

        if (m_medalDuiHuanRewardData != null)
        {
            m_text_goods_name.text = m_medalDuiHuanRewardData.name;

            m_text_goods_num.text = m_goods_num.ToString();

            List<string> list_str = new List<string>();
            CommonUtil.splitStr(m_medalDuiHuanRewardData.reward_prop, list_str, ':');
            int prop_id = int.Parse(list_str[0]);

            // 道具图标
            {
                CommonUtil.setImageSprite(m_text_goods_icon, GameUtil.getPropIconPath(prop_id));
            }

            // 道具描述
            {
                if ((prop_id != 1) && (prop_id != 2))
                {
                    PropInfo propInfo = PropData.getInstance().getPropInfoById(prop_id);
                    if (propInfo != null)
                    {
                        m_text_goods_desc.text = propInfo.m_desc;
                    }
                }
            }

            refreshPrice();
        }
    }

    public void refreshPrice()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "refreshPrice"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "refreshPrice", null, null);
            return;
        }

        m_button_buy.transform.Find("Text_price").GetComponent<Text>().text = (m_medalDuiHuanRewardData.price * m_goods_num).ToString();
    }

    public void onClickJian()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "onClickJian"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "onClickJian", null, null);
            return;
        }

        m_button_jia.interactable = true;

        if ((--m_goods_num) == 1)
        {
            m_button_jian.interactable = false;
        }

        m_text_goods_num.text = m_goods_num.ToString();

        refreshPrice();
    }

    public void onClickJia()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "onClickJia"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "onClickJia", null, null);
            return;
        }

        m_button_jian.interactable = true;

        if ((++m_goods_num) == m_goods_buy_maxNum)
        {
            m_button_jia.interactable = false;
        }

        m_text_goods_num.text = m_goods_num.ToString();

        refreshPrice();
    }

    public void onClickMax()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "onClickMax"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "onClickMax", null, null);
            return;
        }

        m_goods_num = m_goods_buy_maxNum;
        m_text_goods_num.text = m_goods_num.ToString();

        m_button_jian.interactable = true;
        m_button_jia.interactable = false;

        refreshPrice();
    }

    public void onClickBuy()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "onClickBuy"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "onClickBuy", null, null);
            return;
        }

        // 判断是否设置过徽章密码
        {
            if (!UserData.isSetSecondPsw)
            {
                SetSecondPswPanelScript.create();
                ToastScript.createToast("请先设置徽章密码");

                return;
            }
        }

        // 校验徽章密码
        {
            if (!OtherData.s_hasCheckSecondPSW)
            {
                CheckSecondPSWPanelScript.create();

                return;
            }
        }
        
        buy();
    }

    public void buy()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "buy"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "buy", null, null);
            return;
        }

        int totalPrice = 0;
        totalPrice = m_medalDuiHuanRewardData.price * m_goods_num;

        if (UserData.medal < totalPrice)
        {
            ToastScript.createToast("徽章不足,无法兑换");
            return;
        }

        if (UserData.vipLevel < m_medalDuiHuanRewardData.vipLevel)
        {
            ToastScript.createToast("贵族等级不足，无法兑换");
            return;
        }

        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<MedalDuiHuanRequest>().goods_id = m_medalDuiHuanRewardData.goods_id;
            LogicEnginerScript.Instance.GetComponent<MedalDuiHuanRequest>().num = m_goods_num;
            LogicEnginerScript.Instance.GetComponent<MedalDuiHuanRequest>().CallBack = onReceive_BuyGoods;
            LogicEnginerScript.Instance.GetComponent<MedalDuiHuanRequest>().OnRequest();
        }
    }

    public void onReceive_BuyGoods(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("MedalDuiHuanQueRenPanelScript_hotfix", "onReceive_BuyGoods"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.MedalDuiHuanQueRenPanelScript_hotfix", "onReceive_BuyGoods", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);
        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            ToastScript.createToast("兑换成功");

            string reward = (string)jd["reward"];

            // 扣除徽章
            {
                int totalPrice = m_medalDuiHuanRewardData.price * m_goods_num;
                GameUtil.changeData(110, -totalPrice);

                OtherData.s_medalDuiHuanPanelScript.m_text_myMedalNum.text = UserData.medal.ToString();
            }

            // 奖励加到内存
            {
                List<string> list1 = new List<string>();
                CommonUtil.splitStr(reward, list1, ';');
                for (int i = 0; i < list1.Count; i++)
                {
                    List<string> list2 = new List<string>();
                    CommonUtil.splitStr(list1[i], list2, ':');

                    int prop_id = int.Parse(list2[0]);
                    int prop_num = int.Parse(list2[1]);

                    GameUtil.changeData(prop_id, prop_num);
                }
            }

            // 增加兑换记录
            {
                MedalDuiHuanRecordDataContent temp = new MedalDuiHuanRecordDataContent();
                temp.goods_id = m_medalDuiHuanRewardData.goods_id;
                temp.num = m_goods_num;

                string year = CommonUtil.getCurYear().ToString();
                string month = CommonUtil.getCurMonth().ToString();
                if (month.Length == 1)
                {
                    month = ("0" + month);
                }

                string day = CommonUtil.getCurDay().ToString();
                if (day.Length == 1)
                {
                    day = ("0" + day);
                }
                temp.time = year + "年" + month + "月" + day + "日";
                MedalDuiHuanRecordData.getInstance().getDataList().Insert(0, temp);
            }

            for (int i = 0; i < m_goods_num; i++)
            {
                ShowRewardPanelScript.Show(reward, false);
            }

            Destroy(gameObject);
        }
        else
        {
            string msg = (string)jd["msg"];
            ToastScript.createToast(msg);
        }
    }
}
