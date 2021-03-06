﻿using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WeeklySignScript : MonoBehaviour
{
    public List<GameObject> signObjects = new List<GameObject>();
    public GameObject content;
    public Button btn_Sign;
    public Image image_Signed;
    public int totalSignDays;
    public static List<SignItem> _signItems;
    public bool isSignSuccess = false;

    public GameObject go;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/WeeklySignPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start()
    {
        OtherData.s_weeklySignScript = this;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WeeklySignScript_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WeeklySignScript_hotfix", "Start", null, null);
            return;
        }

        InitData();
        InitUi();
    }

    private void Update()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WeeklySignScript_hotfix", "Update"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WeeklySignScript_hotfix", "Update", null, null);
            return;
        }

        //签到成功后，做的一些ui操作
        if (isSignSuccess)
        {
            GameObject signObject = signObjects[totalSignDays];
            var name = signObject.transform.GetChild(2);
            Transform image_lingqu;
            if (SignData.SignWeekDays == 6)
            {
                image_lingqu = signObject.transform.GetChild(3);
            }
            else
            {
                image_lingqu = signObject.transform.GetChild(4);
            }
            //            var image_prop = signObject.transform.GetChild(2);
            var guang = signObject.transform.GetChild(0);
            //取消点击事件
            signObject.GetComponent<Button>().enabled = false;

            //取消光
//            Transform child = guang.transform.GetChild(0);
//
//            Destroy(child.gameObject);

//            Color color = signObject.GetComponent<Image>().color;
//            color.r = 0.5f;
//            color.g = 0.5f;
//            color.b = 0.5f;
//            name.GetComponent<Text>().color = color;
//            image_prop.GetComponent<Image>().color = color;
//            signObject.GetComponent<Image>().color = color;

//            go.transform.localScale = Vector3.zero;

            image_lingqu.transform.localScale = Vector3.one;
            btn_Sign.transform.localScale = Vector3.zero;
            image_Signed.transform.localScale = Vector3.one;

            if (SignData.SignWeekDays == 6)
            {
                signObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Sign/item_bg_big2");
            }
            else
            {
                signObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Sign/item_bg_smail2");
            }

            SignData.IsSign = true;
            SignData.SignWeekDays++;
            isSignSuccess = false;

            SignItem signItem = _signItems[totalSignDays];
            LogUtil.Log(signItem.goods_prop);
            AddProp(signItem.goods_prop);

            //ShowRewardPanelScript.create().GetComponent<ShowRewardPanelScript>().setData(signItem.goods_prop);
            ShowRewardPanelScript.Show(signItem.goods_prop,false);

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.checkRedPoint();
            }
        }
    }

    public void AddProp(string prop)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WeeklySignScript_hotfix", "AddProp"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WeeklySignScript_hotfix", "AddProp", null, prop);
            return;
        }

        List<string> list = new List<String>();
        CommonUtil.splitStr(prop, list, ';');
        for (int i = 0; i < list.Count; i++)
        {
            string[] strings = list[i].Split(':');
            int propId = Convert.ToInt32(strings[0]);
            int propNum = Convert.ToInt32(strings[1]);
            if (propId == 1)
            {
                UserData.gold += propNum;
            }
            else if (propId == 2)
            {
                UserData.yuanbao += propNum;
            }
            else
            {
                var userPropData = new UserPropData();
                userPropData.prop_id = propId;
                userPropData.prop_num = propNum;

                for (int j = 0; j < PropData.getInstance().getPropInfoList().Count; j++)
                {
                    PropInfo propInfo = PropData.getInstance().getPropInfoList()[j];
                    if (propInfo.m_id == userPropData.prop_id)
                    {
                        userPropData.prop_icon = propInfo.m_icon;
                        userPropData.prop_name = propInfo.m_name;
                    }
                }
                UserData.propData.Add(userPropData);
            }

            if (OtherData.s_mainScript != null)
            {
                OtherData.s_mainScript.refreshUI();
                OtherData.s_mainScript.checkRedPoint();
            }
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WeeklySignScript_hotfix", "InitData"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WeeklySignScript_hotfix", "InitData", null, null);
            return;
        }

        totalSignDays = SignData.SignWeekDays;


        for (int i = 0; i < 7; i++)
        {
            Transform child = content.transform.GetChild(i);
            signObjects.Add(child.gameObject);
        }

        if (_signItems == null || _signItems.Count != signObjects.Count)
        {
            LogUtil.Log("数据初始化错误");
            return;
        }
    }

    /// <summary>
    /// 初始化ui   
    /// </summary>
    public void InitUi()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WeeklySignScript_hotfix", "InitUi"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WeeklySignScript_hotfix", "InitUi", null, null);
            return;
        }

        //签到过，按钮不可点击
        if (SignData.IsSign)
        {
            btn_Sign.transform.localScale = Vector3.zero;
            image_Signed.transform.localScale = Vector3.one;
        }
        for (int i = 0; i < signObjects.Count; i++)
        {
            GameObject Object = signObjects[i];
            SignItem signItem = _signItems[i];


            var name = Object.transform.GetChild(2);
            var image_lingqu = Object.transform.GetChild(4);
            var image_prop = Object.transform.GetChild(3);
            var guang = Object.transform.GetChild(0);


            //设置元宝等道具
            Text text1 = name.GetComponent<Text>();
            text1.text = signItem.goods_name;
            var prop = image_prop.GetComponent<Image>();
            if (i == 6)
            {
                var obj = Resources.Load<GameObject>("Prefabs/UI/Item/Item_sign_dalibao");
                Object.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Sign/item_bg_big2");
                Instantiate(obj, guang.transform);
                Destroy(prop.gameObject);
            }
            else
            {
                Object.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Sign/item_bg_smail2");
                prop.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/" + signItem.goods_icon);
            }

            //未签到
            if (SignData.IsSign == false)
            {
                if (i < totalSignDays)
                {
//                    Color color = Object.GetComponent<Image>().color;
//                    color.r = 0.5f;
//                    color.g = 0.5f;
//                    color.b = 0.5f;
//                    Object.GetComponent<Image>().color = color;
//                    text1.color = color;
//                    prop.color = color;

                    image_lingqu.transform.localScale = Vector3.one;
                }
                else
                {
                    image_lingqu.transform.localScale = Vector3.zero;
                }
                if (totalSignDays == i)
                {
                    if (i == 6)
                    {
                        Object.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Sign/Item_bg_current_big");
                    }
                    else
                    {
                        Object.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Sign/item_bg_current_smail");
                    }
                    //发光
//                    go = Resources.Load<GameObject>("Prefabs/UI/Other/Sign_guang");
//                    go.transform.localScale = Vector3.one;
//                    GameObject.Instantiate(go, guang.transform);
                    Button button = Object.AddComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                        //发送 签到请求
                        LogicEnginerScript.Instance.GetComponent<SignRequest>().CallBack = SignCallBack;
                        LogicEnginerScript.Instance.GetComponent<SignRequest>().OnRequest();
                    });
//                    Object.GetComponent<Image>().color = new Color(1, 185/(float)255,0,1);
                }
            }
            //已签到
            else
            {
                if (i < totalSignDays)
                {
//                    Color color = Object.GetComponent<Image>().color;
//                    color.r = 0.5f;
//                    color.g = 0.5f;
//                    color.b = 0.5f;
//                    Object.GetComponent<Image>().color = color;
//                    text1.color = color;
//                    prop.color = color;
                    image_lingqu.transform.localScale = Vector3.one;
                }
                else
                {
                    image_lingqu.transform.localScale = Vector3.zero;
                }
            }
        }
    }

    public void OnSignClick()
    {
    }

    public void SignCallBack(bool flag)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("WeeklySignScript_hotfix", "SignCallBack"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.WeeklySignScript_hotfix", "SignCallBack", null, flag);
            return;
        }

        isSignSuccess = flag;
        if (!flag)
        {
            LogUtil.Log("签到错误");
        }
    }
}