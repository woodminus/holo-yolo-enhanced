// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// A component for moving an object via the GestureManager manipulation gesture.
    /// </summary>
    /// <remarks>
    /// When an active GestureManipulator component is attached to a GameObject it will subscribe
    /// to GestureManager's manipulation gestures, and move the GameObject when a ManipulationGesture occurs.
    /// If the GestureManipulator is disabled it will not respond to any manipulation gestures.
    /// 
    /// This means that if multiple GestureManipulators are active in a given scene when a manipulation
    /// gesture is performed, all the releva