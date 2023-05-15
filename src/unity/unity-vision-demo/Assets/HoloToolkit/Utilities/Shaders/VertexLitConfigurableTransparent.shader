// Very fast vertex lit shader that uses the Unity lighting model.
// Compiles down to only performing the operations you're actually using.
// Uses material property drawers rather than a custom editor for ease of maintenance.

Shader "HoloToolkit/Vertex Lit Configurable Transparent"
{
    Properties
    {
        [Header(Main Color)]
        [Toggle] _UseColor("Enabled?", Float) = 0
        _Color("Main Color", Color) = (1,1,1,1)
        [Space(20)]

        [Header(Base(RGB))]
        [Toggle] _UseMainTex("Enabled?", Float) = 1
        _MainTex("Base (RGB)", 2D) = "white" {}
        [Space(20)]

        // Uses UV scale, etc from main texture
        [Header(Emission(RGB))]
        [Toggle] _UseEmissionTex("Enabled?", Float) = 0
        [NoScaleOffset] _EmissionTex("Emission (RGB)", 2D) = "white" {}
        [Space(20)]

        [Header(Blend State)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("