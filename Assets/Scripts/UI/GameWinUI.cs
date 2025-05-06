using Base;

namespace UI
{
    public class GameWinUI : BaseViewMono
    {
        public void ShowUI()
        {
            gameObject.SetActive(true);
        }

        public void HideUI()
        {
            gameObject.SetActive(false);
        }

        public void Init()
        {
            HideUI();
        }
    }
}