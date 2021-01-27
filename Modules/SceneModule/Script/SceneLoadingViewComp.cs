using UnityEngine.UI;

namespace lgu3d
{
    public class SceneLoadingViewComp : Module_BaseViewComp
    {
        private Slider LoadProgress;

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            ShowLevel = UILevel.HightUI;
            base.Load(_ModelContorl, "LoadingView");
            LoadProgress = UIGameobject.OnSubmit<Slider>("LoadProgress");
        }

        public void UpdataProgress(float _Progress)
        {
            LoadProgress.value = _Progress;
        }
    }
}
