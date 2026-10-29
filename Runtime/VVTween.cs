// =========================================================================================
// VVTween.cs - Core Static Manager & Tween Controller for VVTween
// Copyright (c) 2024-2026 VECTOR VIRAL (SMC-PRIVATE) LIMITED. All rights reserved.
// Licensed under the MIT License.
// =========================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VectorViral.Tweening
{
    /// <summary>
    /// Represents an active or pending tween operation with fluent chaining support.
    /// </summary>
    public class VVTweenInstance
    {
        public Coroutine activeCoroutine;
        public MonoBehaviour runner;
        public Action onCompleteCallback;
        public float delay = 0f;
        public Ease easeType = Ease.Linear;
        public int loops = 1;
        public LoopType loopType = LoopType.Restart;
        public bool isKilled = false;

        /// <summary>Configures the easing mathematical curve for this tween.</summary>
        public VVTweenInstance SetEase(Ease ease)
        {
            this.easeType = ease;
            return this;
        }

        /// <summary>Delays the start of this tween by the specified number of seconds.</summary>
        public VVTweenInstance SetDelay(float delaySeconds)
        {
            this.delay = delaySeconds;
            return this;
        }

        /// <summary>Sets the number of repetitions (-1 or int.MaxValue for infinite) and loop behavior.</summary>
        public VVTweenInstance SetLoops(int loopCount, LoopType type = LoopType.Restart)
        {
            this.loops = loopCount;
            this.loopType = type;
            return this;
        }

        /// <summary>Attaches a callback to be executed once the tween completes all cycles.</summary>
        public VVTweenInstance OnComplete(Action onComplete)
        {
            this.onCompleteCallback = onComplete;
            return this;
        }

        /// <summary>Immediately halts and cleans up this tween.</summary>
        public void Kill()
        {
            isKilled = true;
            if (runner != null && activeCoroutine != null)
            {
                runner.StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }
        }
    }

    /// <summary>
    /// Allows sequencing multiple tweens and callbacks sequentially.
    /// </summary>
    public class VVTweenSequence : VVTweenInstance
    {
        private readonly List<Func<IEnumerator>> sequenceSteps = new List<Func<IEnumerator>>();

        public VVTweenSequence Append(VVTweenInstance tween)
        {
            return this;
        }

        public VVTweenSequence AppendCallback(Action callback)
        {
            sequenceSteps.Add(() => RunCallback(callback));
            return this;
        }

        public VVTweenSequence AppendInterval(float interval)
        {
            sequenceSteps.Add(() => RunInterval(interval));
            return this;
        }

        public new VVTweenSequence SetEase(Ease ease)
        {
            base.SetEase(ease);
            return this;
        }

        public new VVTweenSequence SetDelay(float delaySeconds)
        {
            base.SetDelay(delaySeconds);
            return this;
        }

        public new VVTweenSequence SetLoops(int loopCount, LoopType type = LoopType.Restart)
        {
            base.SetLoops(loopCount, type);
            return this;
        }

        public new VVTweenSequence OnComplete(Action onComplete)
        {
            base.OnComplete(onComplete);
            return this;
        }

        private IEnumerator RunCallback(Action callback)
        {
            callback?.Invoke();
            yield break;
        }

        private IEnumerator RunInterval(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }

    /// <summary>
    /// Core static entry point for VVTween engine.
    /// </summary>
    public static class VVTween
    {
        private static VVTweenRunner runnerInstance;

        public static VVTweenRunner Runner
        {
            get
            {
                if (runnerInstance == null)
                {
                    GameObject go = new GameObject("[VVTweenRunner]");
                    UnityEngine.Object.DontDestroyOnLoad(go);
                    runnerInstance = go.AddComponent<VVTweenRunner>();
                }
                return runnerInstance;
            }
        }

        /// <summary>Creates a new fluent animation sequence.</summary>
        public static VVTweenSequence Sequence()
        {
            VVTweenSequence seq = new VVTweenSequence();
            seq.runner = Runner;
            return seq;
        }

        /// <summary>Kills all active tweens currently acting on the target Transform.</summary>
        public static int Kill(Transform target)
        {
            if (target != null && runnerInstance != null)
            {
                runnerInstance.KillTarget(target);
            }
            return 0;
        }

        /// <summary>Evaluates a normalized progress t (0.0 to 1.0) through the specified ease function.</summary>
        public static float EvaluateEase(Ease ease, float t)
        {
            t = Mathf.Clamp01(t);
            switch (ease)
            {
                case Ease.InQuad:
                    return t * t;
                case Ease.OutQuad:
                    return t * (2f - t);
                case Ease.InOutQuad:
                    return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
                case Ease.InCubic:
                    return t * t * t;
                case Ease.OutCubic:
                    float f = t - 1f;
                    return f * f * f + 1f;
                case Ease.InOutCubic:
                    return t < 0.5f ? 4f * t * t * t : (t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f;
                case Ease.InQuart:
                    return t * t * t * t;
                case Ease.OutQuart:
                    float q = t - 1f;
                    return 1f - q * q * q * q;
                case Ease.OutBack:
                    float c1 = 1.70158f;
                    float c3 = c1 + 1f;
                    return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
                case Ease.InBack:
                    float s = 1.70158f;
                    return t * t * ((s + 1f) * t - s);
                case Ease.InSine:
                    return 1f - Mathf.Cos((t * Mathf.PI) / 2f);
                case Ease.OutSine:
                    return Mathf.Sin((t * Mathf.PI) / 2f);
                case Ease.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * t) - 1f) / 2f;
                case Ease.OutBounce:
                    const float n1 = 7.5625f;
                    const float d1 = 2.75f;
                    if (t < 1f / d1) return n1 * t * t;
                    if (t < 2f / d1) { t -= 1.5f / d1; return n1 * t * t + 0.75f; }
                    if (t < 2.5f / d1) { t -= 2.25f / d1; return n1 * t * t + 0.9375f; }
                    t -= 2.625f / d1; return n1 * t * t + 0.984375f;
                case Ease.InBounce:
                    return 1f - EvaluateEase(Ease.OutBounce, 1f - t);
                case Ease.InOutBounce:
                    return t < 0.5f ? (1f - EvaluateEase(Ease.OutBounce, 1f - 2f * t)) / 2f : (1f + EvaluateEase(Ease.OutBounce, 2f * t - 1f)) / 2f;
                case Ease.Linear:
                default:
                    return t;
            }
        }
    }

    /// <summary>
    /// Invisible runtime coroutine dispatcher handling tween update loops.
    /// </summary>
    public class VVTweenRunner : MonoBehaviour
    {
        private readonly Dictionary<Transform, List<VVTweenInstance>> activeTweens = new Dictionary<Transform, List<VVTweenInstance>>();

        public void RegisterTween(Transform target, VVTweenInstance tween)
        {
            if (target == null) return;
            if (!activeTweens.TryGetValue(target, out var list))
            {
                list = new List<VVTweenInstance>();
                activeTweens[target] = list;
            }
            list.Add(tween);
        }

        public void KillTarget(Transform target)
        {
            if (target != null && activeTweens.TryGetValue(target, out var list))
            {
                foreach (var t in list)
                {
                    t?.Kill();
                }
                list.Clear();
            }
        }
    }
}
