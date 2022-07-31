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
           