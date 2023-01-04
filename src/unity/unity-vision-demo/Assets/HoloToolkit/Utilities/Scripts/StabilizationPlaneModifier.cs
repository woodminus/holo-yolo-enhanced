// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.VR.WSA;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// StabilizationPlaneModifier handles the setting of the stabilization plane in several ways.
    /// </summary>
    public class StabilizationPlaneModifier : Singleton<StabilizationPlaneModifier>
    {
        [Tooltip("Checking enables SetFocusPointForFrame to set the stabilization plane.")]
        public bool SetStabilizationPlane = true;
        [Tooltip("Lerp speed when moving focus point closer.")]
        public float LerpStabilizationPlanePowerCloser = 4.0f;
        [Tooltip("Lerp speed when moving focus point farther away.")]
        public float LerpStabilizationPlanePowerFarther = 7.0f;

        [SerializeField, Tooltip("Used to temporarily override the location of the stabilization plane.")]
        private Transform targetOverride;
        public Transform TargetOverride
        {
            get
            {
                return targetOverride;
            }
            set
            {
                if (targetOverride != value)
                {
                    targetOverride = value;
                    if (targetOverride)
                    {
                        targetOverridePreviousPosition = targetOverride.position;
                    }
                }
            }
        }

        [SerializeField, Tooltip("Keeps track of position-based velocity for the target object.")]
        private bool trackVelocity = false;
        public bool TrackVelocity
        {
            get
            {
                return trackVelocity;
            }
            set
            {
                trackVelocity = value;
                if (TargetOverride)
                {
                    targetOverridePreviousPosition = TargetOverride.position;
                }
            }
        }

        [Tooltip("Use the GazeManager class to set the plane to the gazed upon hologram.")]
        public bool UseGazeManager = true;

        [Tooltip("Default distance to set plane if plane is gaze-locked.")]
        public float DefaultPlaneDistance = 2.0f;

        [Tooltip("Visualize the pl