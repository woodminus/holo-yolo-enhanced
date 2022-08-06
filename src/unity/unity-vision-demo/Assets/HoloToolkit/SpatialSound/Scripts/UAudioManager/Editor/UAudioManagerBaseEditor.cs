// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HoloToolkit.Unity
{
    public class UAudioManagerBaseEditor<TEvent> : Editor where TEvent : AudioEvent, new()
    {
        protected UAudioManagerBase<TEvent> myTarget;
        private string[] eventNames;
        private int selectedEventIndex = 0;
        private readonly string[] posTypes = { "2D", "3D", "Spatial Sound" };

        protected void SetUpEditor()
        {
            // Having a null array of events causes too many errors and should only happen on first adding anyway.
            if (this.myTarget.EditorEvents == null)
            {
                this.myTarget.EditorEvents = new TEvent[0];
            }
            this.eventNames = new string[this.myTarget.EditorEvents.Length];
            UpdateEventNames(this.myTarget.EditorEvents);
        }

        protected void DrawInspectorGUI(bool showEmitters)
        {
            this.serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            DrawEventHeader(this.myTarget.EditorEvents);

            if (this.myTarget.EditorEvents != null && this.myTarget.EditorEvents.Length > 0)
            {
                // Display current event in dropdown.
                EditorGUI.indentLevel++;
                this.selectedEventIndex = EditorGUILayout.Popup(this.selectedEventIndex, this.eventNames);

                if (this.selectedEventIndex < this.myTarget.EditorEvents.Length)
                {
                    TEvent selectedEvent;

                    selectedEvent = this.myTarget.EditorEvents[this.selectedEventIndex];
                    SerializedProperty selectedEventProperty = this.serializedObject.FindProperty("events.Array.data[" + this.selectedEventIndex.ToString() + "]");
                    EditorGUILayout.Space();

                    if (selectedEventProperty != null)
                    {
                        DrawEventInspector(selectedEventProperty, selectedEvent, this.myTarget.EditorEvents, showEmitters);
                        if (!DrawContainerInspector(selectedEventProperty, selectedEvent))
                        {
                            EditorGUI.indentLevel++;
                            DrawSoundClipInspector(selectedEventProperty, selectedEvent);
                            EditorGUI.indentLevel--;
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }

            EditorGUI.EndChangeCheck();
            this.serializedObject.ApplyModifiedProperties();

            if (UnityEngine.GUI.changed)
            {
                EditorUtility.SetDirty(this.myTarget);
            }
        }

        private void DrawEventHeader(TEvent[] EditorEvents)
        {
            // Add or remove current event.
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayoutExtensions.Label("Events");

            using (new EditorGUI.DisabledScope((EditorEvents != null) && (EditorEvents.Length < 1)))
            {
                if (EditorGUILayoutExtensions.Button("Remove"))
                {
                    this.myTarget.EditorEvents = RemoveAudioEvent(EditorEvents, this.selectedEventIndex);
                }
            }

            if (EditorGUILayoutExtensions.Button("Add"))
            {
                this.myTarget.EditorEvents = AddAudioEvent(EditorEvents);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        private void DrawEventInspector(SerializedProperty selectedEventProperty, TEvent selectedEvent, TEvent[] EditorEvents, bool showEmitters)
        {
            // Get current event's properties.
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("name"));

            if (selectedEvent.name != this.eventNames[this.selectedEventIndex])
            {
                UpdateEventNames(EditorEvents);
            }

            if (showEmitters)
            {
                EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("primarySource"));
                if (selectedEvent.IsContinuous())
                {
                    EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("secondarySource"));
                }
            }

            // Positioning
            selectedEvent.spatialization = (SpatialPositioningType)EditorGUILayout.Popup("Positioning", (int)selectedEvent.spatialization, this.posTypes);
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("roomSize"));
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("minGain"));
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("maxGain"));
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("unityGainDistance"));

            // Bus
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("bus"));

            // Fades
            if (!selectedEvent.IsContinuous())
            {
                EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("fadeInTime"));
            }

            // Pitch Settings
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("pitchCenter"));

            // Volume settings
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("volumeCenter"));

            // Pan Settings
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("panCenter"));

            // Instancing
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("instanceLimit"));
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("instanceTimeBuffer"));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("instanceBehavior"));

            // Container
            EditorGUILayout.Space();
        }

        private bool DrawContainerInspector(SerializedProperty selectedEventProperty, TEvent selectedEvent)
        {
            bool addedSound = false;
            EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("container.containerType"));

            if (!selectedEvent.IsContinuous())
            {
                EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("container.looping"));

                if (selectedEvent.container.looping)
                {
                    EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("container.loopTime"));
                }
            }

            // Sounds
            EditorGUILayout.Space();

            if (selectedEvent.IsContinuous())
            {
                EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("container.crossfadeTime"));
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Sounds");

            if (EditorGUILayoutExtensions.Button("Add"))
            {
                AddSound(selectedEvent);

                // Skip drawing sound inspector after adding a new sound.
                addedSound = true;
            }
            EditorGUILayout.EndHorizontal();
            return addedSound;
        }

        private void DrawSoundClipInspector(SerializedProperty selectedEventProperty, TEvent selectedEvent)
        {
            bool allowLoopingClip = !selectedEvent.container.looping;

            if (allowLoopingClip)
            {
                if (selectedEvent.IsContinuous())
                {
                    allowLoopingClip = false;
                }
            }

            for (int i = 0; i < selectedEvent.container.sounds.Length; i++)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(selectedEventProperty.FindPropertyRelative("container.sounds.Array.data[" + i + "].sound"));

                if (EditorGUILayoutExtensions.Button("Remove"))
                {
                    selectedEventProperty.FindPropertyRelative("container.sounds.Array.data[" + i + "]").DeleteCommand();
                    break;
                }

                EditorGUILayout.EndHorizontal();

                if (!selectedEvent.IsContinuous())
                {
                    EditorGUILayout.BeginHorizontal();
                    Edit