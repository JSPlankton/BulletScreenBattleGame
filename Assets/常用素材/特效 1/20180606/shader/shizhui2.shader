// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33374,y:32996,varname:node_4013,prsc:2|emission-3724-OUT,clip-418-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32419,y:32702,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2553245,c2:0,c3:0.6985294,c4:1;n:type:ShaderForge.SFN_Tex2d,id:4431,x:32377,y:32868,ptovrint:False,ptlb:node_4431,ptin:_node_4431,varname:node_4431,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1fc7fd8c7c60e0043b6969a21db243af,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5788,x:32605,y:32898,varname:node_5788,prsc:2|A-4431-B,B-5810-OUT,C-1304-RGB;n:type:ShaderForge.SFN_ValueProperty,id:5810,x:32377,y:33054,ptovrint:False,ptlb:node_5810,ptin:_node_5810,varname:node_5810,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Tex2d,id:8048,x:32361,y:33161,ptovrint:False,ptlb:node_8048,ptin:_node_8048,varname:node_8048,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d2911be3689399e4d826593807821e43,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:3724,x:32758,y:33042,varname:node_3724,prsc:2|A-5788-OUT,B-1478-OUT,C-9309-OUT;n:type:ShaderForge.SFN_Multiply,id:1478,x:32574,y:33223,varname:node_1478,prsc:2|A-8048-R,B-7619-RGB;n:type:ShaderForge.SFN_Color,id:7619,x:32352,y:33344,ptovrint:False,ptlb:node_7619,ptin:_node_7619,varname:node_7619,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.6448275,c2:0.2426471,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3143,x:31846,y:33533,ptovrint:False,ptlb:node_3143,ptin:_node_3143,varname:node_3143,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4d58956b0f40fac43bdffa8925a9771d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Step,id:7706,x:32079,y:33568,varname:node_7706,prsc:2|A-3143-R,B-2803-OUT;n:type:ShaderForge.SFN_Slider,id:2803,x:31536,y:33720,ptovrint:False,ptlb:node_2803,ptin:_node_2803,varname:node_2803,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-0.1,cur:-0.1,max:1;n:type:ShaderForge.SFN_Step,id:418,x:32083,y:33942,varname:node_418,prsc:2|A-3143-R,B-3372-OUT;n:type:ShaderForge.SFN_Add,id:3372,x:31828,y:34080,varname:node_3372,prsc:2|A-2803-OUT,B-8399-OUT;n:type:ShaderForge.SFN_Vector1,id:8399,x:31615,y:34113,varname:node_8399,prsc:2,v1:0.03;n:type:ShaderForge.SFN_Subtract,id:5244,x:32401,y:33874,varname:node_5244,prsc:2|A-418-OUT,B-7706-OUT;n:type:ShaderForge.SFN_Multiply,id:9309,x:32786,y:33895,varname:node_9309,prsc:2|A-5244-OUT,B-2079-RGB;n:type:ShaderForge.SFN_Color,id:2079,x:32506,y:33994,ptovrint:False,ptlb:node_2079,ptin:_node_2079,varname:node_2079,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.6529412,c3:0.5735294,c4:1;proporder:1304-4431-5810-8048-7619-3143-2803-2079;pass:END;sub:END;*/

Shader "Shader Forge/shizhui2" {
    Properties {
        _Color ("Color", Color) = (0.2553245,0,0.6985294,1)
        _node_4431 ("node_4431", 2D) = "white" {}
        _node_5810 ("node_5810", Float ) = 2
        _node_8048 ("node_8048", 2D) = "white" {}
        _node_7619 ("node_7619", Color) = (0.6448275,0.2426471,1,1)
        _node_3143 ("node_3143", 2D) = "white" {}
        _node_2803 ("node_2803", Range(-0.1, 1)) = -0.1
        _node_2079 ("node_2079", Color) = (1,0.6529412,0.5735294,1)
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
            uniform float4 _Color;
            uniform sampler2D _node_4431; uniform float4 _node_4431_ST;
            uniform float _node_5810;
            uniform sampler2D _node_8048; uniform float4 _node_8048_ST;
            uniform float4 _node_7619;
            uniform sampler2D _node_3143; uniform float4 _node_3143_ST;
            uniform float _node_2803;
            uniform float4 _node_2079;
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
                float4 _node_3143_var = tex2D(_node_3143,TRANSFORM_TEX(i.uv0, _node_3143));
                float node_418 = step(_node_3143_var.r,(_node_2803+0.03));
                clip(node_418 - 0.5);
////// Lighting:
////// Emissive:
                float4 _node_4431_var = tex2D(_node_4431,TRANSFORM_TEX(i.uv0, _node_4431));
                float4 _node_8048_var = tex2D(_node_8048,TRANSFORM_TEX(i.uv0, _node_8048));
                float3 emissive = ((_node_4431_var.b*_node_5810*_Color.rgb)+(_node_8048_var.r*_node_7619.rgb)+((node_418-step(_node_3143_var.r,_node_2803))*_node_2079.rgb));
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
            uniform sampler2D _node_3143; uniform float4 _node_3143_ST;
            uniform float _node_2803;
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
                float4 _node_3143_var = tex2D(_node_3143,TRANSFORM_TEX(i.uv0, _node_3143));
                float node_418 = step(_node_3143_var.r,(_node_2803+0.03));
                clip(node_418 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
