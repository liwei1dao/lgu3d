using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lgu3d
{
    [RequireComponent(typeof(Toggle))]
    public class UGUIToggleColor : MonoBehaviour
    {
        public Color OnColor = Color.white;
        public Color OffColor = Color.white;

        public List<MaskableGraphic> graphicItems = new List<MaskableGraphic>();

        private Toggle toggle;

        // Use this for initialization
        void Start()
        {
            toggle = transform.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((isOn) =>
            {
                ChangeIt(isOn);
            });
            ChangeIt(toggle.isOn);
        }

        void ChangeIt(bool isOn)
        {
            for (int i = 0; i < graphicItems.Count; i++)
            {
                MaskableGraphic graphic = graphicItems[i];
                if (graphic)
                {
                    graphic.color = isOn ? OnColor : OffColor;
                }
            }
        }

    }
}
