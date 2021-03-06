﻿using System;
using System.Text.RegularExpressions;

/// <summary> 
/// 验证身份证号码类 
/// </summary> 
public class VerifyRuleUtil
{

    public static bool CheckRealName(string text)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VerifyRuleUtil_hotfix", "CheckRealName"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VerifyRuleUtil_hotfix", "CheckRealName", null, text);
            return b;
        }

        bool _isCorrectRealName;
        if (text.Length > 1)
        {
            _isCorrectRealName = true;
            for (int i = 0; i < text.Length; i++)
            {
                if (!Regex.IsMatch(text.ToString(), "^[\u4e00-\u9fa5]{0,}$"))
                {
                    _isCorrectRealName = false;
                    break;
                }
            }
        }
        else
        {
            _isCorrectRealName = false;
        }
        return _isCorrectRealName;
    }
    /// <summary> 
    /// 验证身份证合理性 
    /// </summary> 
    /// <param name="Id"></param> 
    /// <returns></returns> 
    public static bool CheckIDCard(string idNumber)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VerifyRuleUtil_hotfix", "CheckIDCard"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VerifyRuleUtil_hotfix", "CheckIDCard", null, idNumber);
            return b;
        }

        if (idNumber.Length == 18)
        {
            bool check = CheckIDCard18(idNumber);
            return check;
        }
        else if (idNumber.Length == 15)
        {
            bool check = CheckIDCard15(idNumber);
            return check;
        }
        else
        {
            return false;
        }
    }


    /// <summary> 
    /// 18位身份证号码验证 
    /// </summary> 
    public static bool CheckIDCard18(string idNumber)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VerifyRuleUtil_hotfix", "CheckIDCard18"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VerifyRuleUtil_hotfix", "CheckIDCard18", null, idNumber);
            return b;
        }

        long n = 0;
        if (long.TryParse(idNumber.Remove(17), out n) == false
            || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
        {
            return false;//数字验证 
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(idNumber.Remove(2)) == -1)
        {
            return false;//省份验证 
        }
        string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;//生日验证 
        }
        string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
        char[] Ai = idNumber.Remove(17).ToCharArray();
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
        }
        int y = -1;
        Math.DivRem(sum, 11, out y);
        if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
        {
            return false;//校验码验证 
        }
        return true;//符合GB11643-1999标准 
    }


    /// <summary> 
    /// 16位身份证号码验证 
    /// </summary> 
    public static bool CheckIDCard15(string idNumber)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VerifyRuleUtil_hotfix", "CheckIDCard15"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VerifyRuleUtil_hotfix", "CheckIDCard15", null, idNumber);
            return b;
        }

        long n = 0;
        if (long.TryParse(idNumber, out n) == false || n < Math.Pow(10, 14))
        {
            return false;//数字验证 
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(idNumber.Remove(2)) == -1)
        {
            return false;//省份验证 
        }
        string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;//生日验证 
        }
        return true;
    }

    public static bool CheckPhone(string inputText)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VerifyRuleUtil_hotfix", "CheckPhone"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VerifyRuleUtil_hotfix", "CheckPhone", null, inputText);
            return b;
        }

        return Regex.IsMatch(inputText, @"^[1]+\d{10}");
    }

    public static bool CheckVerificationCode(string inputText)
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("VerifyRuleUtil_hotfix", "CheckVerificationCode"))
        {
            bool b = (bool)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.VerifyRuleUtil_hotfix", "CheckVerificationCode", null, inputText);
            return b;
        }

        return Regex.IsMatch(inputText, @"\d{6}");
    }
}