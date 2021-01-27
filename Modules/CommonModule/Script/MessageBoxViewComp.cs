using System;
using UnityEngine.UI;

namespace lgu3d {
    public class MessageBoxViewComp : Model_BaseViewComp<CommonModule>
    {
        private Text Message;
        private Button CloseButt;
        private Button ConfirmButt;
        private Button CancelButt;
        private Action ConfirmFun;
        private Action CancelFun;
        public override void Load(ModelBase module, params object[] agr)
        {
            base.Load(module, "MessageBox");
            Message = UIGameobject.OnSubmit<Text>("Msg");
            CloseButt = UIGameobject.OnSubmit<Button>("CloseButt");
            ConfirmButt = UIGameobject.OnSubmit<Button>("Butts/ConfirmButt");
            CancelButt = UIGameobject.OnSubmit<Button>("Butts/CancelButt");

            CloseButt.onClick.AddListener(CloseButtClick);
            ConfirmButt.onClick.AddListener(ConfirmButtClick);
            CancelButt.onClick.AddListener(CancelButttClick);
            Hide();
            LoadEnd();
        }


        public void ShowBox(string msg,Action confirmFun, Action cancelFun)
        {
            ConfirmFun = confirmFun;
            CancelFun = cancelFun;
            Message.text = msg;
            Show();
        }
        
        private void CloseButtClick()
        {
            Hide();
        }

        private void ConfirmButtClick()
        {
            Hide();
            ConfirmFun?.Invoke();
        }

        private void CancelButttClick()
        {
            Hide();
            CancelFun?.Invoke();
        }
    }
}
