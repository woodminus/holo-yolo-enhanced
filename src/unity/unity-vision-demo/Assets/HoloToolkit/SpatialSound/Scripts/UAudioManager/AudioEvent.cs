// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// The different rules for how audio should be played back.
    /// </summary>
    public enum AudioContainerType
    {
        Random,
        Sequence,
        Simultaneous,
        ContinuousSequence,
        ContinuousRandom
    }

    /// <summary>
    /// Defines the behavior for when the instance limit is reached for a particular event.
    /// </summary>
    public enum AudioEventInstanceBehavior
    {
        KillOldest,
        KillNewest
    }

    /// <summary>
    /// The different types of spatial positioning.
    /// </summary>
    public enum SpatialPositioningType
    {
        TwoD,           // Stereo
        ThreeD,         // 3D audio
        SpatialSound,   // Microsoft Spatial Sound
    }

    /