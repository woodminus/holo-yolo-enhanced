// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using System.Collections.Generic;
using HoloToolkit.Unity;

/// <summary>
/// Attach this component to a GameObject that contains some meshes (i.e.: the FakeSpatialMappingMesh.fbx).
/// When running in the Unity editor, the planes are then visualized via editor gizmos.  You can then
/// play with the API parameters in realtime to see how the impact the plane finding algorithm.
/// </summary>
public class PlaneFindingTest : MonoBehaviour
{
    [Range(0, 45)]
    public float SnapToGravityThreshold = 0.0f;

    [Range(0,10)]
    public float MinArea = 1.0f;

    public bool VisualizeSubPlanes = false;

    private List<PlaneFinding.MeshData> meshData = new List<PlaneFinding.MeshData>();
    private BoundedPlane[] planes;

    private void Update()
    {
        // Grab the necessary mesh data from the current set of surfaces that we want to run
        // PlaneFinding against.  This must be done on the main UI thread.
        meshData.Clear();
        foreach (MeshFilter mesh in GetComponentsInChildren<MeshFilter>())
        {
            meshData.Add(new PlaneFinding.MeshData(mesh));
        }

        // Now call FindPlanes().  NOTE: In a real application, this MUST be executed on a
        // background thread (i.e.: via ThreadPool.QueueUserWorkItem) so that it doesn't stall the
        // rendering thread while running plane finding.  Maintaining a solid 60fps is crucial
        // to a good user experience.
        planes = (VisualizeSubPlanes) ?
            PlaneFinding.FindSubPlanes(meshData, SnapToGravityThreshold) :
            PlaneFinding.FindPlanes(meshData, SnapToGravityThreshold, MinArea);
    }

    private static Color[] colors = new Color[] { Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.white, Color.yellow };
    private void OnDrawGizmos()
    {
        if (planes != null)
        {
            for (int i = 0; i < planes.Length; ++i)
            {
                Vector3 center = planes[i].Bounds.Center;
                Quaternion rotation = planes[i].Bounds.Rotation;
                Vector3 extents = planes[i].Bounds.Extents;
                Vector3 nor