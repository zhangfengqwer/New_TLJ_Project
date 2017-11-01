using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class WeeklySignScript : MonoBehaviour
{
    private List<GameObject> signObjects = new List<GameObject>();
    public GameObject content;
    public Button btn_Sign;
    public Image image_Signed;
    private int totalSignDays;
    public static List<SignItem> _signItems;
    private bool isSignSuccess = false;

    public static GameObject create()
    {
        GameObject prefab = Resources.Load("Prefabs/UI/Panel/WeeklySignPanel") as GameObject;
        GameObject obj = GameObject.Instantiate(prefab, GameObject.Find("Canvas_Middle").transform);

        return obj;
    }

    void Start()
    {
        InitData();
        InitUi();

        Sign_Guang_Script.create();
    }

    private void Update()
    {
        //签到成功后，做的一些ui操作
        if (isSignSuccess)
        {
            GameObject signObject = signObjects[totalSignDays];
            var name = signObject.transform.GetChild(1);
            var image_lingqu = signObject.transform.GetChild(2);
            var image_prop = signObject.transform.GetChild(3);
            Color color = signObject.GetComponent<Image>().color;
            color.r = 0.5f;
            color.g = 0.5f;
            color.b = 0.5f;
            name.GetComponent<Text>().color = color;
            image_prop.GetComponent<Image>().color = color;
            signObject.GetComponent<Image>().color = color;


            image_lingqu.transform.localScale = Vector3.one;
            btn_Sign.transform.localScale = Vector3.zero;
            image_Signed.transform.localScale = Vector3.one;
            SignData.IsSign = true;
            SignData.SignWeekDays++;
            isSignSuccess = false;

            SignItem signItem = _signItems[totalSignDays];
            print(signItem.goods_prop);
            AddProp(signItem.goods_prop);

            ShowRewardPanelScript.create().GetComponent<ShowRewardPanelScript>().setData(signItem.goods_prop);

            GameObject.Find("Canvas").GetComponent<MainScript>().checkRedPoint();
        }
    }

    private void AddProp(string prop)
    {
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
              
            }
            GameObject.Find("Canvas").GetComponent<MainScript>().refreshUI();
            GameObject.Find("Canvas").GetComponent<MainScript>().checkRedPoint();
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitData()
    {
        totalSignDays = SignData.SignWeekDays;


        for (int i = 0; i < 7; i++)
        {
            Transform child = content.transform.GetChild(i);
            signObjects.Add(child.gameObject);
        }

        if (_signItems == null ||_signItems.Count != signObjects.Count)
        {
            print("数据初始化错误");
            return;
        }
    }

    /// <summary>
    /// 初始化ui   
    /// </summary>
    private void InitUi()
    {
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

            var name = Object.transform.GetChild(1);
            var image_lingqu = Object.transform.GetChild(2);
            var image_prop = Object.transform.GetChild(3);
           

            //设置元宝等道具
            Text text1 = name.GetComponent<Text>();
            text1.text = signItem.goods_name;
            var prop = image_prop.GetComponent<Image>();
            if (i == 6)
            {
                prop.sprite = Resources.Load<Sprite>("Sprites/Sign/icon_libao");
            }
            else
            {
                prop.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/" + signItem.goods_icon);
            }
            
            //未签到
            if (SignData.IsSign == false)
            {
                if (i < totalSignDays)
                {
                    Color color = Object.GetComponent<Image>().color;
                    color.r = 0.5f;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    Object.GetComponent<Image>().color = color;
                    text1.color = color;
                    prop.color = color;

                    image_lingqu.transform.localScale = Vector3.one;
                }
                else
                {
                    image_lingqu.transform.localScale = Vector3.zero;
                }
                if (totalSignDays == i)
                {
                    Button button = Object.AddComponent<Button>();
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() =>
                    {
                        //发送 签到请求
                        LogicEnginerScript.Instance.GetComponent<SignRequest>().CallBack = SignCallBack;
                        LogicEnginerScript.Instance.GetComponent<SignRequest>().OnRequest();
                    });
                    Object.GetComponent<Image>().color = new Color(1, 185/(float)255,0,1);
                }
            }
            //已签到
            else
            {
                if (i < totalSignDays)
                {
                    Color color = Object.GetComponent<Image>().color;
                    color.r = 0.5f;
                    color.g = 0.5f;
                    color.b = 0.5f;
                    Object.GetComponent<Image>().color = color;
                    text1.color = color;
                    prop.color = color;
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
        isSignSuccess = flag;
        if (!flag)
        {
            print("签到错误");
        }
    }
}