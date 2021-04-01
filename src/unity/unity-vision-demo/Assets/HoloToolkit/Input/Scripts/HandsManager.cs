// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine.VR.WSA.Input;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public partial class HandsManager : Singleton<HandsManager>
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>
        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        /// <summary>
        /// Occurs when users hand is detected or lost.
        /// </summary>
        /// <param name="handDetected">True if a hand is Detected, else false.</param>
        public delegate void HandInViewDelegate(bool handDetected);
        public event HandInViewDelegate HandInView;

        private HashSet<uint> trackedHands = new HashSet<uint>();

        private void Awake()
        {
            InteractionManager.SourceDetected += InteractionManager_SourceDetected;
            InteractionManager.SourceLost += In