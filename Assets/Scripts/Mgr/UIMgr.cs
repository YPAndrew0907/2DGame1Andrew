using XYZFrameWork.Base;

namespace Mgr
{
    public class UIMgr : BaseAutoMonoSingle<UIMgr>
    {
        public enum UILayer
        {
            HUD,
            Window,
            Top,
        }
        
        public void OpenUI(UILayer layer, string uiName)
        {
            
        }
    }
}