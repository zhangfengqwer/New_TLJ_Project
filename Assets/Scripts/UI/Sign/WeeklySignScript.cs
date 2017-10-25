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
    private List<SignItem> _signItems;
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
    }

    private void Update()
    {
        //签到成功后，做的一些ui操作
        if (isSignSuccess)
        {
            GameObject signObject = signObjects[totalSignDays];
            var image_lingqu = signObject.transform.GetChild(2);
            Color color = signObject.GetComponent<Image>().color;
            color.r = 0.5f;
            color.g = 0.5f;
            color.b = 0.5f;
            signObject.GetComponent<Image>().color = color;
            image_lingqu.transform.localScale = Vector3.one;
            btn_Sign.transform.localScale = Vector3.zero;
            image_Signed.transform.localScale = Vector3.one;
            SignData.IsSign = true;
            SignData.SignWeekDays++;
            isSignSuccess = false;
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
        //获得签到的道具配置
        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream(Path.Combine(Application.dataPath, "Resources/Temp/sign.json"), FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8);
            string str = streamReader.ReadToEnd();
            print(str);
            _signItems = JsonMapper.ToObject<List<SignItem>>(str);
        }
        catch (Exception e)
        {
            print(e);
        }
        finally
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }

        if (_signItems.Count != signObjects.Count)
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
            //设置元宝等道具
            Text text1 = name.GetComponent<Text>();
            text1.text = signItem.ItemName + "x" + signItem.ItemCount;
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
                    image_lingqu.transform.localScale = Vector3.one;
                }
                else
                {
                    image_lingqu.transform.localScale = Vector3.zero;
                }
                if (totalSignDays == i)
                {
//                    Object.GetComponent<Image>().color = Color.blue;
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
        //发送 签到请求
        LogicEnginerScript.Instance.GetComponent<SignRequest>().CallBack = SignCallBack;
        LogicEnginerScript.Instance.GetComponent<SignRequest>().OnRequest();
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