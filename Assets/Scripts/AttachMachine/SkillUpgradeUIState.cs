using System.Collections;
using Mgr;
using UI;

namespace AttachMachine
{
    public class SkillUpgradeUIState : BaseGameUIState
    {
        public override string StateID => StateIDStr;
        public const    string StateIDStr = "SkillUpgradeUIState";

        private ISkillUpgradeUIState _uiState;
        
        public override void OnCreate(IMachineMaster sceneUI)
        {
            if (sceneUI is ISkillUpgradeUIState ui)
            {
                _uiState = ui;
                _uiState.SkillUpgradeUI.Init();
            }
        }

        public override IEnumerator OnEnterAsync(object payload)
        {
            var cfgs = SkillMgr.Instance.GetAllSkillConfigs();
            _uiState.SkillUpgradeUI.Show(cfgs);
            yield break;
        }
        
        public override void OnUpdate(float deltaTime)
        {

        }
    }

    public interface ISkillUpgradeUIState : IBaseAttachUI
    {
        public SkillUpgradeUI SkillUpgradeUI { get; }
    }
}