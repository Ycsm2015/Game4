using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MYUIFW
{
    public class UIManager : MonoBehaviour
    {
        /* 字段 */
        private static UIManager _Instance = null;
        //UI窗体预设路径（Param1：窗体预设名称，2：表示窗体预设路径）
        private Dictionary<string, string> _DicFormsPaths;
        //缓存所有UI窗体
        private Dictionary<string, BaseUIForm> _DicALLUIForms;
        //“栈”结构表示的“当前UI窗体”集合
        private Stack<BaseUIForm> _StaCurrentUIForms;
        //当前显示的UI窗体
        private Dictionary<string, BaseUIForm> _DicCurrentShowUIForms;
        //UI根结点
        private Transform _TraCanvasTransform = null;
        //全屏幕显示的节点
        private Transform _TraNormal = null;
        //固定显示的节点
        private Transform _TraFixed = null;
        //弹出节点
        private Transform _TraPopUp = null;
        //UI管理脚本的节点
        private Transform _TraUIScripts = null;

        //得到实例
        public static UIManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new GameObject("_UIManager").AddComponent<UIManager>();
            }
            return _Instance;
        }
        //初始化核心数据
        public void Awake()
        {
            //字段初始化
            _DicFormsPaths = new Dictionary<string, string>();
            _DicALLUIForms = new Dictionary<string, BaseUIForm>();
            _StaCurrentUIForms = new Stack<BaseUIForm>();
            _DicCurrentShowUIForms = new Dictionary<string, BaseUIForm>();
            //初始化加载（根UI窗体）Canvas预设
            InitRootCanvasLoading();
            //得到UI根节点、全屏节点、固定节点、弹出节点
            _TraCanvasTransform = GameObject.FindGameObjectWithTag(UIFormDefine.SYS_TAG_CANVAS).transform;
            _TraNormal = _TraCanvasTransform.Find("Normal");
            _TraFixed = _TraCanvasTransform.Find("Fixed");
            _TraPopUp = _TraCanvasTransform.Find("PopUp");
            _TraUIScripts = _TraCanvasTransform.Find("_ScriptMgr");


            //把本脚本作为“根UI窗体”的子节点
            this.gameObject.transform.SetParent(_TraUIScripts, false);
            //“根UI窗体”在场景转换的时候，不允许销毁
            DontDestroyOnLoad(_TraCanvasTransform);
            //初始化“UI窗体预设”路径数据
            if (_DicFormsPaths != null)
            {
                _DicFormsPaths.Add("ConsoleWindow", @"ConsoleWindow");
            }

        }
        //显示（打开）UI窗体
        //功能：
        //1.根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        //2.根据不同的UI窗体的“显示模式”，分别作不同的加载处理
        public void ShowUIForm(string uiFormName)
        {
            BaseUIForm baseUIForm = null;           //UI窗体基类

            //参数检查
            if (string.IsNullOrEmpty(uiFormName)) return;
            //根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
            baseUIForm = LoadFormsToAllUIFormsCatch(uiFormName);
            if (baseUIForm == null) return;

            //判断是否清空“栈”结构体集合
            if (baseUIForm.CurrentUIType.IsClearReverseChange)
            {
                ClearStackArray();
            }
            //根据不同的UI窗体的显示模式，分别作不同的加载处理
            switch (baseUIForm.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:         //“普通显示”窗口模式
                    //把当前窗体加载到“当前窗体”集合中
                    LoadUIToCurrentCache(uiFormName);
                    break;
                case UIFormShowMode.ReverseChange:  //需要“反向切换”窗口模式
                    PushUIForm(uiFormName);
                    break;
                case UIFormShowMode.HideOther:      //“隐藏其他”窗口模式
                    ExitUIFormFromCacheAndShowOther(uiFormName);

                    break;
                default:
                    break;
            }
        }

        //关闭或返回上一个UI窗体（关闭当前UI窗体）
        public void CloseOrReturnUIForm(string uiFormName)
        {
            BaseUIForm baseUIForm = null;

            /* 参数检查 */
            if (string.IsNullOrEmpty(uiFormName)) return;
            //“所有UI窗体缓存”如果没有记录，则直接返回
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm == null) return;

            /* 判断不同的窗体的显示模式，分别进行处理 */
            switch (baseUIForm.CurrentUIType.UIForms_ShowMode)
            {
                case UIFormShowMode.Normal:
                    ExitUIFormCache(uiFormName);
                    break;
                case UIFormShowMode.ReverseChange:
                    PopUIForm();
                    break;
                case UIFormShowMode.HideOther:
                    ExitUIFormFromCacheAndShowOther(uiFormName);
                    break;
                default:
                    break;
            }
        }
        //初始化加载（根UI窗体）Canvas预设
        private void InitRootCanvasLoading()
        {
            ResourcesMgr.GetInstance().LoadAsset(UIFormDefine.SYS_PATH_CANVAS, false);
            //Resources.Load(UIFormDefine.SYS_TAG_CANVAS);
        }
        //根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        private BaseUIForm LoadFormsToAllUIFormsCatch(string uiFormName)
        {
            BaseUIForm baseUIResult = null;     //加载的返回UI窗体基类

            _DicALLUIForms.TryGetValue(uiFormName, out baseUIResult);
            if (baseUIResult == null)
            {
                //加载制定名称的“UI窗体”
                baseUIResult = LoadUIForm(uiFormName);
            }

            return baseUIResult;
        }
        private void EnterUIFormCache(string uiFormName)
        {
            BaseUIForm baseUIForm;                  //UI窗体基类
            BaseUIForm baseUIFormsFromAllCache;     //“所有窗体集合”中的窗体基类

            //“正在显示UI窗体缓存”集合里有记录，则直接返回。
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm != null)
                return;
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIFormsFromAllCache);
            if (baseUIFormsFromAllCache != null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, baseUIFormsFromAllCache);
                baseUIFormsFromAllCache.Display();
            }
        }

        private void ExitUIFormCache(string uiFormName)
        {
            BaseUIForm baseUIForm;              //UI窗体基类

            //“正在显示UI窗体缓存”集合没有记录，则直接返回
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm == null)
                return;

            //指定UI窗体，运行隐藏状态，且从“正在显示UI窗体缓存”集合中移除
            baseUIForm.Hiding();
            _DicCurrentShowUIForms.Remove(uiFormName);

        }

        private void EnterFormsToCacheHideOther(string uiFormName)
        {
            BaseUIForm baseUIForm;              //UI窗体基类
            BaseUIForm baseUIFormsFromAllCache;  //“所有窗体集合”中的窗体基类

            //“正在显示UI窗体缓存”与“栈缓存”集合里所有窗体进行隐藏处理。
            foreach (BaseUIForm baseUIFormItem in _DicCurrentShowUIForms.Values)
            {
                baseUIFormItem.Hiding();
            }
            foreach (BaseUIForm baseUIFormItem in _StaCurrentUIForms)
            {
                baseUIFormItem.Hiding();
            }
            //把当前窗体，加载到“正在显示UI窗体缓存”集合里
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIFormsFromAllCache);
            if (baseUIFormsFromAllCache != null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, baseUIFormsFromAllCache);
                baseUIFormsFromAllCache.Display();
            }
        }
        private void ExitUIFormFromCacheAndShowOther(string uiFormName)
        {
            BaseUIForm baseUIForm;              //UI窗体集合

            //“正在显示UI窗体缓存”集合没有记录，直接返回
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm == null) return;

            //指定UI窗体，运行隐藏状态，且从“正在显示UI窗体缓存”集合中移除
            baseUIForm.Hiding();
            _DicCurrentShowUIForms.Remove(uiFormName);

            //“正在显示UI窗体缓存”与栈缓存“集合里所有窗体进行再次显示处理。
            foreach (BaseUIForm baseUIFormItem in _DicCurrentShowUIForms.Values)
            {
                baseUIFormItem.ReDisplay();
            }
            foreach (BaseUIForm baseUIFormItem in _StaCurrentUIForms)
            {
                baseUIFormItem.ReDisplay();
            }
        }

        //UI窗体入栈
        //功能：
        //  1.判断栈里是否已经有窗体，有则“冻结”
        //  2.先判断“UI预设缓存集合”是否有指定的UI窗体，有则处理。
        //  3.指定UI窗体入“栈”
        private void PushUIForm(string uiFormName)
        {
            BaseUIForm baseUI;              //UI预设窗体

            //判断栈里是否已经有窗体，有则“冻结”
            if (_StaCurrentUIForms.Count > 0)
            {
                BaseUIForm topUIForm = _StaCurrentUIForms.Peek();
                topUIForm.Freeze();
            }

            //先判断“UI预设缓存集合”是否有指定的UI窗体，有则处理。
            _DicALLUIForms.TryGetValue(uiFormName, out baseUI);
            if (baseUI != null)
            {
                baseUI.Display();

            }
            else
            {

            }

            //指定UI窗体入“栈”
            _StaCurrentUIForms.Push(baseUI);
        }

        private void PopUIForm()
        {
            if(_StaCurrentUIForms.Count>=2 )
            {
                /* 出栈逻辑 */
                BaseUIForm topUIForm = _StaCurrentUIForms.Pop();
                //出栈的窗体，进行隐藏处理
                topUIForm.Hiding();
                //出栈窗体的下一个窗体逻辑
                BaseUIForm nextUIForm = _StaCurrentUIForms.Peek();
                //下一个窗体“重新显示”处理
                nextUIForm.ReDisplay();
            }else if(_StaCurrentUIForms.Count == 1)
            {
                /* 出栈逻辑 */
                BaseUIForm topUIForm = _StaCurrentUIForms.Pop();
                //出栈的窗体，进行“隐藏”处理
                topUIForm.Hiding();
            }
        }
        //加载指定名称的“UI窗体”
        //功能：
        //  1.根据“UI窗体名称”，加载预设克隆体。
        //  2.根据不同预设克隆体中带的脚本中不同的“位置信息”，加载到“跟窗体”下不同的节点
        //  3.隐藏刚创建的UI克隆体
        //  4.把克隆体加入到“所有UI窗体”缓存集合中
        private BaseUIForm LoadUIForm(string uiFormName)
        {
            string strUIFormPath = null;            //UI窗体路径
            GameObject goCloneUIPrefabs = null;     //创建的UI克隆体预设
            BaseUIForm baseUIForm = null;           //窗体基类
            //根据UI窗体名称，得到对应的加载路径
            _DicFormsPaths.TryGetValue(uiFormName, out strUIFormPath);
            //根据UI窗体名称，加载预设克隆体
            if (!string.IsNullOrEmpty(strUIFormPath))
            {
                goCloneUIPrefabs = ResourcesMgr.GetInstance().LoadAsset(strUIFormPath, false);
                //goCloneUIPrefabs = Resources.Load<GameObject>(strUIFormPath);
            }
            //设置“UI克隆体”的父节点（根据克隆体中带的脚本不同的“位置信息”）
            if (_TraCanvasTransform != null && goCloneUIPrefabs != null)
            {
                baseUIForm = goCloneUIPrefabs.GetComponent<BaseUIForm>();
                if (baseUIForm == null)
                {
                    Debug.Log("baseUIForm == null,请先确认窗体预设对象上是否加载了baseUIForm的子类脚本。Parma uiFormName = " + uiFormName);
                    return null;
                }
                switch (baseUIForm.CurrentUIType.UIForms_Type)
                {
                    case UIFormType.Normal: //普通窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraNormal, false);
                        break;
                    case UIFormType.Fixed:  //固定窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraFixed, false);
                        break;
                    case UIFormType.PopUp:  //弹出窗体节点
                        goCloneUIPrefabs.transform.SetParent(_TraPopUp, false);
                        break;
                    default:
                        break;
                }
                //设置隐藏
                goCloneUIPrefabs.SetActive(false);
                //把克隆体加入到“所有UI窗体”（缓存）集合中
                _DicALLUIForms.Add(uiFormName, baseUIForm);
                return baseUIForm;
            }
            else
            {
                Debug.Log("_TraCanvasTransfor == null Or goCloneUIPrefabs == null ::Param uiFormName = " + uiFormName);

            }
            Debug.Log("unknow error ::Param uiFormName = " + uiFormName);
            return null;
        }
        //把当前窗体加载到“当前窗体”集合中
        private void LoadUIToCurrentCache(string uiFormName)
        {
            BaseUIForm baseUIForm;                  //UI窗体基类
            BaseUIForm baseUIFormFromAllCache;      //从“所有窗体集合”中得到的窗体
            //如果“正在显示”的集合中，存在这个UI窗体，则直接返回
            _DicCurrentShowUIForms.TryGetValue(uiFormName, out baseUIForm);
            if (baseUIForm != null) return;
            //把当前窗体，加载到“正在显示”集合中
            _DicALLUIForms.TryGetValue(uiFormName, out baseUIFormFromAllCache);
            if (baseUIFormFromAllCache != null)
            {
                _DicCurrentShowUIForms.Add(uiFormName, baseUIFormFromAllCache);
                baseUIFormFromAllCache.Display();   //显示当前窗体
            }
        }

        //清空“栈”结构体集合
        private bool ClearStackArray()
        {
            if(_StaCurrentUIForms != null&& _StaCurrentUIForms.Count >=1 )
            {
                _StaCurrentUIForms.Clear();
                return true;
            }
            return false;
        }
    }
}

