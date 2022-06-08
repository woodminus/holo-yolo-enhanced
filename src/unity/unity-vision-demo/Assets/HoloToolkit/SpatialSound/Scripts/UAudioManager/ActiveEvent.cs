// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace HoloToolkit.Unity
{
    /// <summary>
    /// Currently active AudioEvents along with their AudioSource components for instance limiting events
    /// </summary>
    public class ActiveEvent : IDisposable
    {
        private AudioSource primarySource = null;
        public AudioSource PrimarySource
        {
            get
            {
                return primarySource;
            }
            private set
            {
                primarySource = value;
                if (primarySource != null)
                {
                    primarySource.enabled = true;
                }
            }
        }

        private AudioSource secondarySource = null;
        public AudioSource SecondarySource
        {
            get
            {
                return secondarySource;
            }
            private set
            {
                secondarySource = value;
                if (secondarySource != null)
                {
                    secondarySource.enabled = true;
                }
            }
        }

        public bool IsPlaying
        {
            get
            {
                return
                    (primarySource != null && primarySource.isPlaying) ||
                    (secondarySource != null && secondarySource.isPlaying);
            }
        }

        public GameObject AudioEmitter
        {
            get;
            priva