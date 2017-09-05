using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CommonUtil
{
    // 格式2017/7/12 15:05:03
    static public string getCurTime()
    {
        return DateTime.Now.ToString();
    }

    // 格式2017/7
    static public string getCurYearMonth()
    {
        return DateTime.Now.Year + "/" + DateTime.Now.Month;
    }

    static public int getCurYear()
    {
        return DateTime.Now.Year;
    }

    static public int getCurMonth()
    {
        return DateTime.Now.Month;
    }

    static public int getCurDay()
    {
        return DateTime.Now.Day;
    }

    static public bool splitStrIsPerfect(string str, List<string> list, string str_c)
    {
        bool b = false;
        {
            string temp = "";
            for (int i = str.Length - str_c.Length; i < str.Length; i++)
            {
                temp += str[i];
            }

            if (temp.CompareTo(str_c) == 0)
            {
                b = true;
            }
        }

        char c = '.';
        str = str.Replace(str_c, ".");
        splitStr(str,list,c);

        return b;
    }

    /*
     * 裁剪字符串：1.2.3.3.5
     * str：源字符串
     * list：裁剪后存放的地方
     * c：裁剪规则
     * 如：splitStr("1.2.3.4.5",list,'.');
     */
    static public void splitStr(string str, List<string> list, char c)
    {
        string temp = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] != c)
            {
                temp += str[i];
            }
            else
            {
                list.Add(temp);
                temp = "";
            }

            if ((str[i] != c) && (i == (str.Length - 1)))
            {
                list.Add(temp);
                temp = "";
            }
        }
    }

    /*
     * subStringEndByChar("1/2/3/4/5/6",'/')
     * 返回6
     */
    static public string subStringEndByChar(string str,char c)
    {
        return str.Substring(str.LastIndexOf(c) + 1);
    }
}
