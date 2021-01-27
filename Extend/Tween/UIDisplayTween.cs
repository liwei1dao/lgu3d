using UnityEngine;
using DG.Tweening;


namespace lgu3d
{
    /// <summary>
    /// 左右，上下，透明度变化，缩放等的ui动画
    /// </summary>
    public static class UIDisplayTween
    {
        //public static Ease ease = Ease.InOutBack;
        public static float duration = 0.5f;
        //public static float delay = 0.35f;
        public static Vector3 outXPos = new Vector3(-1366, 0, 0); //left
        public static Vector3 outYPos = new Vector3(0, 768, 0); //top

        /// <summary>
        /// 显示界面的中间从小到大的缩放动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="callbackOnComplete"></param>
        public static void ShowScaleTween(this Transform uiTr, TweenCallback callbackOnComplete = null, Ease ease = Ease.OutBack, float delay = 0.2f)
        {
            DOTween.Kill(uiTr);
            ObjDefaultValue objDefaultValue = uiTr.GetOrAddComponent<ObjDefaultValue>();
            uiTr.localPosition = objDefaultValue.defaultLocalPos;
            uiTr.localScale = Vector3.one * 0.1f;
            uiTr.DOScale(Vector3.one, duration).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
            InternalAlphaTween(uiTr, true, null, delay);
        }

        /// <summary>
        /// 显示界面的中间从小到大的缩放动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="callbackOnComplete"></param>
        public static void ShowScaleTweenWithDuration(this Transform uiTr, float _duration, TweenCallback callbackOnComplete = null, Ease ease = Ease.OutBack, float delay = 0.2f)
        {
            DOTween.Kill(uiTr);
            ObjDefaultValue objDefaultValue = uiTr.GetOrAddComponent<ObjDefaultValue>();
            uiTr.localPosition = objDefaultValue.defaultLocalPos;
            uiTr.localScale = Vector3.one * 0.1f;
            uiTr.DOScale(Vector3.one, _duration).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
            InternalAlphaTween(uiTr, true, null, delay);
        }
        /// <summary>
        /// 隐藏缩放动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="callbackOnComplete"></param>
        public static void HideScaleTween(this Transform uiTr, TweenCallback callbackOnComplete = null, Ease ease = Ease.InBack, float delay = 0.2f)
        {
            DOTween.Kill(uiTr);
            ObjDefaultValue objDefaultValue = uiTr.GetOrAddComponent<ObjDefaultValue>();
            uiTr.localPosition = objDefaultValue.defaultLocalPos;
            uiTr.DOScale(Vector3.one * 0.1f, duration).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
            InternalAlphaTween(uiTr, false, null, delay);
        }

        /// <summary>
        /// 水平平移动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="isShow">是否从左到右</param>
        /// <param name="callbackOnComplete"></param>
        public static void ShowHorizontalTween(this Transform uiTr, bool isShow = false,
            TweenCallback callbackOnComplete = null, bool isLeft = true,
            Ease ease = Ease.InOutBack, float delay = 0.2f)
        {
            DOTween.Kill(uiTr);
            ObjDefaultValue objDefaultValue = uiTr.GetOrAddComponent<ObjDefaultValue>();
            uiTr.localScale = Vector3.one;
            RectTransform mRt = uiTr.GetComponent<RectTransform>();
            if (isShow)
            {
                uiTr.localPosition = isLeft ? outXPos : -outXPos;
                if (mRt)
                {
                    mRt.DOAnchorPos3D(objDefaultValue.defaultAnchoredPosition3D, duration)
                    .SetEase(ease)
                    .OnComplete(callbackOnComplete)
                    .SetDelay(delay);
                }
                else
                {
                    uiTr.DOLocalMove(objDefaultValue.defaultLocalPos, duration)
                    .SetEase(ease)
                    .OnComplete(callbackOnComplete)
                    .SetDelay(delay);
                }


                InternalAlphaTween(uiTr, true, null, delay);
            }
            else
            {
                uiTr.DOLocalMove(isLeft ? -outXPos : outXPos, 0.35f).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
                InternalAlphaTween(uiTr, false, null, delay);
            }
        }

