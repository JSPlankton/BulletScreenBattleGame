// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:35506,y:32977,varname:node_4013,prsc:2|emission-1860-OUT,clip-4233-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32869,y:32592,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.3442191,c3:0.1029412,c4:1;n:type:ShaderForge.SFN_Tex2d,id:4714,x:32783,y:33006,ptovrint:False,ptlb:node_4714,ptin:_node_4714,varname:node_4714,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9958e69cda4116c46adefef462aaa4e7,ntxv:0,isnm:False|UVIN-339-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:9048,x:32783,y:32819,ptovrint:False,ptlb:node_9048,ptin:_node_9048,varname:node_9048,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1f276df0e92ee5a4a95bbef8a4e38814,ntxv:0,isnm:False|UVIN-4265-UVOUT;n:type:ShaderForge.SFN_Panner,id:4265,x:32608,y:32819,varname:node_4265,prsc:2,spu:0.2,spv:-0.3|UVIN-2944-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:2944,x:32436,y:32819,varname:node_2944,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:339,x:32608,y:33006,varname:node_339,prsc:2,spu:-0.1,spv:-0.2|UVIN-6616-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:6616,x:32436,y:33006,varname:node_6616,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:1195,x:33040,y:32641,varname:node_1195,prsc:2|A-1933-R,B-1304-RGB;n:type:ShaderForge.SFN_Add,id:1826,x:33208,y:32756,varname:node_1826,prsc:2|A-1195-OUT,B-9633-RGB;n:type:ShaderForge.SFN_Tex2d,id:1933,x:32714,y:32615,ptovrint:False,ptlb:node_1933,ptin:_node_1933,varname:node_1933,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fcf6d78a8772ac241a9b860a3a0ed460,ntxv:0,isnm:False|UVIN-4265-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:7199,x:33431,y:33423,ptovrint:False,ptlb:node_7199,ptin:_node_7199,varname:node_7199,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1029af4307d16db4f9c5c14af2432cb0,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:3334,x:33436,y:33806,ptovrint:False,ptlb:node_3334,ptin:_node_3334,varname:node_3334,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.3103448,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:9633,x:32783,y:33151,ptovrint:False,ptlb:node_9633,ptin:_node_9633,varname:node_9633,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3363e2f70fa307242b0374fcd0154570,ntxv:0,isnm:False|UVIN-339-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:4090,x:33089,y:33345,ptovrint:False,ptlb:node_7199_copy,ptin:_node_7199_copy,varname:_node_7199_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1029af4307d16db4f9c5c14af2432cb0,ntxv:0,isnm:False;n:type:ShaderForge.SFN_OneMinus,id:22,x:33263,y:33362,varname:node_22,prsc:2|IN-4090-R;n:type:ShaderForge.SFN_Multiply,id:7246,x:33353,y:32911,varname:node_7246,prsc:2|A-1826-OUT,B-22-OUT;n:type:ShaderForge.SFN_Multiply,id:7562,x:33700,y:33489,varname:node_7562,prsc:2|A-7199-R,B-6432-RGB;n:type:ShaderForge.SFN_Tex2d,id:8760,x:33700,y:33618,ptovrint:False,ptlb:node_8760,ptin:_node_8760,varname:node_8760,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6329ab342ee7ba340b82e8c255e384fc,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:1860,x:33815,y:33060,varname:node_1860,prsc:2|A-7246-OUT,B-7562-OUT,C-5082-OUT,D-40-OUT,E-5273-OUT;n:type:ShaderForge.SFN_Tex2d,id:7088,x:33685,y:33838,ptovrint:False,ptlb:node_7088,ptin:_node_7088,varname:node_7088,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b527f5af37389184889b717949b5ade4,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5082,x:33955,y:33771,varname:node_5082,prsc:2|A-3334-RGB,B-7088-R;n:type:ShaderForge.SFN_Color,id:6432,x:33065,y:33546,ptovrint:False,ptlb:node_6432,ptin:_node_6432,varname:node_6432,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.4758621,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3017,x:33689,y:34170,ptovrint:False,ptlb:node_3017,ptin:_node_3017,varname:node_3017,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d76bd851de28c164c8af40f31dd9ff29,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:3955,x:33295,y:34131,ptovrint:False,ptlb:node_3955,ptin:_node_3955,varname:node_3955,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5172414,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:40,x:33977,y:34121,varname:node_40,prsc:2|A-3955-RGB,B-3017-R;n:type:ShaderForge.SFN_Tex2d,id:828,x:34548,y:33231,ptovrint:False,ptlb:node_828,ptin:_node_828,varname:node_828,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:067f06881014f154aa818bdd9590cb75,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8483,x:34570,y:33465,ptovrint:False,ptlb:node_8483,ptin:_node_8483,varname:node_8483,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:85d84475629794e49895dbdfe9fcd611,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9998,x:34802,y:33471,varname:node_9998,prsc:2|A-828-R,B-8483-R;n:type:ShaderForge.SFN_Slider,id:4972,x:34547,y:33719,ptovrint:False,ptlb:node_4972,ptin:_node_4972,varname:node_4972,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Step,id:6421,x:35089,y:33498,varname:node_6421,prsc:2|A-4972-OUT,B-9998-OUT;n:type:ShaderForge.SFN_Add,id:8197,x:34968,y:33846,varname:node_8197,prsc:2|A-4972-OUT,B-4849-OUT;n:type:ShaderForge.SFN_Vector1,id:4849,x:34737,y:33916,varname:node_4849,prsc:2,v1:-0.03;n:type:ShaderForge.SFN_Step,id:4233,x:35188,y:33824,varname:node_4233,prsc:2|A-8197-OUT,B-9998-OUT;n:type:ShaderForge.SFN_Subtract,id:1913,x:35423,y:33653,varname:node_1913,prsc:2|A-4233-OUT,B-6421-OUT;n:type:ShaderForge.SFN_Multiply,id:5273,x:35670,y:33817,varname:node_5273,prsc:2|A-1913-OUT,B-5139-RGB,C-3137-OUT;n:type:ShaderForge.SFN_Color,id:5139,x:35445,y:33861,ptovrint:False,ptlb:node_5139,ptin:_node_5139,varname:node_5139,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.7737322,c3:0.2867647,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:3137,x:35445,y:34016,ptovrint:False,ptlb:node_3137,ptin:_node_3137,varname:node_3137,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;proporder:1304-9048-4714-1933-9633-4090-7199-3334-7088-6432-3017-3955-828-8483-4972-5139-3137;pass:END;sub:END;*/

