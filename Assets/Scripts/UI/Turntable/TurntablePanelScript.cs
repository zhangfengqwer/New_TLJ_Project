using DG.Tweening;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurntablePanelScript : MonoBehaviour
{
    public GameObject m_listView;
    public ListViewScript m_ListViewScript;

    public Button m_button_free;
    public Button m_button_huizhang;

    public Text m_text_myLuckyValue;

    public List<GameObject> m_rewardObj_list = new List<GameObject>();

    public GameObject m_curInGameObject;
    public int m_maxRunRotateCount = 3;
    public int m_curRunRotateCount = 0;
    public bool m_isStartRotate = false;
    public GameObject m_targetGameObject;

    public static GameObject s_instance = null;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TurntablePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);
        s_instance = obj;

        return obj;
    }

    void Start()
    {
        OtherData.s_turntablePanelScript = this;

        initUI_Image();

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "Start", null, null);
            return;
        }

        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        m_button_free.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.freeCount.ToString();
        m_button_huizhang.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.huizhangCount.ToString();

        m_text_myLuckyValue.text = UserData.myTurntableData.luckyValue.ToString();

        // 获取转盘数据
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = onReceive_GetTurntable;
            LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().OnRequest();
        }
    }

    public void initUI_Image()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "initUI_Image"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "initUI_Image", null, null);
            return;
        }

        CommonUtil.setImageSpriteByAssetBundle(gameObject.transform.Find("Image_bg/Imagebg").GetComponent<Image>(), "turntable.unity3d", "bg");
    }

    private void Update()
    {
    }

    public void loadReward()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "loadReward"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "loadReward", null, null);
            return;
        }

        m_rewardObj_list.Clear();

        for (int i = 0; i < TurntableDataScript.getInstance().getDataList().Count; i++)
        {
            GameObject obj = gameObject.transform.Find("Image_bg/Left/Reward/Item" + (i + 1).ToString()).gameObject;

            m_rewardObj_list.Add(obj);

            // 具体数据
            {
                TurntableData temp = TurntableDataScript.getInstance().getDataList()[i];

                int id = temp.m_id;
                obj.transform.name = id.ToString();

                string reward = temp.m_reward;
                List<string> list = new List<string>();
                CommonUtil.splitStr(reward, list, ':');
                int prop_id = int.Parse(list[0]);
                int prop_num = int.Parse(list[1]);

                // 图标
                CommonUtil.setImageSprite(obj.transform.Find("Image_icon").GetComponent<Image>(), GameUtil.getPropIconPath(prop_id));

                // 数量
                obj.transform.Find("Text_num").GetComponent<Text>().text = prop_num.ToString();

                // 徽章角标
                if (temp.m_isHuiZhang)
                {
                    obj.transform.Find("Image_huizhangjiaobiao").localScale = new Vector3(1,1,1);
                }
            }
        }
    }

    // 获取转盘数据
    public void onReceive_GetTurntable(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onReceive_GetTurntable"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onReceive_GetTurntable", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        TurntableDataScript.getInstance().initJson(data);
        loadReward();
    }

    // 使用转盘
    public void onReceive_UseTurntable(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onReceive_UseTurntable"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onReceive_UseTurntable", null, data);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(data);

        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            m_isStartRotate = true;

            int reward_id = (int)jd["reward_id"];
            int type = (int)jd["type"];
            int subHuiZhangNum = (int)jd["subHuiZhangNum"];

            GameUtil.changeData((int)TLJCommon.Consts.Prop.Prop_huizhang,-subHuiZhangNum);

            {
                UserData.myTurntableData = JsonMapper.ToObject<MyTurntableData>(jd["turntableData"].ToString());

                // 刷新UI次数
                {
                    m_button_free.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.freeCount.ToString();
                    m_button_huizhang.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.huizhangCount.ToString();
                }

                // 刷新幸运值
                {
                    m_text_myLuckyValue.text = UserData.myTurntableData.luckyValue.ToString();
                }
            }

            // 开始旋转前先禁止此界面可被关闭
            gameObject.GetComponent<ScaleUtil>().setCanClose(false);

            // 轮盘开始转
            {
                for (int i = 0; i < m_rewardObj_list.Count; i++)
                {
                    if (m_rewardObj_list[i].transform.name.CompareTo(reward_id.ToString()) == 0)
                    {
                        m_targetGameObject = m_rewardObj_list[i];
                        startRunRotate();

                        break;
                    }
                }
            }
        }
        else
        {
            ToastScript.createToast("使用失败");
        }
    }

    public void startRunRotate()
    {
        m_curRunRotateCount = 0;
        
        m_curInGameObject = m_rewardObj_list[0];

        for (int i = 0; i < m_rewardObj_list.Count; i++)
        {
            if (m_curInGameObject.name.CompareTo(m_rewardObj_list[i].name) == 0)
            {
                CommonUtil.setImageSpriteByAssetBundle(m_rewardObj_list[i].GetComponent<Image>(), "turntable.unity3d", "zj");
            }
            else
            {
                CommonUtil.setImageSpriteByAssetBundle(m_rewardObj_list[i].GetComponent<Image>(), "turntable.unity3d", "wzj");
            }
        }

        InvokeRepeating("onInvokeRunRotate",0.1f,0.1f);
    }

    public void onInvokeRunRotate()
    {
        int index = m_rewardObj_list.IndexOf(m_curInGameObject);
        if (index == (m_rewardObj_list.Count - 1))
        {
            index = 0;
        }
        else
        {
            ++index;
        }

        m_curInGameObject = m_rewardObj_list[index];

        for (int i = 0; i < m_rewardObj_list.Count; i++)
        {
            if (m_curInGameObject.name.CompareTo(m_rewardObj_list[i].name) == 0)
            {
                CommonUtil.setImageSpriteByAssetBundle(m_rewardObj_list[i].GetComponent<Image>(), "turntable.unity3d", "zj");
            }
            else
            {
                CommonUtil.setImageSpriteByAssetBundle(m_rewardObj_list[i].GetComponent<Image>(), "turntable.unity3d", "wzj");
            }
        }

        if (m_curInGameObject.name.CompareTo(m_targetGameObject.name) == 0)
        {
            if (++m_curRunRotateCount == m_maxRunRotateCount)
            {
                CancelInvoke("onInvokeRunRotate");

                Invoke("onInvokeRunRotateEnd",1);
            }
        }
    }

    public void onInvokeRunRotateEnd()
    {
        {
            string reward = TurntableDataScript.getInstance().getDataById(int.Parse(m_targetGameObject.transform.name)).m_reward;

            // 加到内存
            GameUtil.changeDataWithStr(reward);

            // 显示奖励
            ShowRewardPanelScript.Show(reward, true);

            // 显示在转盘通知列表
            addTurntableBroadcast(UserData.name, int.Parse(m_targetGameObject.transform.name));

            m_isStartRotate = false;

            // 旋转结束后允许此界面可被关闭
            gameObject.GetComponent<ScaleUtil>().setCanClose(true);
        }
    }

    public void onReceive_TurntableBroadcast(string data)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onReceive_TurntableBroadcast"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onReceive_TurntableBroadcast", null, data);
            return;
        }

        JsonData jd = JsonMapper.ToObject(data);

        {
            string name = (string)jd["name"];
            int reward_id = (int)jd["reward_id"];
            int canShowSelf = (int)jd["canShowSelf"];

            if (canShowSelf == 0)
            {
                if (UserData.name.CompareTo(name) != 0)
                {
                    addTurntableBroadcast(name, reward_id);
                }
            }
            else
            {
                addTurntableBroadcast(name, reward_id);
            }
        }
    }

    public void addTurntableBroadcast(string name,int reward_id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "addTurntableBroadcast"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "addTurntableBroadcast", null, name, reward_id);
            return;
        }

        try
        {
            TurntableBroadcastDataScript.getInstance().addData(name, reward_id);

            {
                m_ListViewScript.clear();
                for (int i = 0; i < TurntableBroadcastDataScript.getInstance().getTurntableBroadcastDataList().Count; i++)
                {
                    GameObject prefab = Resources.Load("Prefabs/UI/Item/Item_zhuanpan_guangbo") as GameObject;
                    GameObject obj = MonoBehaviour.Instantiate(prefab);

                    {
                        TurntableBroadcastData temp = TurntableBroadcastDataScript.getInstance().getTurntableBroadcastDataList()[i];

                        TurntableData data = TurntableDataScript.getInstance().getDataById(temp.m_reward_id);
                        if (data != null)
                        {
                            string reward = TurntableDataScript.getInstance().getDataById(temp.m_reward_id).m_reward;
                            int prop_id = CommonUtil.splitStr_Start(reward, ':');
                            int prop_num = CommonUtil.splitStr_End(reward, ':');
                            string prop_name = PropData.getInstance().getPropInfoById(prop_id).m_name;

                            string content = "恭喜" + temp.m_name + "获得" + prop_name + "*" + prop_num;
                            obj.transform.Find("Text").GetComponent<Text>().text = content;
                        }
                    }

                    m_ListViewScript.addItem(obj);
                }

                m_ListViewScript.addItemEnd();
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("addTurntableBroadcast异常----" + ex.Message);
        }
    }

    private void OnDestroy()
    {
        //LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = null;
        s_instance = null;
    }

    public void onClickFree()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onClickFree"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onClickFree", null, null);
            return;
        }

        if (m_isStartRotate)
        {
            return;
        }

        if (UserData.myTurntableData.freeCount == 0)
        {
            ToastScript.createToast("次数不足");

            return;
        }

        // 使用转盘
        reqUseZhuanPan(1);
    }

    public void onClickHuiZhang()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onClickHuiZhang"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onClickHuiZhang", null, null);
            return;
        }

        if (m_isStartRotate)
        {
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

        int needHuiZhangNum = 3;

        switch (UserData.myTurntableData.huizhangCount)
        {
            case 0:
            {
                ToastScript.createToast("次数不足");

                return;
            }
            break;

            case 1:
            {
                needHuiZhangNum = 10;

                //if (UserData.medal < 10)
                //{
                //    ToastScript.createToast("徽章不足");

                //    return;
                //}
            }
            break;

            case 2:
            {
                needHuiZhangNum = 5;

                //if (UserData.medal < 5)
                //{
                //    ToastScript.createToast("徽章不足");

                //    return;
                //}
            }
            break;

            case 3:
            {
                needHuiZhangNum = 3;

                //if (UserData.medal < 3)
                //{
                //    ToastScript.createToast("徽章不足");

                //    return;
                //}
            }
            break;

        }

        UseHuiZhangZhuanPanPanelScript.create().setData(this, needHuiZhangNum);
    }

    public void reqUseZhuanPan(int type)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "reqUseZhuanPan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "reqUseZhuanPan", null, type);
            return;
        }

        // 使用转盘
        {
            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().CallBack = onReceive_UseTurntable;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().type = type;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().OnRequest();
        }
    }

    public void onClickFree_tip()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onClickFree_tip"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onClickFree_tip", null, null);
            return;
        }

        if (m_isStartRotate)
        {
            return;
        }

        string tip = "1、每进行一局游戏可获得一次抽奖机会（每日每人可获得三次抽奖机会哦~）。\r\n2、升级贵族等级获得贵族特权，即可增加抽奖机会。";
        TurntableTipPanelScript.create().GetComponent<TurntableTipPanelScript>().setTip(tip);
    }

    public void onClickHuiZhang_tip()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onClickHuiZhang_tip"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onClickHuiZhang_tip", null, null);
            return;
        }

        if (m_isStartRotate)
        {
            return;
        }

        string tip = "1、使用徽章进行抽奖，每日可获得三次抽奖机会。\r\n2、通过比赛场获得胜利可赢取徽章奖励。";
        TurntableTipPanelScript.create().GetComponent<TurntableTipPanelScript>().setTip(tip);
    }

    public void onClickDuiHuan()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("TurntablePanelScript_hotfix", "onClickDuiHuan"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.TurntablePanelScript_hotfix", "onClickDuiHuan", null, null);
            return;
        }

        Destroy(gameObject);
        NoticePanelScript.create();
    }
}
