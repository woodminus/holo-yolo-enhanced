#if _USEMAINTEX_ON
    UNITY_DECLARE_TEX2D(_MainTex);
#endif

#if _USECOLOR_ON
    float4 _Color;
#endif

#if _USEBUMPMAP_ON
    UNITY_DECLARE_TEX2D(_BumpMap);
#endif

#if _USEEMISSIONTEX_ON
    UNITY_DECLARE_TEX2D(_EmissionTex);
#endif

float _Specular;
float _Gloss;

struct Input
{
    // Will get compiled out if not touched
    float2 uv_MainTex;
    
    #if _NEAR_PLANE_FADE_ON
        float fade;
    #endif
};

void vert(inout appdata_full v, out Input o)
{
    UNITY_INITIALIZE_OUT