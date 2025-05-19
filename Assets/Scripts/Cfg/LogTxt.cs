namespace Cfg
{
    public static class LogTxt
    {
        public const string LOGIN  = "登录";
        public const string LOGOUT = "登出";

        #region ERROR

        public const string PARAM_ERROR    = "参数错误，请检查";
        public const string NOT_EXIT_ERROR = "变量不存在，请检查";

        public const string TYPE_ERROR            = "类型错误，请检查";
        public const string PARAM_CREATE_ERROR    = "类型创建错误，请检查";
        public const string PARAM_TRANSFORM_ERROR = "类型转换错误，请检查";

        public const string ASSET_LOAD_ERROR = "资源加载错误";


        public const string MAP_OUT_RANGE_ERROR = "地图超出范围 ：";

        public const string LOAD_ASSET_ERROR = "加载资源失败";
        public const string LOAD_NAME_ERROR  = "加载名称错误";

        public const string POOL_GET_ERROR      = "对象池获取发生错误";
        public const string POOL_TEMPLATE_ERROR = "对象池模板错误";
        public const string OBJECT_GET_ERROR    = "变量丢失，请检查代码";

        #endregion

        #region Warning

        public const string REPEAT_ADD_WARNING = "重复添加";


        #endregion
    }
}