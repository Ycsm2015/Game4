using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MYUIFW
{
    public class UIType : MonoBehaviour
    {
        //是否需要清空“反向切换”
        public bool IsClearReverseChange = false;
        //UI窗体类型
        public UIFormType UIForms_Type = UIFormType.Normal;
        //UI窗体显示类型
        public UIFormShowMode UIForms_ShowMode = UIFormShowMode.Normal;
        //UI船体透明度类型
        public UIFormLucenyType UIForm_LucencyType = UIFormLucenyType.Lucency;
    }

}
