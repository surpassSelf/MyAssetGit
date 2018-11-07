using UnityEngine;
using System.Collections;

public static class Util{

    #region 位操作
    /// <summary>
    /// long整形位标记方法
    /// </summary>
    /// <param name="num">long</param>
    /// <param name="index">哪一位标记</param>
    /// <param name="flag">true 1 false 0</param>
    /// <returns></returns>
    public static long SetLongBit(long num, int index, bool flag)
    {
        return flag ? ((long)1 << index | num) : (~((long)1 << index) & num);
    }

    /// <summary>
    /// 获取long整形某一位的标记值
    /// </summary>
    /// <param name="num">存标记的值</param>
    /// <param name="index">要取的位</param>
    /// <returns>1 true 0 false</returns>
    public static int GetLongBit(long num, int index)
    {
        return (num & ((long)1 << index)) > 0 ? 1 : 0;
    }

    #endregion
}
