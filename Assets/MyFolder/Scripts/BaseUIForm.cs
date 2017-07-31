using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MYUIFW
{
    public class BaseUIForm : MonoBehaviour
    {
        /* 字段 */
        //当前（基类）窗口的类型
        private UIType _CurrentUIType = new UIType();

        /* 属性 */
        
        internal UIType CurrentUIType
        {
            set
            {
                _CurrentUIType = value;
            }

            get
            {
                return _CurrentUIType;
            }
        }

        //页面显示
        public virtual void Display()
        {
            this.gameObject.SetActive(true);
        }
        //页面隐藏（不在“栈”集合中）
        public virtual void Hiding()
        {
            this.gameObject.SetActive(false);
        }

        //页面重新显示
        public virtual void ReDisplay()
        {
            this.gameObject.SetActive(true);
        }
        //页面冻结（还在“栈”集合中）
        public virtual void Freeze()
        {
            this.gameObject.SetActive(true);
        }
    }
}

