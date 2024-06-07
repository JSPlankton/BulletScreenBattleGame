// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33629,y:32684,varname:node_4013,prsc:2|emission-9147-OUT,clip-6162-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32707,y:32578,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2339099,c2:0.1012111,c3:0.7647059,c4:1;n:type:ShaderForge.SFN_Panner,id:3430,x:32250,y:32907,varname:node_3430,prsc:2,spu:0.2,spv:-0.8|UVIN-3890-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3890,x:31796,y:32954,varname:node_3890,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:9677,x:32250,y:33152,varname:node_9677,prsc:2,spu:-0.35,spv:-1|UVIN-3890-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:838,x:32481,y:32903,ptovrint:False,ptlb:node_838,ptin:_node_838,varname:node_838,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c48b1bd0aee7d4b4ca0b69bfdcf604fe,ntxv:0,isnm:False|UVIN-3430-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:727,x:32480,y:33165,ptovrint:False,ptlb:node_727,ptin:_node_727,varname:node_727,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cc0423763bc0470429e25421eea78f02,ntxv:0,isnm:False|UVIN-9677-UVOUT;n:type:ShaderForge.SFN_Multiply,id:9147,x:33029,y:32741,varname:node_9147,prsc:2|A-1304-RGB,B-9471-OUT;n:type:ShaderForge.SFN_Add,id:8635,x:32777,y:33008,varname:node_8635,prsc:2|A-838-R,B-727-R;n:type:ShaderForge.SFN_Tex2d,id:5392,x:33077,y:33340,ptovrint:False,ptlb:node_5392,ptin:_node_5392,varname:node_5392,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1b858a7e07dff5c40b0f72fd9868cbe9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6162,x:33368,y:33213,varname:node_6162,prsc:2|A-9471-OUT,B-5392-R;n:type:ShaderForge.SFN_Power,id:9471,x:33013,y:33008,varname:node_9471,prsc:2|VAL-8635-OUT,EXP-2008-OUT;n:type:ShaderForge.SFN_Slider,id:2008,x:32648,y:33182,ptovrint:False,ptlb:node_2008,ptin:_node_2008,varname:node_2008,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:3.858883,max:5;proporder:1304-838-727-5392-2008;pass:END;sub:END;*/

Shader "Shader Forge/wenli" {
    Properties {
        _Color ("Color", Color) = (0.2339099,0.1012111,0.7647059,1)
        _node_838 ("node_838", 2D) = "white" {}
        _node_727 ("node_727", 2D) = "white" {}
        _node_5392 ("node_5392", 2D) = "white" {}
        _node_2008 ("node_2008", Range(0, 5)) = 3.858883
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
            Blend SrcAlpha OneMinusSrcAlpha
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
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _node_838; uniform float4 _node_838_ST;
            uniform sampler2D _node_727; uniform float4 _node_727_ST;
            uniform sampler2D _node_5392; uniform float4 _node_5392_ST;
            uniform float _node_2008;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 node_6663 = _Time + _TimeEditor;
                float2 node_3430 = (i.uv0+node_6663.g*float2(0.2,-0.8));
                float4 _node_838_var = tex2D(_node_838,TRANSFORM_TEX(node_3430, _node_838));
                float2 node_9677 = (i.uv0+node_6663.g*float2(-0.35,-1));
                float4 _node_727_var = tex2D(_node_727,TRANSFORM_TEX(node_9677, _node_727));
                float node_9471 = pow((_node_838_var.r+_node_727_var.r),_node_2008);
                float4 _node_5392_var = tex2D(_node_5392,TRANSFORM_TEX(i.uv0, _node_5392));
                clip((node_9471*_node_5392_var.r) - 0.5);
////// Lighting:
////// Emissive:
                float3 emissive = (_Color.rgb*node_9471);
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
            uniform float4 _TimeEditor;
            uniform sampler2D _node_838; uniform float4 _node_838_ST;
            uniform sampler2D _node_727; uniform float4 _node_727_ST;
            uniform sampler2D _node_5392; uniform float4 _node_5392_ST;
            uniform float _node_2008;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 node_3076 = _Time + _TimeEditor;
                float2 node_3430 = (i.uv0+node_3076.g*float2(0.2,-0.8));
                float4 _node_838_var = tex2D(_node_838,TRANSFORM_TEX(node_3430, _node_838));
                float2 node_9677 = (i.uv0+node_3076.g*float2(-0.35,-1));
                float4 _node_727_var = tex2D(_node_727,TRANSFORM_TEX(node_9677, _node_727));
                float node_9471 = pow((_node_838_var.r+_node_727_var.r),_node_2008);
                float4 _node_5392_var = tex2D(_node_5392,TRANSFORM_TEX(i.uv0, _node_5392));
                clip((node_9471*_node_5392_var.r) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
