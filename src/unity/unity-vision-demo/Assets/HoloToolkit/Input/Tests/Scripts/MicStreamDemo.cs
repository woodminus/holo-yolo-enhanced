// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Text;

namespace HoloToolkit.Unity
{
    [RequireComponent(typeof(AudioSource))]
    public class MicStreamDemo : MonoBehaviour
    {
        /// <summary>
        /// Which type of microphone/quality to access
        /// </summary>
        public MicStream.StreamCategory StreamType = MicStream.StreamCategory.HIGH_QUALITY_VOICE;

        /// <summary>
        /// can boost volume here as desired. 1 is default but probably too quiet. can change during operation. 
        /// </summary>
        public float InputGain = 1;

        /// <summary>
        /// if keepAllData==false, you'll always get the newest data no matter how long the program hangs for any reason, but will lose some data if the program does hang 
        /// can only be set on initialization
        /// </summary>
        public 