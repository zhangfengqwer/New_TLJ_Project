using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

public class WeeklySignScript : MonoBehaviour
{
    private List<string> signData = new List<string>();
    private List<GameObject> signObjects = new List<GameObject>();
    public GameObject content;

    private int totalSignDays;
    private List<SignItem> _signItems;
    private bool isSign = false;

    // Use this for initialization
    void Start()
    {
        if (SignData.SignTotalDays >= 7)
        {
            SignData.SignTotalDays = 0;
        }
        InitData();
        string[] strings = SignData.UpdateTime.ToShortDateString().Split('/');
        foreach (var VARIABLE in strings)
        {
            print(VARIABLE);
            
        }
        print(SignData.UpdateTime.ToShortDateString() + "\n" + DateTime.Now+"\n"+strings.Length);
        if (_signItems.Count != signObjects.Count)
        {
            print("数据初始化错误");
            return;
        }
        InitUi();
    }

    private void InitData()
    {
        totalSignDays = SignData.SignTotalDays;
        signData.Add("第一天");
        signData.Add("第二天");
        signData.Add("第三天");
        signData.Add("第四天");
        signData.Add("第五天");
        signData.Add("第六天");
        signData.Add("第七天");
        for (int i = 0; i < 7; i++)
        {
            Transform child = content.transform.GetChild(i);
            signObjects.Add(child.gameObject);
        }
        FileStream fileStream = null;
        try
        {
            fileStream = new FileStream(Path.Combine(Application.dataPath, "sign.json"), FileMode.Open);
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
    }

    private void InitUi()
    {
        for (int i = 0; i < signObjects.Count; i++)
        {
            GameObject Object = signObjects[i];
            string s = signData[i];
            SignItem signItem = _signItems[i];

            var child = Object.transform.GetChild(0);
            var name = Object.transform.GetChild(1);
            Text text = child.GetComponent<Text>();
            Text text1 = name.GetComponent<Text>();
            text.text = s;
            text1.text = signItem.ItemName + "x" + signItem.ItemCount;
            if (totalSignDays > i)
            {
                Color color = Object.GetComponent<Image>().color;
                color.a = 0.5f;
                Object.GetComponent<Image>().color = color;
            }
            if (totalSignDays == i)
            {
                Object.GetComponent<Image>().color = Color.blue;
            }
        }
    }

    public void OnSignClick()
    {
        GameObject signObject = signObjects[totalSignDays];

        Color color = signObject.GetComponent<Image>().color;
        color.a = 0.5f;
        signObject.GetComponent<Image>().color = color;
        SignData.SignTotalDays++;
        print(SignData.SignTotalDays);
        
    }
}