using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign30PanelScript : MonoBehaviour {
    public GameObject m_obj_leiji1;
    public GameObject m_obj_leiji2;
    public GameObject m_obj_leiji3;
    public GameObject m_obj_leiji4;

    public Text m_text_lianxuqiandaotianshu;
    public Text m_text_time;

    public Button m_btn_sign;

    public GameObject m_obj_meiri_reward1;
    public GameObject m_obj_meiri_reward2;

    public int m_curChoiceId;
    public int m_curMonthdays;      // 本月天数

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/Sign30Panel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    // Use this for initialization
    void Start()
    {
        OtherData.s_sign30PanelScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "Start", null, null);
            return;
        }

        if (Sign30Data.getInstance().getSign30DataContentList().Count == 0)
        {
            Debug.Log("签到奖励配置表未赋值");
            return;
        }

        initUI();
    }

    // Update is called once per frame
    void Update() {

    }

    public void initUI()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "initUI"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "initUI", null, null);
            return;
        }

        // 年月
        m_text_time.text = CommonUtil.getCurYear() + "年" + CommonUtil.getCurMonth() + "月";

        // 本月天数
        m_curMonthdays = CommonUtil.getCurMonthAllDays();

        // 显示当天签到奖励
        {
            int day = CommonUtil.getCurDay();

            for (int i = 0; i < Sign30Data.getInstance().getSign30DataContentList().Count; i++)
            {
                if (Sign30Data.getInstance().getSign30DataContentList()[i].type == 1)
                {
                    if (Sign30Data.getInstance().getSign30DataContentList()[i].day == day)
                    {
                        m_curChoiceId = Sign30Data.getInstance().getSign30DataContentList()[i].id;
                        showCurDayReward(Sign30Data.getInstance().getSign30DataContentList()[i].reward_prop);
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < m_curMonthdays; i++)
        {
            GameObject pre = Resources.Load("Prefabs/UI/Item/Item_Sign30") as GameObject;
            GameObject obj = Instantiate(pre);
            obj.transform.name = i.ToString();
            obj.transform.SetParent(gameObject.transform.Find("Image_bg"));
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<Button>().onClick.AddListener(() => onClickItemDay(obj));

            {
                Vector2 vec2_firstPos = new Vector2(-394, 168);
                int jiange = 85;

                float x = vec2_firstPos.x + (i % 7) * jiange;
                float y = vec2_firstPos.y - (i / 7) * jiange;
                obj.transform.localPosition = new Vector3(x, y, 1);
            }

            // 奖励icon
            {
                int prop_id = GameUtil.getPropIdFromReward(Sign30Data.getInstance().getSign30DataContentList()[i].reward_prop);
                CommonUtil.setImageSprite(obj.transform.Find("Image_icon").GetComponent<Image>(), GameUtil.getPropIconPath(prop_id));
            }

            // 第几天
            obj.transform.Find("Text_day").GetComponent<Text>().text = (i + 1).ToString();

            // 当天的做一些其他处理
            if ((i + 1) == CommonUtil.getCurDay())
            {
                // 当天的方块专门设置一个颜色
                CommonUtil.setImageColor(obj.GetComponent<Image>(), 255, 253, 113);

                setBtnSignState(CommonUtil.getCurDay());
            }

            // 签到状态
            {
                for (int j = 0; j < Sign30Data.getInstance().getSign30DataContentList().Count; j++)
                {
                    int day = Sign30Data.getInstance().getSign30DataContentList()[j].day;

                    if (day == (i + 1))
                    {
                        bool isSigned = false;

                        for (int k = 0; k < Sign30RecordData.getInstance().getSign30RecordList().Count; k++)
                        {
                            // 已签
                            if (Sign30RecordData.getInstance().getSign30RecordList()[k] == day)
                            {
                                isSigned = true;

                                //obj.transform.Find("Image_buqian").localScale = new Vector3(0, 0, 0);
                                obj.transform.Find("Image_yiguoqi").localScale = new Vector3(0, 0, 0);
                                obj.transform.Find("Image_icon").localScale = new Vector3(0, 0, 0);
                                break;
                            }
                        }

                        // 过去
                        if (day < CommonUtil.getCurDay())
                        {
                            if (!isSigned)
                            {
                                obj.transform.Find("Image_yiqian").localScale = new Vector3(0, 0, 0);

                                // 如果是今天没有签到的话，不显示补签
                                if (day == CommonUtil.getCurDay())
                                {
                                    //obj.transform.Find("Image_buqian").localScale = new Vector3(0, 0, 0);
                                    obj.transform.Find("Image_yiguoqi").localScale = new Vector3(0, 0, 0);
                                }
                            }
                        }
                        // 当天
                        else if (day == CommonUtil.getCurDay())
                        {
                            if (isSigned)
                            {
                                //obj.transform.Find("Image_buqian").localScale = new Vector3(0, 0, 0);
                                obj.transform.Find("Image_yiguoqi").localScale = new Vector3(0, 0, 0);
                            }
                            else
                            {
                                obj.transform.Find("Image_yiqian").localScale = new Vector3(0, 0, 0);
                                //obj.transform.Find("Image_buqian").localScale = new Vector3(0, 0, 0);
                                obj.transform.Find("Image_yiguoqi").localScale = new Vector3(0, 0, 0);
                            }
                        }
                        // 未来
                        else
                        {
                            obj.transform.Find("Image_yiqian").localScale = new Vector3(0, 0, 0);
                            //obj.transform.Find("Image_buqian").localScale = new Vector3(0, 0, 0);
                            obj.transform.Find("Image_yiguoqi").localScale = new Vector3(0, 0, 0);
                        }
                    }
                }
            }
        }

        // 连续累计签到天数显示
        m_text_lianxuqiandaotianshu.text = "连续签到天数：" + Sign30RecordData.getInstance().getLianXuSignDays().ToString() + "天";

        // 累计签到奖励
        setLeiJiSignState();
    }

    public void setLeiJiSignState()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "setLeiJiSignState"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "setLeiJiSignState", null, null);
            return;
        }

        int signAllDays = Sign30RecordData.getInstance().getLianXuSignDays();
        
        // 全勤奖
        if (signAllDays == CommonUtil.getCurMonthAllDays())
        {
            bool isGet = Sign30RecordData.getInstance().isLeiJiSignTheDay(35);

            // 达成已领取
            if (isGet)
            {
                CommonUtil.setImageSprite(m_obj_leiji4.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
            }
            // 达成未领取
            else
            {
            }

            CommonUtil.setImageSprite(m_obj_leiji1.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
            CommonUtil.setImageSprite(m_obj_leiji2.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
            CommonUtil.setImageSprite(m_obj_leiji3.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
        }
        else
        {
            CommonUtil.setImageSprite(m_obj_leiji4.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");

            if (signAllDays >= Sign30Data.getInstance().getSign30DataById(34).day)
            {
                bool isGet = Sign30RecordData.getInstance().isLeiJiSignTheDay(34);

                // 达成已领取
                if (isGet)
                {
                    CommonUtil.setImageSprite(m_obj_leiji3.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                }
                // 达成未领取
                else
                {
                }

                CommonUtil.setImageSprite(m_obj_leiji1.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji2.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji4.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
            }
            else if (signAllDays >= Sign30Data.getInstance().getSign30DataById(33).day)
            {
                bool isGet = Sign30RecordData.getInstance().isLeiJiSignTheDay(33);

                // 达成已领取
                if (isGet)
                {
                    CommonUtil.setImageSprite(m_obj_leiji2.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                }
                // 达成未领取
                else
                {
                }

                CommonUtil.setImageSprite(m_obj_leiji1.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji3.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji4.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
            }
            else if (signAllDays >= Sign30Data.getInstance().getSign30DataById(32).day)
            {
                bool isGet = Sign30RecordData.getInstance().isLeiJiSignTheDay(32);

                // 达成已领取
                if (isGet)
                {
                    CommonUtil.setImageSprite(m_obj_leiji1.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                }
                // 达成未领取
                else
                {
                }

                CommonUtil.setImageSprite(m_obj_leiji2.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji3.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji4.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
            }
            else
            {
                CommonUtil.setImageSprite(m_obj_leiji1.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji2.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji3.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
                CommonUtil.setImageSprite(m_obj_leiji4.transform.Find("Button").GetComponent<Image>(), "Sprites/Sign30/lihe_bukelingqu");
            }
        }
        
        m_obj_leiji1.transform.Find("Text").GetComponent<Text>().text = "连续签到" + Sign30Data.getInstance().getSign30DataById(32).day.ToString() + "天";
        m_obj_leiji2.transform.Find("Text").GetComponent<Text>().text = "连续签到" + Sign30Data.getInstance().getSign30DataById(33).day.ToString() + "天";
        m_obj_leiji3.transform.Find("Text").GetComponent<Text>().text = "连续签到" + Sign30Data.getInstance().getSign30DataById(34).day.ToString() + "天";
        m_obj_leiji4.transform.Find("Text").GetComponent<Text>().text = "全勤奖励";
    }

    public void onReceive_Sign30(string result)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "onReceive_Sign30"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "onReceive_Sign30", null, result);
            return;
        }

        NetLoading.getInstance().Close();

        JsonData jd = JsonMapper.ToObject(result);

        int code = (int)jd["code"];
        int type = (int)jd["type"];

        if (code == (int)TLJCommon.Consts.Code.Code_OK)
        {
            int id = (int)jd["id"];
            string reward_prop = (string)jd["reward_prop"];

            // 奖励加到内存
            {
                List<string> list1 = new List<string>();
                CommonUtil.splitStr(reward_prop, list1, ';');
                for (int i = 0; i < list1.Count; i++)
                {
                    List<string> list2 = new List<string>();
                    CommonUtil.splitStr(list1[i], list2, ':');

                    int prop_id = int.Parse(list2[0]);
                    int prop_num = int.Parse(list2[1]);

                    GameUtil.changeData(prop_id, prop_num);
                }
            }

            ShowRewardPanelScript.Show(reward_prop, false);

            switch (type)
            {
                // 今天签到
                case 1:
                    {
                        ToastScript.createToast("签到成功");

                        {
                            Sign30RecordData.getInstance().getSign30RecordList().Add(id);

                            m_text_lianxuqiandaotianshu.text = "连续签到天数：" + Sign30RecordData.getInstance().getLianXuSignDays().ToString() + "天";

                            GameObject obj = transform.Find("Image_bg/" + (id - 1).ToString()).gameObject;
                            obj.transform.Find("Image_yiqian").localScale = new Vector3(1, 1, 1);
                            //obj.transform.Find("Image_buqian").localScale = new Vector3(0, 0, 0);
                            obj.transform.Find("Image_yiguoqi").localScale = new Vector3(0, 0, 0);
                            obj.transform.Find("Image_icon").localScale = new Vector3(0, 0, 0);

                            OtherData.s_mainScript.checkRedPoint();

                            setBtnSignState(Sign30Data.getInstance().getSign30DataById(id).day);
                            setLeiJiSignState();
                        }
                    }
                    break;

                // 补签
                case 2:
                    {
                        ToastScript.createToast("补签成功");

                        {
                            Sign30RecordData.getInstance().getSign30RecordList().Add(id);

                            m_text_lianxuqiandaotianshu.text = "连续签到天数：" + Sign30RecordData.getInstance().getLianXuSignDays().ToString() + "天";

                            GameObject obj = transform.Find("Image_bg/" + (id - 1).ToString()).gameObject;
                            obj.transform.Find("Image_yiqian").localScale = new Vector3(1, 1, 1);
                            //obj.transform.Find("Image_buqian").localScale = new Vector3(0, 0, 0);

                            obj.transform.Find("Image_icon").localScale = new Vector3(0, 0, 0);
                        }

                        Destroy(OtherData.s_buQianQueRenPanelScript.gameObject);

                        // 扣除补签费
                        GameUtil.changeData(1, -OtherData.s_buQianQueRenPanelScript.getBuQianGoldHuaFei());

                        // 增加补签次数
                        ++Sign30RecordData.getInstance().m_curMonthBuQianCount;

                        setBtnSignState(Sign30Data.getInstance().getSign30DataById(id).day);
                    }
                    break;

                // 累计签到奖励
                case 3:
                    {
                        Destroy(OtherData.s_sign30LeiJiPanelScript.gameObject);

                        Sign30RecordData.getInstance().getSign30LeiJiRecordList().Add(id);
                        setLeiJiSignState();

                        ToastScript.createToast("领取奖励成功");
                    }
                    break;
            }
        }
        else
        {
            string msg = (string)jd["msg"];
            ToastScript.createToast(msg);
        }
    }

    public void onClickItemDay(GameObject obj)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "onClickItemDay"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "onClickItemDay", null, obj);
            return;
        }

        int day = int.Parse(obj.transform.name) + 1;

        for (int i = 0; i < m_curMonthdays; i++)
        {
            if (Sign30Data.getInstance().getSign30DataContentList()[i].type == 1)
            {
                // 选中的那天的方块设置专门的颜色
                if (Sign30Data.getInstance().getSign30DataContentList()[i].day == day)
                {
                    CommonUtil.setImageColor(transform.Find("Image_bg/" + i.ToString()).GetComponent<Image>(), 255, 253, 113);

                    m_curChoiceId = Sign30Data.getInstance().getSign30DataContentList()[i].id;
                    showCurDayReward(Sign30Data.getInstance().getSign30DataContentList()[i].reward_prop);
                }
                else
                {
                    CommonUtil.setImageColor(transform.Find("Image_bg/" + i.ToString()).GetComponent<Image>(), 255, 255, 255);
                }
            }
        }

        setBtnSignState(day);
    }

    public void onClickSign()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "onClickSign"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "onClickSign", null, null);
            return;
        }

        // 请求签到
        {
            Sign30DataContent temp = Sign30Data.getInstance().getSign30DataById(m_curChoiceId);

            if (temp.type == 1)
            {
                // 补签
                if (temp.day < CommonUtil.getCurDay())
                {
                    // 显示补签确认界面
                    BuQianQueRenPanelScript.create();
                    
                    return;
                }
            }
        }

        reqSign(m_curChoiceId);
    }

    public void reqSign(int id)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "reqSign"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "reqSign", null, id);
            return;
        }

        NetLoading.getInstance().Show();

        // 请求签到
        {
            LogicEnginerScript.Instance.GetComponent<Sign30Request>().CallBack = onReceive_Sign30;

            Sign30DataContent temp = Sign30Data.getInstance().getSign30DataById(id);

            LogicEnginerScript.Instance.GetComponent<Sign30Request>().m_id = temp.id;

            // 签到、补签
            if (temp.type == 1)
            {
                // 正常签到
                if (temp.day == CommonUtil.getCurDay())
                {
                    LogicEnginerScript.Instance.GetComponent<Sign30Request>().m_type = 1;
                }
                // 补签
                else if (temp.day < CommonUtil.getCurDay())
                {
                    LogicEnginerScript.Instance.GetComponent<Sign30Request>().m_type = 2;
                }
            }
            // 累计签到奖励
            else if (temp.type == 2)
            {
                LogicEnginerScript.Instance.GetComponent<Sign30Request>().m_type = 3;
            }

            LogicEnginerScript.Instance.GetComponent<Sign30Request>().OnRequest();
        }
    }

    public void onClickBox1()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "onClickBox1"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "onClickBox1", null, null);
            return;
        }

        int count = 0;
        for (int i = 0; i < Sign30Data.getInstance().getSign30DataContentList().Count; i++)
        {
            if (Sign30Data.getInstance().getSign30DataContentList()[i].type == 2)
            {
                ++count;

                if (count == 1)
                {
                    Sign30LeiJiPanelScript.create(Sign30Data.getInstance().getSign30DataContentList()[i].id);
                    break;
                }
            }
        }
    }

    public void onClickBox2()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "onClickBox2"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "onClickBox2", null, null);
            return;
        }

        int count = 0;
        for (int i = 0; i < Sign30Data.getInstance().getSign30DataContentList().Count; i++)
        {
            if (Sign30Data.getInstance().getSign30DataContentList()[i].type == 2)
            {
                ++count;

                if (count == 2)
                {
                    Sign30LeiJiPanelScript.create(Sign30Data.getInstance().getSign30DataContentList()[i].id);
                    break;
                }
            }
        }
    }

    public void onClickBox3()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "onClickBox3"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "onClickBox3", null, null);
            return;
        }

        int count = 0;
        for (int i = 0; i < Sign30Data.getInstance().getSign30DataContentList().Count; i++)
        {
            if (Sign30Data.getInstance().getSign30DataContentList()[i].type == 2)
            {
                ++count;

                if (count == 3)
                {
                    Sign30LeiJiPanelScript.create(Sign30Data.getInstance().getSign30DataContentList()[i].id);
                    break;
                }
            }
        }
    }

    public void onClickBox4()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "onClickBox4"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "onClickBox4", null, null);
            return;
        }

        int count = 0;
        for (int i = 0; i < Sign30Data.getInstance().getSign30DataContentList().Count; i++)
        {
            if (Sign30Data.getInstance().getSign30DataContentList()[i].type == 2)
            {
                ++count;

                if (count == 4)
                {
                    Sign30LeiJiPanelScript.create(Sign30Data.getInstance().getSign30DataContentList()[i].id);
                    break;
                }
            }
        }
    }

    public void setBtnSignState(int day)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "setBtnSignState"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "setBtnSignState", null, day);
            return;
        }

        // 未来
        if (day > CommonUtil.getCurDay())
        {
            CommonUtil.setButtonEnable(m_btn_sign, false);
            CommonUtil.setImageSprite(m_btn_sign.transform.Find("Image").GetComponent<Image>(), "Sprites/Sign30/wz_qiandao");
        }
        // 当天
        else if (day == CommonUtil.getCurDay())
        {
            bool isSign = false;
            for (int i = 0; i < Sign30RecordData.getInstance().getSign30RecordList().Count; i++)
            {
                if (Sign30RecordData.getInstance().getSign30RecordList()[i] == day)
                {
                    isSign = true;

                    break;
                }
            }

            // 已签
            if (isSign)
            {
                CommonUtil.setButtonEnable(m_btn_sign, false);
                CommonUtil.setImageSprite(m_btn_sign.transform.Find("Image").GetComponent<Image>(), "Sprites/Sign30/wz_qiandao");
            }
            // 未签
            else
            {
                CommonUtil.setButtonEnable(m_btn_sign, true);
                CommonUtil.setImageSprite(m_btn_sign.transform.Find("Image").GetComponent<Image>(), "Sprites/Sign30/wz_qiandao");
            }
        }
        // 过去
        else
        {
            bool isSign = false;
            for (int i = 0; i < Sign30RecordData.getInstance().getSign30RecordList().Count; i++)
            {
                if (Sign30RecordData.getInstance().getSign30RecordList()[i] == day)
                {
                    isSign = true;

                    break;
                }
            }
            
            // 已签
            if (isSign)
            {
                CommonUtil.setButtonEnable(m_btn_sign, false);
                CommonUtil.setImageSprite(m_btn_sign.transform.Find("Image").GetComponent<Image>(), "Sprites/Sign30/wz_qiandao");
            }
            // 补签
            else
            {
                CommonUtil.setButtonEnable(m_btn_sign, false);
                CommonUtil.setImageSprite(m_btn_sign.transform.Find("Image").GetComponent<Image>(), "Sprites/Sign30/wz_qiandao");

                //CommonUtil.setButtonEnable(m_btn_sign, true);
                //CommonUtil.setImageSprite(m_btn_sign.transform.Find("Image").GetComponent<Image>(),"Sprites/Sign30/wz_buqian");
            }
        }
    }

    public void showCurDayReward(string reward)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Sign30PanelScript_hotfix", "showCurDayReward"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Sign30PanelScript_hotfix", "showCurDayReward", null, reward);
            return;
        }

        List<string> list1 = new List<string>();
        CommonUtil.splitStr(reward, list1, ':');

        int prop_id = int.Parse(list1[0]);
        int prop_num = int.Parse(list1[1]);

        CommonUtil.setImageSprite(m_obj_meiri_reward1.transform.Find("Image").GetComponent<Image>(),GameUtil.getPropIconPath(prop_id));
        m_obj_meiri_reward1.transform.Find("Text").GetComponent<Text>().text = prop_num.ToString();
    }
}
