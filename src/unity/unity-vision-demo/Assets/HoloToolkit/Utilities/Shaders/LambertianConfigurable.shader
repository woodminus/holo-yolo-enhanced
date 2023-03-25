// Very fast shader that uses the Unity lighting model.
// Compiles down to only performing the operations you're actually using.
// Uses material property drawers rather than a custom editor for ease of maintenance.

Shader "HoloToolkit/Lambertian Configurable"
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
        [Header(Normalmap)]
        [Toggle] _UseBumpMap("Enabled?", Float) = 0
        [NoScaleOffset] _BumpMap("Normalmap", 2D) = "bump" {}
        [Space(20)]

        // Uses UV scale, etc from main texture
        [Header(Emission(RGB))]
        [Toggle] _UseEmissionTex("Enabled?", Float) = 0
        [NoScaleOffset] _EmissionTex("Emission (RGB)", 2D) = "white" {}
        [Space(20)]

        [Header(Blend State)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("SrcBlend", Float) = 1 //"One"
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("DestBlend", Float) = 0 //"Zero"
        [Space(20)]

        [Header(Other)]
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2 //"Back"
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4 //"LessEqual"
        [Enum(Off,0,On,1)] _ZWrite("ZWrite", Float) = 1.0 //"On"
        [Enum(UnityEngine.Rendering.ColorWriteMask)] _ColorWriteMask("ColorWriteMask", Float) = 15 //"All"
    }

    SubS