        /// <summary>
        /// 垂直运动动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="isShow"></param>
        /// <param name="callbackOnComplete"></param>
        public static void ShowVerticalTween(this Transform uiTr, bool isShow = true,
            TweenCallback callbackOnComplete = null, bool isTop = true,
            Ease ease = Ease.InOutBack, float delay = 0.2f)
        {
            uiTr.DOKill();
            ObjDefaultValue objDefaultValue = uiTr.GetOrAddComponent<ObjDefaultValue>();
            uiTr.localScale = Vector3.one;
            RectTransform mRt = uiTr.GetComponent<RectTransform>();
            if (isShow)
            {
                uiTr.localPosition = isTop ? outYPos : -outYPos;
                if (mRt)
                {
                    mRt.DOAnchorPos3D(objDefaultValue.defaultAnchoredPosition3D, duration)
                        .SetEase(ease)
                        .OnComplete(() =>
                        {
                            if (callbackOnComplete != null) callbackOnComplete();
                        })
                        .SetDelay(delay);
                }
                else
                {
                    uiTr.DOLocalMove(objDefaultValue.defaultLocalPos, duration)
                        .SetEase(ease)
                        .OnComplete(() =>
                        {
                            Debug.LogError("ShowVerticalTween:" + objDefaultValue.defaultLocalPos);
                            if (callbackOnComplete != null) callbackOnComplete();
                        })
                        .SetDelay(delay);
                }
                InternalAlphaTween(uiTr, true, null, delay);
            }
            else
            {
                if (mRt)
                {
                    mRt.DOAnchorPos3D(isTop ? -outYPos : outYPos, duration).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
                }
                else
                {
                    uiTr.DOLocalMove(isTop ? -outYPos : outYPos, duration).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
                }

                InternalAlphaTween(uiTr, false, null, delay);
            }
        }

        private static void InternalAlphaTween(Transform uiTr, bool isShow = true, TweenCallback callbackOnComplete = null, float delay = 0.2f)
        {
            CanvasGroup group = uiTr.GetOrAddComponent<CanvasGroup>();

            if (isShow)
            {
                group.alpha = 0;
                DOTween.To(x => group.alpha = x, 0, 1, duration + 0.1f).OnComplete(callbackOnComplete).SetDelay(delay);
            }
            else
            {
                group.alpha = 1;
                DOTween.To(x => group.alpha = x, 1, 0, duration + 0.1f).OnComplete(callbackOnComplete).SetDelay(delay);
            }
        }

        /// <summary>
        /// 只显示alpha变化的动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="isShow"></param>
        /// <param name="callbackOnComplete"></param>
        public static void ShowAlphaTween(this Transform uiTr, bool isShow = true, TweenCallback callbackOnComplete = null, Ease ease = Ease.InBack, float delay = 0.2f, float duration = 0.35f)
        {
            DOTween.Kill(uiTr);
            ObjDefaultValue objDefaultValue = uiTr.GetOrAddComponent<ObjDefaultValue>();
            CanvasGroup group = uiTr.GetOrAddComponent<CanvasGroup>();
            uiTr.localScale = objDefaultValue.defaultLocalScale;
            uiTr.localPosition = objDefaultValue.defaultLocalPos;
            if (isShow)
            {
                group.alpha = 0;
                DOTween.To(x => group.alpha = x, 0, 1, duration).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
            }
            else
            {
                group.alpha = 1;
                DOTween.To(x => group.alpha = x, 1, 0, duration).SetEase(ease).OnComplete(callbackOnComplete).SetDelay(delay);
            }
        }

        /// <summary>
        /// 随机打开运动动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="isShow"></param>
        /// <param name="callbackOnComplete"></param>
        public static void ShowRandomTween(this Transform uiTr, TweenCallback callbackOnComplete = null)
        {
            DOTween.Kill(uiTr);
            int ran = Random.Range(0, 6);
            switch (ran)
            {
                case 0:
                    uiTr.ShowScaleTween(callbackOnComplete);
                    break;

                case 1:
                    uiTr.ShowHorizontalTween(true, callbackOnComplete);
                    break;

                case 2:
                    uiTr.ShowVerticalTween(true, callbackOnComplete);
                    break;

                case 3:
                    uiTr.ShowAlphaTween(true, callbackOnComplete);
                    break;

                case 4:
                    uiTr.ShowHorizontalTween(true, callbackOnComplete, false);
                    break;

                case 5:
                    uiTr.ShowVerticalTween(true, callbackOnComplete, false);
                    break;
            }
        }

        /// <summary>
        /// 随机隐藏动画
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="callbackOnComplete"></param>
        public static void HideRandomTween(this Transform uiTr, TweenCallback callbackOnComplete = null)
        {
            int ran = Random.Range(0, 6);
            switch (ran)
            {
                case 0:
                    uiTr.HideScaleTween(callbackOnComplete);
                    break;

                case 1:
                    uiTr.ShowHorizontalTween(false, callbackOnComplete);
                    break;

                case 2:
                    uiTr.ShowVerticalTween(false, callbackOnComplete);
                    break;

                case 3:
                    uiTr.ShowAlphaTween(false, callbackOnComplete);
                    break;

                case 4:
                    uiTr.ShowHorizontalTween(false, callbackOnComplete, false);
                    break;

                case 5:
                    uiTr.ShowVerticalTween(false, callbackOnComplete, false);
                    break;
            }
        }
    }
}