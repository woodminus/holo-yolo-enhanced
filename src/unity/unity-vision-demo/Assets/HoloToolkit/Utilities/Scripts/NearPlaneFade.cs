// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

[ExecuteInEditMode]
public class NearPlaneFade : MonoBehaviour
{
    public float FadeDistanceStart = 0.85f;
    public float FadeDistanceEnd = 0.5f;

    public bool NearPlaneFadeOn = true;

    private const string FadeKeywordOn = "_NEAR_PLANE_FADE_ON";

    private int fadeDistancePropertyID;

    void Start()
    {
        fadeDistancePropertyID = Shader.PropertyToID("_NearPlaneFadeDistance");
        UpdateShaderParams();
    }

    private void OnValidate()
    {
        FadeDistanceStart = Mathf.Max(FadeDistanceStart, 0);
        FadeDistanceEnd = Mathf.Max(FadeDistanceEnd, 0);
        FadeDistanceStart = Mathf.Max(FadeDistanceStart, FadeDistanceEnd);

        UpdateShaderParams();
    }

    private void UpdateShaderParams()
    {
        