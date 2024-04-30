Shader "Custom/OutlineURP"
{
    Properties 
    {
        [MaterialToggle(_TEX_ON)] _DetailTex ("Enable Detail texture", Float) = 0  //1
        _MainTex ("Detail", 2D) = "white" {}                                     //2
        _ToonShade ("Shade", 2D) = "white" {}                                    //3
        [MaterialToggle(_COLOR_ON)] _TintColor ("Enable Color Tint", Float) = 0   //4
        _Color ("Base Color", Color) = (1,1,1,1)                                  //5    
        [MaterialToggle(_VCOLOR_ON)] _VertexColor ("Enable Vertex Color", Float) = 0 //6        
        _Brightness ("Brightness 1 = neutral", Float) = 1.0                       //7    
        _OutlineColor ("Outline Color", Color) = (0.5,0.5,0.5,1.0)                //10
        _OutlineWidth ("Outline width", Range(0.001, 0.1)) = 0.01                 //11
    }
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 250 
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Front
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            struct appdata_t 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            v2f vert (appdata_t v) 
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex + v.normal * _OutlineWidth);
                o.color = _OutlineColor;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDHLSL
        }
    }
    Fallback "Universal Render Pipeline/Lit"
}
