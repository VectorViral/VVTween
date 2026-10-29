// =========================================================================================
// VVTweenExtensions.cs - Component Extension Methods & Namespace Compatibility
// Copyright (c) 2024-2026 VECTOR VIRAL (SMC-PRIVATE) LIMITED. All rights reserved.
// Licensed under the MIT License.
// =========================================================================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VectorViral.Tweening;

namespace VectorViral.Tweening
{
    public static class VVTweenExtensions
    {
        public static VVTweenInstance DOLocalMove(this Transform target, Vector3 endValue, float duration)
        {
            VVTweenInstance tween = new VVTweenInstance { runner = VVTween.Runner };
            VVTween.Runner.RegisterTween(target, tween);
            tween.activeCoroutine = VVTween.Runner.StartCoroutine(LocalMoveCoroutine(target, endValue, duration, tween));
            return tween;
        }

        public static VVTweenInstance DOMove(this Transform target, Vector3 endValue, float duration)
        {
            return DOLocalMove(target, endValue, duration);
        }

        public static VVTweenInstance DOLocalRotate(this Transform target, Vector3 endValue, float duration)
        {
            VVTweenInstance tween = new VVTweenInstance { runner = VVTween.Runner };
            VVTween.Runner.RegisterTween(target, tween);
            tween.activeCoroutine = VVTween.Runner.StartCoroutine(LocalRotateCoroutine(target, endValue, duration, tween));
            return tween;
        }

        public static VVTweenInstance DORotate(this Transform target, Vector3 endValue, float duration)
        {
            return DOLocalRotate(target, endValue, duration);
        }

        public static VVTweenInstance DOScale(this Transform target, Vector3 endValue, float duration)
        {
            VVTweenInstance tween = new VVTweenInstance { runner = VVTween.Runner };
            VVTween.Runner.RegisterTween(target, tween);
            tween.activeCoroutine = VVTween.Runner.StartCoroutine(ScaleCoroutine(target, endValue, duration, tween));
            return tween;
        }

        public static VVTweenInstance DOScale(this Transform target, float endValue, float duration)
        {
            return DOScale(target, new Vector3(endValue, endValue, endValue), duration);
        }

        public static VVTweenInstance DOPath(this Transform target, Vector3[] path, float duration, PathType pathType = PathType.Linear)
        {
            VVTweenInstance tween = new VVTweenInstance { runner = VVTween.Runner };
            VVTween.Runner.RegisterTween(target, tween);
            if (path != null && path.Length > 0)
            {
                Vector3 endValue = path[path.Length - 1];
                tween.activeCoroutine = VVTween.Runner.StartCoroutine(LocalMoveCoroutine(target, endValue, duration, tween));
            }
            return tween;
        }

        public static void DOKill(this Transform target)
        {
            if (target != null)
            {
                VVTween.Kill(target);
            }
        }

        public static void DORestart(this Transform target)
        {
        }

        public static VVTweenInstance DOFade(this CanvasGroup target, float endValue, float duration)
        {
            VVTweenInstance tween = new VVTweenInstance { runner = VVTween.Runner };
            if (target != null)
            {
                VVTween.Runner.RegisterTween(target.transform, tween);
                tween.activeCoroutine = VVTween.Runner.StartCoroutine(FadeCanvasGroupCoroutine(target, endValue, duration, tween));
            }
            return tween;
        }

        public static VVTweenInstance DOFade(this SpriteRenderer target, float endValue, float duration)
        {
            VVTweenInstance tween = new VVTweenInstance { runner = VVTween.Runner };
            if (target != null)
            {
                VVTween.Runner.RegisterTween(target.transform, tween);
                tween.activeCoroutine = VVTween.Runner.StartCoroutine(FadeSpriteCoroutine(target, endValue, duration, tween));
            }
            return tween;
        }

