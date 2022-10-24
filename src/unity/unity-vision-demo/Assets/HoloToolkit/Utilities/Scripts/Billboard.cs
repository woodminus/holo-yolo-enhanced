// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity
{
    public enum PivotAxis
    {
        // Rotate about all axes.
        Free,
        // Rotate about an individual axis.
        X,
        Y
    }

    /// <summary>
    /// The Billboard class implements the behaviors needed to keep a GameObject
    /// oriented towards the user.
    /// </summary>
  