Shader "Shader Forge/longpi" {
    Properties {
        _Color ("Color", Color) = (1,0.3442191,0.1029412,1)
        _node_9048 ("node_9048", 2D) = "white" {}
        _node_4714 ("node_4714", 2D) = "white" {}
        _node_1933 ("node_1933", 2D) = "white" {}
        _node_9633 ("node_9633", 2D) = "white" {}
        _node_7199_copy ("node_7199_copy", 2D) = "white" {}
        _node_7199 ("node_7199", 2D) = "white" {}
        _node_3334 ("node_3334", Color) = (1,0.3103448,0,1)
        _node_7088 ("node_7088", 2D) = "white" {}
        _node_6432 ("node_6432", Color) = (1,0.4758621,0,1)
        _node_3017 ("node_3017", 2D) = "white" {}
        _node_3955 ("node_3955", Color) = (1,0.5172414,0,1)
        _node_828 ("node_828", 2D) = "white" {}
        _node_8483 ("node_8483", 2D) = "white" {}
        _node_4972 ("node_4972", Range(0, 1)) = 0
        _node_5139 ("node_5139", Color) = (1,0.7737322,0.2867647,1)
        _node_3137 ("node_3137", Float ) = 1
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
            uniform sampler2D _node_1933; uniform float4 _node_1933_ST;
            uniform sampler2D _node_7199; uniform float4 _node_7199_ST;
            uniform float4 _node_3334;
            uniform sampler2D _node_9633; uniform float4 _node_9633_ST;
            uniform sampler2D _node_7199_copy; uniform float4 _node_7199_copy_ST;
            uniform sampler2D _node_7088; uniform float4 _node_7088_ST;
            uniform float4 _node_6432;
            uniform sampler2D _node_3017; uniform float4 _node_3017_ST;
            uniform float4 _node_3955;
            uniform sampler2D _node_828; uniform float4 _node_828_ST;
            uniform sampler2D _node_8483; uniform float4 _node_8483_ST;
            uniform float _node_4972;
            uniform float4 _node_5139;
            uniform float _node_3137;
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
            float4 frag(VertexOutput i) : COLOR {
                float4 _node_828_var = tex2D(_node_828,TRANSFORM_TEX(i.uv0, _node_828));
                float4 _node_8483_var = tex2D(_node_8483,TRANSFORM_TEX(i.uv0, _node_8483));
                float node_9998 = (_node_828_var.r*_node_8483_var.r);
                float node_4233 = step((_node_4972+(-0.03)),node_9998);
                clip(node_4233 - 0.5);
////// Lighting:
////// Emissive:
                float4 node_4158 = _Time + _TimeEditor;
                float2 node_4265 = (i.uv0+node_4158.g*float2(0.2,-0.3));
                float4 _node_1933_var = tex2D(_node_1933,TRANSFORM_TEX(node_4265, _node_1933));
                float2 node_339 = (i.uv0+node_4158.g*float2(-0.1,-0.2));
                float4 _node_9633_var = tex2D(_node_9633,TRANSFORM_TEX(node_339, _node_9633));
                float4 _node_7199_copy_var = tex2D(_node_7199_copy,TRANSFORM_TEX(i.uv0, _node_7199_copy));
                float4 _node_7199_var = tex2D(_node_7199,TRANSFORM_TEX(i.uv0, _node_7199));
                float4 _node_7088_var = tex2D(_node_7088,TRANSFORM_TEX(i.uv0, _node_7088));
                float4 _node_3017_var = tex2D(_node_3017,TRANSFORM_TEX(i.uv0, _node_3017));
                float3 emissive = ((((_node_1933_var.r*_Color.rgb)+_node_9633_var.rgb)*(1.0 - _node_7199_copy_var.r))+(_node_7199_var.r*_node_6432.rgb)+(_node_3334.rgb*_node_7088_var.r)+(_node_3955.rgb*_node_3017_var.r)+((node_4233-step(_node_4972,node_9998))*_node_5139.rgb*_node_3137));
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
            uniform sampler2D _node_828; uniform float4 _node_828_ST;
            uniform sampler2D _node_8483; uniform float4 _node_8483_ST;
            uniform float _node_4972;
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
            float4 frag(VertexOutput i) : COLOR {
                float4 _node_828_var = tex2D(_node_828,TRANSFORM_TEX(i.uv0, _node_828));
                float4 _node_8483_var = tex2D(_node_8483,TRANSFORM_TEX(i.uv0, _node_8483));
                float node_9998 = (_node_828_var.r*_node_8483_var.r);
                float node_4233 = step((_node_4972+(-0.03)),node_9998);
                clip(node_4233 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
