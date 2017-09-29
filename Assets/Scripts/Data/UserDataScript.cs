using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataScript
{
    static UserDataScript s_userDataScript = null;

    //public string m_account;
    //public string m_password;

    UserInfo m_userInfo = new UserInfo();
    List<MyProp> m_myPropList = new List<MyProp>();

    public static UserDataScript getInstance()
    {
        if (s_userDataScript == null)
        {
            s_userDataScript = new UserDataScript();
        }

        return s_userDataScript;
    }

    // 清空数据
    public static void clearData()
    {
        s_userDataScript = null;
    }

    // 获取用户个人信息
    public UserInfo getUserInfo()
    {
        return m_userInfo;
    }

    // 获取用户道具信息
    public List<MyProp> getMyPropList()
    {
        return m_myPropList;
    }
}

public class UserInfo
{
    public string m_uid = "";
    public string m_name = "";
    public int m_goldNum = 0;
    public int m_yuanBaoNum = 0;
}

public class MyProp
{
    public int m_id = 0;
    public int m_num = 0;
}