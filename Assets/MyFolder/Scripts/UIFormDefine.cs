using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MYUIFW
{

    public enum UIFormType
    {
        //普通窗体
        Normal,
        //固定窗体
        Fixed,
        //弹出窗体
        PopUp
    }

    public enum UIFormShowMode
    {
        //普通
        Normal,
        //反向切换
        ReverseChange,
        //隐藏其他
        HideOther
    }

    public enum UIFormLucenyType
    {
        //完全透明，不能穿透
        Lucency,
        //半透明，不能穿透
        Translucence,
        //低透明度，不能穿透
        Impenetrable,
        //可以穿透
        Pentrate
    }

    public class UIFormDefine : MonoBehaviour
    {
        public static string SYS_TAG_CANVAS = "_TagCanvas";

        //public const string SYS_PATH_CANVAS = "MyFolder/UIPrefabs/Canvas";
        public const string SYS_PATH_CANVAS = "Canvas";


    }
}