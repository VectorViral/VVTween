// =========================================================================================
// VVTweenEnums.cs - Core Enums for VVTween Animation Engine
// Copyright (c) 2024-2026 VECTOR VIRAL (SMC-PRIVATE) LIMITED. All rights reserved.
// Licensed under the MIT License.
// =========================================================================================

namespace VectorViral.Tweening
{
    /// <summary>
    /// Easing curve formulas supported natively by VVTween.
    /// </summary>
    public enum Ease
    {
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InSine,
        OutSine,
        InOutSine,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce
    }

    /// <summary>
    /// Behavior when repeating a tween loop.
    /// </summary>
    public enum LoopType
    {
        /// <summary>Each cycle restarts from the beginning value.</summary>
        Restart,
        /// <summary>Each alternate cycle reverses direction (ping-pong).</summary>
        Yoyo,
        /// <summary>Each cycle starts from the previous end value and accumulates.</summary>
        Incremental
    }

    /// <summary>
    /// Path interpolation modes for multi-point trajectory movements.
    /// </summary>
    public enum PathType
    {
        Linear,
        CatmullRom
    }
}
