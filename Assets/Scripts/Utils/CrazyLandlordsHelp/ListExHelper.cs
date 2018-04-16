using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyLandlords.Helper
{
    public static class ListExHelper
    {
        public static void RemoveList<T>(this List<T> list, List<T> items)
        {
            foreach (var item in items)
            {
                list.Remove(item);
            }
        }

        public static void RemoveList<T>(this List<T> list, List<List<T>> items)
        {
            foreach (var item in items)
            {
                foreach (var i in item)
                {
                    list.Remove(i);
                }
            }
        }
    }
}