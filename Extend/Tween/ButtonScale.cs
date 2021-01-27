using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace lgu3d
{
    public class ButtonScale : MonoBehaviour, UIEventSystemsInterface
    {
        public Vector3 hover = new Vector3(1.0f, 1f, 1f);
        public Vector3 pressed = new Vector3(1.25f, 1.25f, 1.25f);
        public float duration = 0.1f;
        public Ease ease = Ease.InOutBack;

        private Vector3 oldScale = Vector3.one;
        private Button m_button;

        private void Start()
        {
            oldScale = transform.localScale;
            m_button = transform.GetComponent<Button>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ScaleTo(pressed);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //ScaleTo(pressed);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //ScaleTo(oldScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ScaleTo(oldScale);
        }

        public void ScaleTo(Vector3 to)
        {
            if (m_button)
            {
                if (m_button.interactable)
                {
                    transform.DOScale(to, duration).SetEase(ease);
                }
            }
            else
            {
                transform.DOScale(to, duration).SetEase(ease);
            }
        }
    }
}