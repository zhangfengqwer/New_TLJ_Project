using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurntablePanelScript : MonoBehaviour
{
    public Image m_image_neiyuan;
    public Image m_image_deng1;
    public Image m_image_deng2;

    public Button m_button_free;
    public Button m_button_huizhang;

    List<GameObject> m_rewardObj_list = new List<GameObject>();

    bool m_isStartRotate = false;
    bool m_isStartChoice = false;
    bool m_isCrossBeforeGameObject = false;
    bool m_isStartJianSu = false;
    float m_rotateSpeed = 200;
    GameObject m_targetGameObject;
    GameObject m_beforeGameObject;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/TurntablePanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start()
    {
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
            m_image_neiyuan.transform.Rotate(new Vector3(0, 0, -m_rotateSpeed / 100.0f));

            if (m_isStartChoice)
            {
                if (((int)m_targetGameObject.transform.eulerAngles.z == 0) && m_isCrossBeforeGameObject)
                {
                    m_isStartRotate = false;
                    m_isStartChoice = false;
                    m_isCrossBeforeGameObject = false;
                    m_isStartJianSu = false;
                    m_rotateSpeed = 200;

                    // 显示奖励
                    ShowRewardPanelScript.Show(TurntableDataScript.getInstance().getDataById(int.Parse(m_targetGameObject.transform.name)).m_reward);
                }
                else
                {
                    if (0 == (int)m_beforeGameObject.transform.eulerAngles.z)
                    {
                        m_isStartJianSu = true;
                        m_isCrossBeforeGameObject = true;
                    }

                    if (m_isStartJianSu)
                    {
                        m_rotateSpeed -= 0.7f;

                        if (m_rotateSpeed <= 10f)
                        {
                            m_rotateSpeed = 10f;
                        }
                    }
                }
            }
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

    void onInvokeStartChoice()
    {
        m_isStartChoice = true;
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

                        if (i == 0)
                        {
                            m_beforeGameObject = m_rewardObj_list[7];
                        }
                        else if (i == 1)
                        {
                            m_beforeGameObject = m_rewardObj_list[8];
                        }
                        else if (i == 2)
                        {
                            m_beforeGameObject = m_rewardObj_list[9];
                        }
                        else
                        {
                            m_beforeGameObject = m_rewardObj_list[i - 3];
                        }

                        m_isStartRotate = true;
                        Invoke("onInvokeStartChoice", 3);
                    }
                }
            }
        }
        else
        {
            ToastScript.createToast("使用失败");
        }
    }

    private void OnDestroy()
    {
        LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = null;
    }

    public void onClickFree()
    {
        // 使用转盘
        {
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().CallBack = onReceive_UseTurntable;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().type = 1;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().OnRequest();
        }
    }

    public void onClickHuiZhang()
    {
        // 使用转盘
        {
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().CallBack = onReceive_UseTurntable;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().type = 2;
            LogicEnginerScript.Instance.GetComponent<UseTurntableRequest>().OnRequest();
        }
    }

    public void onClickFree_tip()
    {

    }

    public void onClickHuiZhang_tip()
    {

    }
}
