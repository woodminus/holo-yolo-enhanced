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
            private set;
        }

        public string MessageOnAudioEnd
        {
            get;
            private set;
        }

        public AudioEvent audioEvent = null;
        public bool isStoppable = true;
        public float volDest = 1;
        public float altVolDest = 1;
        public float currentFade = 0;
        public bool playingAlt = false;
        public bool isActiveTimeComplete = false;
        public float activeTime = 0;
        public bool cancelEvent = false;

        public ActiveEvent(AudioEvent audioEvent, GameObject emitter, AudioSource primarySource, AudioSource secondarySource, string messageOnAudioEnd = null)
        {
            this.audioEvent = audioEvent;
            AudioEmitter = emitter;
            PrimarySource = primarySource;
            SecondarySource = secondarySource;
            MessageOnAudioEnd = messageOnAudioEnd;
            SetSourceProperties();
        }

        public static AnimationCurve SpatialRolloff;

        /// <summary>
        /// Set the volume, spatialization, etc., on our AudioSources to match the settings on the event to play.
        /// </summary>
        private void SetSourceProperties()
        {
            Action<Action<AudioSource>> forEachSource = (action) =>
            {
                action(PrimarySource);
                if (SecondarySource != null)
                {
                    action(SecondarySource);
                }
            };

            AudioEvent audioEvent = this.audioEvent;
            switch (audioEvent.spatialization)
            {
                case SpatialPositioningType.TwoD:
                    forEachSource((source) =>
                    {
                        source.spatialBlend = 0f;
                        source.spatialize = false;
                    });
                    break;
                case SpatialPositioningType.ThreeD:
                    forEachSource((source) =>
                    {
                        source.spatialBlend = 1f;
                        source.spatialize = false;
                    });
                    break;
                case SpatialPositioningType.SpatialSound:
                    forEachSource((source) =>
                    {
                        source.spatialBlend = 1f;
                        source.spatialize = true;
                    });
                    break;
                default:
                    Debug.LogErrorFormat("Unexpected spatialization type: {0}", audioEvent.spatialization.ToString());
                    break;
            }

            if (audioEvent.spatialization == SpatialPositioningType.SpatialSound)
            {
                CreateFlatSpatialRolloffCurve();
                forEachSource((source) =>
                {
                    source.rolloffMode = AudioRolloffMode.Custom;
                    source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, SpatialRolloff);
                    SpatialSoundSettings.SetRoomSize(source, audioEvent.roomSize);
                    SpatialSoundSettings.SetMinGain(source, audioEvent.minGain);
                    SpatialSoundSettings.SetMaxGain(source, audioEvent.maxGain);
                    SpatialSoundSettings.SetUnityGainDistance(source, audioEvent.unityGainDistance);
                });
            }
            else
            {
                forEachSource((source) =>
                {
                    source.rolloffMode = AudioRolloffMode.Logarithmic;
                });
            }

            if (audioEvent.bus != null)
            {
                forEachSource((source) => source.outputAudioMixerGroup = audioEvent.bus);
            }

            float pitch = 1f;

            if (audioEvent.pitchRandomization != 0)
            {
                pitch = UnityEngine.Random.Range(audioEvent.pitchCenter - audioEvent.pitchRandomization, audioEvent.pitchCenter + audioEvent.pitchRandomization);
            }
            else
            {
                pitch = audioEvent.pitchCenter;
            }
            forEachSource((source) => source.pitch = pitch);

            float vol = 1f;
            if (audioEvent.fadeInTime > 0)
            {
                forEachSource((s