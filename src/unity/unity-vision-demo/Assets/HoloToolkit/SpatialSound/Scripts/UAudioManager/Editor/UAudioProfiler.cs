// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class UAudioProfiler : EditorWindow
    {
        private int currentFrame = 0;
        private List<ProfilerEvent[]> eventTimeline;
        private Vector2 scrollOffset = new Vector2();
        private const int MaxFrames = 300;

        private class ProfilerEvent
        {
            public string EventName = "";
            public string EmitterName = "";
            public string BusName = "";
        }

        [MenuItem("Addons/UAudioTools/Profiler")]
        static void ShowEditor()
        {
            UAudioProfiler profilerWindow = GetWindow<UAudioProfiler>();
            if (profilerWindow.eventTimeline == null)
            {
                profilerWindow.currentFrame = 0;
        