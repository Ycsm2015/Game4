using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MYUIFW
{
    public class ResourcesMgr : MonoBehaviour
    {
        /* 字段 */
        private static ResourcesMgr _Instance;
        private Hashtable ht = null;


        public static ResourcesMgr GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new GameObject("_ResourceMgr").AddComponent<ResourcesMgr>();
            }
            return _Instance;
        }

        private void Awake()
        {
            ht = new Hashtable();
        }

        public T LoadResource<T>(string path, bool isCatch) where T : UnityEngine.Object
        {
            if (ht.Contains(path))
            {
                return ht[path] as T;
            }
            T TResource = Resources.Load<T>(path);
            if (TResource == null)
            {
                Debug.LogError(GetType() + "/GetInstance()/TResource 提取到的资源找不到，请检查。 path=" + path);
            }
            else if (isCatch)
            {
                ht.Add(path, TResource);
            }
            return TResource;
        }

        public GameObject LoadAsset(string path, bool isCatch)
        {
            GameObject goObj = LoadResource<GameObject>(path, isCatch);
            GameObject goObjClone = GameObject.Instantiate<GameObject>(goObj);
            if (goObjClone == null)
            {
                Debug.LogError(GetType() + "/LoadAsset()/克隆资源不成功，请检查。 path=" + path);
            }
            return goObjClone;
        }
    }

}
