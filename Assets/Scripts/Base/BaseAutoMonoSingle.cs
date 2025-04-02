using Base;
using UnityEngine;

namespace XYZFrameWork.Base
{
    public class BaseAutoMonoSingle<T> : BaseMono where T : BaseMono, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).ToString());
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<T>();
                }

                return _instance;
            }
        }
    }
}
