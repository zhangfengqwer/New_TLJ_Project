using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurntablePanelScript : MonoBehaviour
{
    public GameObject m_listView;
    ListViewScript m_ListViewScript;

    public Image m_image_neiyuan;
    public Image m_image_deng1;
    public Image m_image_deng2;

    public Button m_button_free;
    public Button m_button_huizhang;

    List<GameObject> m_rewardObj_list = new List<GameObject>();

    bool m_isStartRotate = false;
    int m_crossCount = 0;
    float m_rotateSpeed = 200;
    GameObject m_targetGameObject;

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
        m_ListViewScript = m_listView.GetComponent<ListViewScript>();

        m_button_free.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.freeCount.ToString();
        m_button_huizhang.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.huizhangCount.ToString();

        // 获取转盘数据
        {
            LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = onReceive_GetTurntable;
            LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().OnRequest();
        }

        InvokeRepeating("onInvokeDeng",0.5f,0.5f);
    }

    private void Update()
    {
        if (m_isStartRotate)
        {
            if ((int)m_targetGameObject.transform.eulerAngles.z == 0)
            {
                ++m_crossCount;
                if (m_crossCount == 3)
                {
                    m_isStartRotate = false;
                    m_crossCount = 0;
                    m_rotateSpeed = 200;

                    // 显示奖励
                    ShowRewardPanelScript.Show(TurntableDataScript.getInstance().getDataById(int.Parse(m_targetGameObject.transform.name)).m_reward);

                    // 显示在转盘通知列表
                    addTurntableBroadcast(UserData.name, int.Parse(m_targetGameObject.transform.name));
                }
            }

            if (m_crossCount == 2)
            {
                m_rotateSpeed -= 0.4f;
                if (m_rotateSpeed < 10.0f)
                {
                    m_rotateSpeed = 10.0f;
                }
            }

            float speed = Time.deltaTime * m_rotateSpeed / (1.0f / 60.0f);
            m_image_neiyuan.transform.Rotate(new Vector3(0, 0, -speed / 100.0f));
        }
    }

    void onInvokeDeng()
    {
        if (m_image_deng1.transform.localScale.x == 0)
        {
            m_image_deng1.transform.localScale = new Vector3(1,1,1);
            m_image_deng2.transform.localScale = new Vector3(0,0,0);
        }
        else
        {
            m_image_deng1.transform.localScale = new Vector3(0,0,0);
            m_image_deng2.transform.localScale = new Vector3(1,1,1);
        }
    }

    void loadReward()
    {
        m_rewardObj_list.Clear();

        for (int i = 0; i < TurntableDataScript.getInstance().getDataList().Count; i++)
        {
            GameObject prefab = Resources.Load("Prefabs/UI/Other/Turntable_reward") as GameObject;
            GameObject obj = Instantiate(prefab, m_image_neiyuan.transform);
            obj.transform.localRotation = Quaternion.Euler(0,0,(-36 * i) - 18);

            m_rewardObj_list.Add(obj);

            // 具体数据
            {
                int id = TurntableDataScript.getInstance().getDataList()[i].m_id;
                obj.transform.name = id.ToString();

                string reward = TurntableDataScript.getInstance().getDataList()[i].m_reward;
                List<string> list = new List<string>();
                CommonUtil.splitStr(reward,list,':');
                int prop_id = int.Parse(list[0]);
                int prop_num = int.Parse(list[1]);

                PropInfo propInfo = PropData.getInstance().getPropInfoById(prop_id);

                // 图标
                CommonUtil.setImageSprite(obj.transform.Find("Image_icon").GetComponent<Image>(), "Sprites/Icon/Prop/" + propInfo.m_icon);
                
                // 名称
                obj.transform.Find("Text_name").GetComponent<Text>().text = propInfo.m_name;

                // 数量
                obj.transform.Find("Text_num").GetComponent<Text>().text = prop_num.ToString();
            }

            if (i % 2 != 0)
            {
                obj.transform.Find("Text_name").GetComponent<Text>().color = Color.white;
                obj.transform.Find("Text_num").GetComponent<Text>().color = Color.white;
            }
        }
    }

    // 获取转盘数据
    void onReceive_GetTurntable(string data)
    {
        TurntableDataScript.getInstance().initJson(data);
        loadReward();
    }

    // 使用转盘
    void onReceive_UseTurntable(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);

        int code = (int)jd["code"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            int reward_id = (int)jd["reward_id"];
            int type = (int)jd["type"];
            int subHuiZhangNum = (int)jd["subHuiZhangNum"];

            GameUtil.changeData((int)TLJCommon.Consts.Prop.Prop_huizhang,subHuiZhangNum);

            UserData.myTurntableData = JsonMapper.ToObject<MyTurntableData>(jd["turntableData"].ToString());

            // 刷新UI次数
            {
                m_button_free.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.freeCount.ToString();
                m_button_huizhang.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.huizhangCount.ToString();
            }

            // 轮盘开始转
            {
                for (int i = 0; i < m_rewardObj_list.Count; i++)
                {
                    if (m_rewardObj_list[i].transform.name.CompareTo(reward_id.ToString()) == 0)
                    {
                        m_targetGameObject = m_rewardObj_list[i];

                        m_isStartRotate = true;
                    }
                }
            }
        }
        else
        {
            ToastScript.createToast("使用失败");
        }
    }

    public void onReceive_TurntableBroadcast(string data)
    {
        JsonData jd = JsonMapper.ToObject(data);

        {
            string name = (string)jd["name"];
            int reward_id = (int)jd["reward_id"];

            if (UserData.name.CompareTo(name) != 0)
            {
                addTurntableBroadcast(name, reward_id);
            }
        }
    }

    public void addTurntableBroadcast(string name,int reward_id)
    {
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

                        string reward = TurntableDataScript.getInstance().getDataById(temp.m_reward_id).m_reward;
                        int prop_id = CommonUtil.splitStr_Start(reward, ':');
                        int prop_num = CommonUtil.splitStr_End(reward, ':');
                        string prop_name = PropData.getInstance().getPropInfoById(prop_id).m_name;

                        string content = "恭喜" + temp.m_name + "获得" + prop_name + "*" + prop_num;
                        obj.transform.Find("Text").GetComponent<Text>().text = content;
                    }

                    m_ListViewScript.addItem(obj);
                }

                m_ListViewScript.addItemEnd();
            }
        }
        catch (Exception ex)
        {
            LogUtil.Log("addTurntableBroadcast----" + ex.Message);
        }
    }

    private void OnDestroy()
    {
        //LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = null;
        s_instance = null;
    }

    public void onClickFree()
    {
        if (m_isStartRotate)
        {
            return;
        }

        // 使用转盘
        {
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().CallBack = onReceive_UseTurntable;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().type = 1;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().OnRequest();
        }
    }

    public void onClickHuiZhang()
    {
        if (m_isStartRotate)
        {
            return;
        }

        // 使用转盘
        {
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().CallBack = onReceive_UseTurntable;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().type = 2;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().OnRequest();
        }
    }

    public void onClickFree_tip()
    {
        string tip = "1、每进行一局游戏可获得一次抽奖机会（每日每人可获得三次抽奖机会哦~）。\r\n2、升级vip等级获得贵族特权，即可增加抽奖机会。\r\n3、使用徽章进行抽奖，每日可获得三次抽奖机会。";
        TurntableTipPanelScript.create().GetComponent<TurntableTipPanelScript>().setTip(tip);
    }

    public void onClickHuiZhang_tip()
    {
        string tip = "1、每进行一局游戏可获得一次抽奖机会（每日每人可获得三次抽奖机会哦~）。\r\n2、升级vip等级获得贵族特权，即可增加抽奖机会。\r\n3、使用徽章进行抽奖，每日可获得三次抽奖机会。";
        TurntableTipPanelScript.create().GetComponent<TurntableTipPanelScript>().setTip(tip);
    }
}