        public static VVTweenInstance DOFillAmount(this Image target, float endValue, float duration)
        {
            VVTweenInstance tween = new VVTweenInstance { runner = VVTween.Runner };
            if (target != null)
            {
                VVTween.Runner.RegisterTween(target.transform, tween);
                tween.activeCoroutine = VVTween.Runner.StartCoroutine(FillAmountCoroutine(target, endValue, duration, tween));
            }
            return tween;
        }

        public static void DOKill(this Image target)
        {
            if (target != null)
            {
                VVTween.Kill(target.transform);
            }
        }

        private static IEnumerator LocalMoveCoroutine(Transform target, Vector3 endValue, float duration, VVTweenInstance tween)
        {
            if (tween.delay > 0f)
                yield return new WaitForSeconds(tween.delay);

            if (target == null || tween.isKilled) yield break;

            Vector3 startValue = target.localPosition;
            if (duration <= 0.001f)
            {
                target.localPosition = endValue;
                tween.onCompleteCallback?.Invoke();
                yield break;
            }

            int loopCounter = 0;
            int maxLoops = tween.loops <= 0 ? int.MaxValue : tween.loops;

            while (loopCounter < maxLoops && !tween.isKilled && target != null)
            {
                float elapsed = 0f;
                Vector3 from = (tween.loopType == LoopType.Yoyo && loopCounter % 2 == 1) ? endValue : startValue;
                Vector3 to = (tween.loopType == LoopType.Yoyo && loopCounter % 2 == 1) ? startValue : endValue;

                while (elapsed < duration && !tween.isKilled && target != null)
                {
                    elapsed += Time.deltaTime;
                    float t = VVTween.EvaluateEase(tween.easeType, elapsed / duration);
                    target.localPosition = Vector3.LerpUnclamped(from, to, t);
                    yield return null;
                }

                if (target != null && !tween.isKilled)
                {
                    target.localPosition = to;
                }

                loopCounter++;
            }

            if (!tween.isKilled)
            {
                tween.onCompleteCallback?.Invoke();
            }
        }

        private static IEnumerator LocalRotateCoroutine(Transform target, Vector3 endEuler, float duration, VVTweenInstance tween)
        {
            if (tween.delay > 0f)
                yield return new WaitForSeconds(tween.delay);

            if (target == null || tween.isKilled) yield break;

            Quaternion startRot = target.localRotation;
            Quaternion endRot = Quaternion.Euler(endEuler);

            if (duration <= 0.001f)
            {
                target.localRotation = endRot;
                tween.onCompleteCallback?.Invoke();
                yield break;
            }

            int loopCounter = 0;
            int maxLoops = tween.loops <= 0 ? int.MaxValue : tween.loops;

            while (loopCounter < maxLoops && !tween.isKilled && target != null)
            {
                float elapsed = 0f;
                Quaternion from = (tween.loopType == LoopType.Yoyo && loopCounter % 2 == 1) ? endRot : startRot;
                Quaternion to = (tween.loopType == LoopType.Yoyo && loopCounter % 2 == 1) ? startRot : endRot;

                while (elapsed < duration && !tween.isKilled && target != null)
                {
                    elapsed += Time.deltaTime;
                    float t = VVTween.EvaluateEase(tween.easeType, elapsed / duration);
                    target.localRotation = Quaternion.SlerpUnclamped(from, to, t);
                    yield return null;
                }

                if (target != null && !tween.isKilled)
                {
                    target.localRotation = to;
                }

                loopCounter++;
            }

            if (!tween.isKilled)
            {
                tween.onCompleteCallback?.Invoke();
            }
        }

