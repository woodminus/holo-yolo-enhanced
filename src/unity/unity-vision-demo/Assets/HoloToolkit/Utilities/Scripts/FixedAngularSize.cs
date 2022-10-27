// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// Causes a Hologram to maintain a fixed angular size, which is to say it
    /// occupies the same pixels in the view regardless of its distance from
    /// the camera.
    /// </summary>
    public class FixedAngularSize : MonoBehaviour
    {
        // The ratio between the transform's local scale and 