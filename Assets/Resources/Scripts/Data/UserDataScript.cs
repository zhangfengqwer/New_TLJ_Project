using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataScript
{
    static UserDataScript s_userDataScript = null;

    public string m_account;
    public string m_password;

    UserInfo m_userInfo = new UserInfo();

    public static UserDataScript getInstance()
    {
        if (s_userDataScript == null)
        {
            s_userDataScript = new UserDataScript();
        }

        return s_userDataScript;
    }

    public UserInfo getUserInfo()
    {
        return m_userInfo;
    }
}

public class UserInfo
{
    public string m_uid;
    public string m_name;
    public int m_goldNum;
}