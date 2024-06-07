// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33628,y:32767,varname:node_4013,prsc:2|emission-4987-OUT,clip-6780-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32597,y:32610,ptovrint:False,ptlb:Cneise,ptin:_Cneise,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:5821,x:32432,y:33132,ptovrint:False,ptlb:node_5821,ptin:_node_5821,varname:node_5821,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4d58956b0f40fac43bdffa8925a9771d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Step,id:5259,x:32723,y:33212,varname:node_5259,prsc:2|A-5821-R,B-721-A;n:type:ShaderForge.SFN_Multiply,id:1929,x:32917,y:32878,varname:node_1929,prsc:2|A-6787-R,B-5259-OUT;n:type:ShaderForge.SFN_Multiply,id:414,x:33169,y:32750,varname:node_414,prsc:2|A-1304-RGB,B-1929-OUT,C-4520-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4520,x:32722,y:32546,ptovrint:False,ptlb:neiqiang,ptin:_neiqiang,varname:node_4520,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:721,x:32222,y:33306,varname:node_721,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:6787,x:32653,y:32862,ptovrint:False,ptlb:node_6787,ptin:_node_6787,varname:node_6787,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:ea35335147c612b4ca4c45fe1e44e229,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6780,x:33010,y:33132,varname:node_6780,prsc:2|A-6787-R,B-8820-OUT;n:type:ShaderForge.SFN_Step,id:8820,x:32731,y:33522,varname:node_8820,prsc:2|A-5821-R,B-3032-OUT;n:type:ShaderForge.SFN_Add,id:3032,x:32487,y:33549,varname:node_3032,prsc:2|A-721-A,B-1848-OUT;n:type:ShaderForge.SFN_Vector1,id:1848,x:32203,y:33629,varname:node_1848,prsc:2,v1:0.05;n:type:ShaderForge.SFN_Subtract,id:9645,x:32989,y:33480,varname:node_9645,prsc:2|A-8820-OUT,B-5259-OUT;n:type:ShaderForge.SFN_Multiply,id:5789,x:33283,y:33490,varname:node_5789,prsc:2|A-9645-OUT,B-4217-OUT,C-8485-RGB;n:type:ShaderForge.SFN_ValueProperty,id:4217,x:32970,y:33614,ptovrint:False,ptlb:bianqiang,ptin:_bianqiang,varname:node_4217,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Color,id:8485,x:32943,y:33744,ptovrint:False,ptlb:bianse,ptin:_bianse,varname:node_8485,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.6174443,c3:0.3235294,c4:1;n:type:ShaderForge.SFN_Add,id:4987,x:33420,y:32931,varname:node_4987,prsc:2|A-414-OUT,B-3335-OUT;n:type:ShaderForge.SFN_Multiply,id:3335,x:33420,y:33144,varname:node_3335,prsc:2|A-6787-R,B-5789-OUT;proporder:1304-5821-4520-6787-4217-8485;pass:END;sub:END;*/

Shader "Shader Forge/rongjiexian 1" {
    Properties {
        _Cneise ("Cneise", Color) = (1,1,1,1)
        _node_5821 ("node_5821", 2D) = "white" {}
        _neiqiang ("neiqiang", Float ) = 1
        _node_6787 ("node_6787", 2D) = "white" {}
        _bianqiang ("bianqiang", Float ) = 1
        _bianse ("bianse", Color) = (1,0.6174443,0.3235294,1)
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
            uniform float4 _Cneise;
            uniform sampler2D _node_5821; uniform float4 _node_5821_ST;
            uniform float _neiqiang;
            uniform sampler2D _node_6787; uniform float4 _node_6787_ST;
            uniform float _bianqiang;
            uniform float4 _bianse;
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
                float4 _node_6787_var = tex2D(_node_6787,TRANSFORM_TEX(i.uv0, _node_6787));
                float4 _node_5821_var = tex2D(_node_5821,TRANSFORM_TEX(i.uv0, _node_5821));
                float node_8820 = step(_node_5821_var.r,(i.vertexColor.a+0.05));
                clip((_node_6787_var.r*node_8820) - 0.5);
////// Lighting:
////// Emissive:
                float node_5259 = step(_node_5821_var.r,i.vertexColor.a);
                float3 emissive = ((_Cneise.rgb*(_node_6787_var.r*node_5259)*_neiqiang)+(_node_6787_var.r*((node_8820-node_5259)*_bianqiang*_bianse.rgb)));
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
            uniform sampler2D _node_5821; uniform float4 _node_5821_ST;
            uniform sampler2D _node_6787; uniform float4 _node_6787_ST;
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
                float4 _node_6787_var = tex2D(_node_6787,TRANSFORM_TEX(i.uv0, _node_6787));
                float4 _node_5821_var = tex2D(_node_5821,TRANSFORM_TEX(i.uv0, _node_5821));
                float node_8820 = step(_node_5821_var.r,(i.vertexColor.a+0.05));
                clip((_node_6787_var.r*node_8820) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
