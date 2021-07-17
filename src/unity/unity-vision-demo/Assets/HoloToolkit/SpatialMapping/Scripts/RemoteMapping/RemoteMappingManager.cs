// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace HoloToolkit.Unity
{
    [RequireComponent(typeof(RemoteMeshTarget))]
    public partial class RemoteMappingManager : Singleton<RemoteMappingManager>
    {
        [Tooltip("Key to press in editor to enable spatial mapping over the network.")]
        public KeyCode RemoteMappingKey = KeyCode.N;

        [Tooltip("Keyword for sending meshes from HoloLens to Unity over the network.")]
        public string SendMeshesKeyword = "send meshes";

        /// <summary>
        /// Receives meshes collected over the network.
        /// </summary>
        private RemoteMeshTarget remoteMeshTarget;

        /// <summary>
        /// Used for voice commands.
        /// </summary>
        private KeywordRecognizer keywordRecognizer;

        /// <summary>
        /// Collection of supported keywords and their associated actions.
        /// </summary>
        private Dictionary<string, System.Action> keywordCollection;

        // Use this for initialization.
        private void Start()
        {
            // Create our keyword collection.
            keywordCollection = new Dictionary<string, System.Action>();
            keywordCollection.Add(SendMeshesKeyword, () => SendMeshes());

            // Tell the KeywordRecognizer about our keywords.
   