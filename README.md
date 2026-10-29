# VVTween (VectorViral Tweening)

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Redistribution: Allowed](https://img.shields.io/badge/Redistribution-100%25%20Free%20%26%20Permitted-brightgreen.svg)]()
[![Unity 2021.3+](https://img.shields.io/badge/Unity-2021.3%2B-blue.svg)](https://unity.com/)
[![Zero Dependencies](https://img.shields.io/badge/Dependencies-Zero-brightgreen.svg)]()
[![White Label](https://img.shields.io/badge/White%20Label-100%25-orange.svg)]()

**VVTween** is an ultra-lightweight, zero-dependency, open-source tweening and animation engine built for Unity. Designed and published by **VECTOR VIRAL (SMC-PRIVATE) LIMITED** under the **MIT License** with an explicit **Universal Free Redistribution Grant**.

> **100% FREE FOR COMMERCIAL REDISTRIBUTION:**
> Anyone is free to copy, modify, remake, bundle, and redistribute VVTween's source code inside commercial game templates, assets, or projects sold on CodeCanyon, Codester, itch.io, Unity Asset Store, or any marketplace with **zero royalties and zero licensing restrictions**.

---

## ⚡ Highlights

- **Universal Redistribution Rights:** Explicitly licensed for redistribution in resold source code templates and commercial games.
- **Zero External Dependencies:** 100% pure C# running natively on standard Unity Coroutines and math.
- **Fluent & Familiar API:** Drop-in syntax for common tween calls (`DOLocalMove`, `DOLocalRotate`, `DOScale`, `DOFade`, `DOFillAmount`).
- **Comprehensive Easing Library:** Native implementations of Linear, Quad, Cubic, Quart, Sine, Back, and Bounce curves.
- **Garbage-Collector Friendly:** Zero runtime allocations during steady-state tween progression.
- **Full Unity LTS Support:** Validated on Unity 2021.3 LTS, 2022.3 LTS, and Unity 6.

---

## 📦 Installation

### Option 1: Unity Package Manager (UPM via Git URL)
1. Open your Unity project.
2. Navigate to **Window > Package Manager**.
3. Click the **`+`** button in the top-left corner and select **Add package from git URL...**.
4. Enter:
   ```text
   https://github.com/VectorViral/VVTween.git
   ```
5. Click **Add**.

### Option 2: Direct Folder Import
Copy the `Runtime` folder directly into your Unity project's `Assets/Plugins/VVTween/` or `Assets/Scripts/VVTween/`.

---

## 🚀 Quick Start

### 1. Basic Transform Tweens
```csharp
using UnityEngine;
using VectorViral.Tweening; // or using VV.Tweening;

public class ExampleTween : MonoBehaviour
{
    void Start()
    {
        // Move locally to (0, 5, 0) over 1 second with OutQuad easing
        transform.DOLocalMove(new Vector3(0, 5, 0), 1.0f)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => Debug.Log("Movement finished!"));

        // Smooth local rotation
        transform.DOLocalRotate(new Vector3(0, 180, 0), 0.75f)
                 .SetEase(Ease.InOutSine);

        // Scale punch / ping-pong
        transform.DOScale(1.5f, 0.4f)
                 .SetLoops(2, LoopType.Yoyo)
                 .SetEase(Ease.OutBack);
    }
}
```

### 2. UI Transitions (CanvasGroup & Image)
```csharp
using UnityEngine;
using UnityEngine.UI;
using VectorViral.Tweening;

public class UIAnimationExample : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image radialProgressBar;

    public void ShowScreen()
    {
        // Fade in screen over 0.35s
        canvasGroup.DOFade(1.0f, 0.35f);

        // Animate circular fill amount from 0 to 1
        radialProgressBar.DOFillAmount(1.0f, 1.2f)
                         .SetEase(Ease.Linear);
    }

    public void HideScreen()
    {
        // Fade out screen
        canvasGroup.DOFade(0.0f, 0.25f);
    }
}
```

### 3. Killing Active Tweens
```csharp
// Kill all tweens running on this object
transform.DOKill();
```

---

## 📐 Supported Easing Curves

| Easing Type | In | Out | InOut |
|---|---|---|---|
| **Linear** | `Ease.Linear` | `Ease.Linear` | `Ease.Linear` |
| **Quad** | `Ease.InQuad` | `Ease.OutQuad` | `Ease.InOutQuad` |
| **Cubic** | `Ease.InCubic` | `Ease.OutCubic` | `Ease.InOutCubic` |
| **Quart** | `Ease.InQuart` | `Ease.OutQuart` | `Ease.InOutQuart` |
| **Sine** | `Ease.InSine` | `Ease.OutSine` | `Ease.InOutSine` |
| **Back** | `Ease.InBack` | `Ease.OutBack` | `Ease.InOutBack` |
| **Bounce** | `Ease.InBounce` | `Ease.OutBounce` | `Ease.InOutBounce` |

---

## 🔄 Migrating from DOTween

VVTween was architected with identical method signatures to standard tweening libraries for seamless drop-in compatibility.

Simply replace:
```csharp
// Old
using DG.Tweening;

// New VVTween
using VectorViral.Tweening;
// or
using VV.Tweening;
```

All standard calls (`DOLocalMove`, `DOLocalRotate`, `DOScale`, `DOFade`, `DOFillAmount`, `DOKill`, `SetEase`, `SetDelay`, `SetLoops`, `OnComplete`) remain 100% identical!

---

## 📄 License & Redistribution Rights

Licensed under the **MIT License** with an explicit **Universal Free Redistribution Grant**.  
Copyright (c) 2024-2026 VECTOR VIRAL (SMC-PRIVATE) LIMITED.

Anyone is free to copy, modify, merge, publish, distribute, sublicense, remake, and sell/redistribute copies of this software in any project or commercial template without fee or royalty. See [LICENSE.md](LICENSE.md) for full text.
