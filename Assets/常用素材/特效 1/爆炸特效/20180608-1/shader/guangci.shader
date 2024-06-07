// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33857,y:32731,varname:node_4013,prsc:2|emission-8011-OUT,clip-8938-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:33177,y:32629,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:2029,x:32547,y:32942,ptovrint:False,ptlb:node_2029,ptin:_node_2029,varname:node_2029,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:269d5107401861d4a83e7809e593b9c8,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3899,x:32839,y:33115,varname:node_3899,prsc:2|A-231-R,B-4218-R;n:type:ShaderForge.SFN_Tex2d,id:231,x:32563,y:33285,ptovrint:False,ptlb:node_231,ptin:_node_231,varname:node_231,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c360ead16a7ac8145957892dca606172,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Step,id:7915,x:33078,y:33182,varname:node_7915,prsc:2|A-3899-OUT,B-1505-A;n:type:ShaderForge.SFN_Tex2d,id:4218,x:32527,y:33123,ptovrint:False,ptlb:node_4218,ptin:_node_4218,varname:node_4218,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:03d9a3a2fb5011841876b98b8dfbc0b2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:8938,x:33159,y:32946,varname:node_8938,prsc:2|A-2029-R,B-7915-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2189,x:33159,y:32808,ptovrint:False,ptlb:node_2189,ptin:_node_2189,varname:node_2189,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:1505,x:32919,y:33395,varname:node_1505,prsc:2;n:type:ShaderForge.SFN_Slider,id:5486,x:33163,y:33426,ptovrint:False,ptlb:node_5486,ptin:_node_5486,varname:node_5486,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1367521,max:1;n:type:ShaderForge.SFN_Multiply,id:8011,x:33550,y:32833,varname:node_8011,prsc:2|A-1304-RGB,B-2189-OUT,C-8938-OUT;proporder:1304-2029-231-4218-2189-5486;pass:END;sub:END;*/

Shader "Shader Forge/guangci" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _node_2029 ("node_2029", 2D) = "white" {}
        _node_231 ("node_231", 2D) = "white" {}
        _node_4218 ("node_4218", 2D) = "white" {}
        _node_2189 ("node_2189", Float ) = 1
        _node_5486 ("node_5486", Range(0, 1)) = 0.1367521
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha Zero
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _Color;
            uniform sampler2D _node_2029; uniform float4 _node_2029_ST;
            uniform sampler2D _node_231; uniform float4 _node_231_ST;
            uniform sampler2D _node_4218; uniform float4 _node_4218_ST;
            uniform float _node_2189;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _node_2029_var = tex2D(_node_2029,TRANSFORM_TEX(i.uv0, _node_2029));
                float4 _node_231_var = tex2D(_node_231,TRANSFORM_TEX(i.uv0, _node_231));
                float4 _node_4218_var = tex2D(_node_4218,TRANSFORM_TEX(i.uv0, _node_4218));
                float node_8938 = (_node_2029_var.r*step((_node_231_var.r*_node_4218_var.r),i.vertexColor.a));
                clip(node_8938 - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (_Color.rgb*_node_2189*node_8938);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _node_2029; uniform float4 _node_2029_ST;
            uniform sampler2D _node_231; uniform float4 _node_231_ST;
            uniform sampler2D _node_4218; uniform float4 _node_4218_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _node_2029_var = tex2D(_node_2029,TRANSFORM_TEX(i.uv0, _node_2029));
                float4 _node_231_var = tex2D(_node_231,TRANSFORM_TEX(i.uv0, _node_231));
                float4 _node_4218_var = tex2D(_node_4218,TRANSFORM_TEX(i.uv0, _node_4218));
                float node_8938 = (_node_2029_var.r*step((_node_231_var.r*_node_4218_var.r),i.vertexColor.a));
                clip(node_8938 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