        private static IEnumerator ScaleCoroutine(Transform target, Vector3 endValue, float duration, VVTweenInstance tween)
        {
            if (tween.delay > 0f)
                yield return new WaitForSeconds(tween.delay);

            if (target == null || tween.isKilled) yield break;

            Vector3 startValue = target.localScale;
            if (duration <= 0.001f)
            {
                target.localScale = endValue;
                tween.onCompleteCallback?.Invoke();
                yield break;
            }

            int loopCounter = 0;
            int maxLoops = tween.loops <= 0 ? int.MaxValue : tween.loops;

            while (loopCounter < maxLoops && !tween.isKilled && target != null)
            {
                float elapsed = 0f;
                Vector3 from = (tween.loopType == LoopType.Yoyo && loopCounter % 2 == 1) ? endValue : startValue;
                Vector3 to = (tween.loopType == LoopType.Yoyo && loopCounter % 2 == 1) ? startValue : endValue;

                while (elapsed < duration && !tween.isKilled && target != null)
                {
                    elapsed += Time.deltaTime;
                    float t = VVTween.EvaluateEase(tween.easeType, elapsed / duration);
                    target.localScale = Vector3.LerpUnclamped(from, to, t);
                    yield return null;
                }

                if (target != null && !tween.isKilled)
                {
                    target.localScale = to;
                }

                loopCounter++;
            }

            if (!tween.isKilled)
            {
                tween.onCompleteCallback?.Invoke();
            }
        }

        private static IEnumerator FadeCanvasGroupCoroutine(CanvasGroup target, float endValue, float duration, VVTweenInstance tween)
        {
            if (tween.delay > 0f)
                yield return new WaitForSeconds(tween.delay);

            if (target == null || tween.isKilled) yield break;

            float startValue = target.alpha;
            if (duration <= 0.001f)
            {
                target.alpha = endValue;
                tween.onCompleteCallback?.Invoke();
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < duration && !tween.isKilled && target != null)
            {
                elapsed += Time.deltaTime;
                float t = VVTween.EvaluateEase(tween.easeType, elapsed / duration);
                target.alpha = Mathf.Lerp(startValue, endValue, t);
                yield return null;
            }

            if (target != null && !tween.isKilled)
            {
                target.alpha = endValue;
                tween.onCompleteCallback?.Invoke();
            }
        }

        private static IEnumerator FadeSpriteCoroutine(SpriteRenderer target, float endValue, float duration, VVTweenInstance tween)
        {
            if (tween.delay > 0f)
                yield return new WaitForSeconds(tween.delay);

            if (target == null || tween.isKilled) yield break;

            Color c = target.color;
            float startAlpha = c.a;

            if (duration <= 0.001f)
            {
                c.a = endValue;
                target.color = c;
                tween.onCompleteCallback?.Invoke();
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < duration && !tween.isKilled && target != null)
            {
                elapsed += Time.deltaTime;
                float t = VVTween.EvaluateEase(tween.easeType, elapsed / duration);
                c.a = Mathf.Lerp(startAlpha, endValue, t);
                target.color = c;
                yield return null;
            }

            if (target != null && !tween.isKilled)
            {
                c.a = endValue;
                target.color = c;
                tween.onCompleteCallback?.Invoke();
            }
        }

        private static IEnumerator FillAmountCoroutine(Image target, float endValue, float duration, VVTweenInstance tween)
        {
            if (tween.delay > 0f)
                yield return new WaitForSeconds(tween.delay);

            if (target == null || tween.isKilled) yield break;

            float startFill = target.fillAmount;
            if (duration <= 0.001f)
            {
                target.fillAmount = endValue;
                tween.onCompleteCallback?.Invoke();
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < duration && !tween.isKilled && target != null)
            {
                elapsed += Time.deltaTime;
                float t = VVTween.EvaluateEase(tween.easeType, elapsed / duration);
                target.fillAmount = Mathf.Lerp(startFill, endValue, t);
                yield return null;
            }

            if (target != null && !tween.isKilled)
            {
                target.fillAmount = endValue;
                tween.onCompleteCallback?.Invoke();
            }
        }
    }
}

// -----------------------------------------------------------------------------------------
// VV.Tweening Namespace Shortcut
// -----------------------------------------------------------------------------------------
namespace VV.Tweening
{
    public static class VVTween
    {
        public static VVTweenSequence Sequence() => VectorViral.Tweening.VVTween.Sequence();
        public static int Kill(Transform target) => VectorViral.Tweening.VVTween.Kill(target);
    }
}
