// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33951,y:32391,varname:node_4013,prsc:2|emission-125-OUT,olwid-5656-OUT,olcol-7271-RGB;n:type:ShaderForge.SFN_Color,id:1304,x:33254,y:32956,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.4758621,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3395,x:32920,y:32297,ptovrint:False,ptlb:node_3395,ptin:_node_3395,varname:node_3395,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:93d85c7df4c15e74dbc16e10c20f28ac,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Sin,id:7268,x:32715,y:32566,varname:node_7268,prsc:2|IN-7796-T;n:type:ShaderForge.SFN_Time,id:7796,x:32530,y:32566,varname:node_7796,prsc:2;n:type:ShaderForge.SFN_Add,id:6865,x:32976,y:32613,varname:node_6865,prsc:2|A-7268-OUT,B-5778-OUT;n:type:ShaderForge.SFN_Vector1,id:5778,x:32748,y:32744,varname:node_5778,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:512,x:33254,y:32600,varname:node_512,prsc:2|A-6865-OUT,B-691-OUT;n:type:ShaderForge.SFN_Vector1,id:691,x:32929,y:32766,varname:node_691,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Add,id:125,x:33491,y:32336,varname:node_125,prsc:2|A-5599-OUT,B-4536-OUT;n:type:ShaderForge.SFN_Tex2d,id:5772,x:33254,y:32764,ptovrint:False,ptlb:node_5772,ptin:_node_5772,varname:node_5772,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:d2911be3689399e4d826593807821e43,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4536,x:33470,y:32682,varname:node_4536,prsc:2|A-512-OUT,B-5772-R,C-1304-RGB;n:type:ShaderForge.SFN_Multiply,id:5599,x:33279,y:32299,varname:node_5599,prsc:2|A-3395-RGB,B-4694-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4694,x:33078,y:32387,ptovrint:False,ptlb:node_4694,ptin:_node_4694,varname:node_4694,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Slider,id:5656,x:33502,y:32848,ptovrint:False,ptlb:node_5656,ptin:_node_5656,varname:node_5656,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.01,max:1;n:type:ShaderForge.SFN_Color,id:7271,x:33659,y:32995,ptovrint:False,ptlb:node_7271,ptin:_node_7271,varname:node_7271,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.4878296,c3:0.2573529,c4:1;proporder:1304-3395-5772-4694-5656-7271;pass:END;sub:END;*/

Shader "Shader Forge/shizhui" {
    Properties {
        _Color ("Color", Color) = (1,0.4758621,0,1)
        _node_3395 ("node_3395", 2D) = "white" {}
        _node_5772 ("node_5772", 2D) = "white" {}
        _node_4694 ("node_4694", Float ) = 0.5
        _node_5656 ("node_5656", Range(0, 1)) = 0.01
        _node_7271 ("node_7271", Color) = (1,0.4878296,0.2573529,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _node_5656;
            uniform float4 _node_7271;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(float4(v.vertex.xyz + v.normal*_node_5656,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_node_7271.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
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
            uniform sampler2D _node_3395; uniform float4 _node_3395_ST;
            uniform sampler2D _node_5772; uniform float4 _node_5772_ST;
            uniform float _node_4694;
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
////// Lighting:
////// Emissive:
                float4 _node_3395_var = tex2D(_node_3395,TRANSFORM_TEX(i.uv0, _node_3395));
                float4 node_7796 = _Time + _TimeEditor;
                float4 _node_5772_var = tex2D(_node_5772,TRANSFORM_TEX(i.uv0, _node_5772));
                float3 emissive = ((_node_3395_var.rgb*_node_4694)+(((sin(node_7796.g)+2.0)*0.5)*_node_5772_var.r*_Color.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
