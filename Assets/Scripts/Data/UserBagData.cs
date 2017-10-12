using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBagData {

    static UserBagData s_userBagData = null;

    List<MyBagPropData> m_myBagDataList = new List<MyBagPropData>();

    public static UserBagData getInstance()
    {
        if (s_userBagData == null)
        {
            s_userBagData = new UserBagData();
        }

        return s_userBagData;
    }

    public void initJson(string json)
    {
        m_myBagDataList.Clear();

        try
        {
            JsonData jsonData = JsonMapper.ToObject(json);
            List<MyBagPropData> temp = JsonMapper.ToObject<List<MyBagPropData>>(jsonData["prop_list"].ToString());

            for (int i = 0; i < temp.Count; i++)
            {
                MyBagPropData myBagPropData = new MyBagPropData();
                myBagPropData.prop_id = temp[i].prop_id;
                myBagPropData.prop_num = temp[i].prop_num;
                m_myBagDataList.Add(myBagPropData);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public List<MyBagPropData> getUserBagDataList()
    {
        return m_myBagDataList;
    }

    public void useProp(int prop_id,int num)
    {
        for (int i = 0; i < m_myBagDataList.Count; i++)
        {
            if (m_myBagDataList[i].prop_id == prop_id)
            {
                m_myBagDataList[i].prop_num -= num;

                if (m_myBagDataList[i].prop_num <= 0)
                {
                    m_myBagDataList.RemoveAt(i);
                }

                break;
            }
        }
    }
}

public class MyBagPropData
{
    public int prop_id = 0;
    public int prop_num = 0;
}