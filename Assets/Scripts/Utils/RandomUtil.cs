using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class RandomUtil
{
    static int s_count = 0;

    public static int getRandom(int start, int end)
    {
        if (++s_count >= 99999)
        {
            s_count = 0;
        }

        string s_timeStamp = getTimeStamp().ToString();
        int timeStamp = int.Parse(s_timeStamp.Substring(5));
        Random ran = new Random(timeStamp + s_count);

        return ran.Next(start, end + 1);
    }

    static long getTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }
}
