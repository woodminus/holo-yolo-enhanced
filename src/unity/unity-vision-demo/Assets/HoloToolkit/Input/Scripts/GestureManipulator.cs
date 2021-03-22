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
    /// gesture is performed, all the relevant GameObjects will be moved.  If the desired behavior is that only
    /// a single object be moved at a time, it is recommended that objects which should not be moved disable
    /// their GestureManipulators, then re-enable them when necessary (e.g. the object is focused).
    /// </remarks>
    public class GestureManipulator : MonoBehaviour
    {
        [Tooltip("How much to scale each axis of hand movement (camera relative) when manipulating the object")]
        public Vector3 handPositionScale = new Vector3(2.0f, 2.0f, 4.0f);  // Default tuning values, expected to be modified per application

        private Vector3 initialHandPosition;

        private Vector3 initialObjectPosition;

        private Interpolator targetInterpolator;

        private GestureManager gestureManager;

        private bool Manipulating { get; set; }

