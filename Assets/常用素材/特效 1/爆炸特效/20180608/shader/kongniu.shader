// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33209,y:32677,varname:node_4013,prsc:2|alpha-9116-OUT,refract-4992-OUT;n:type:ShaderForge.SFN_Tex2d,id:2457,x:32503,y:33094,ptovrint:False,ptlb:node_2457,ptin:_node_2457,varname:node_2457,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1f276df0e92ee5a4a95bbef8a4e38814,ntxv:0,isnm:False|UVIN-4191-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2284,x:32487,y:32880,ptovrint:False,ptlb:node_2284,ptin:_node_2284,varname:node_2284,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c360ead16a7ac8145957892dca606172,ntxv:0,isnm:False|UVIN-1023-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2524,x:32694,y:33162,ptovrint:False,ptlb:node_2524,ptin:_node_2524,varname:node_2524,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cea327e4e0b2273489ffbf99bcc9485e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Panner,id:1023,x:32328,y:32880,varname:node_1023,prsc:2,spu:0.5,spv:0.4|UVIN-3201-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:3201,x:32143,y:32880,varname:node_3201,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:4191,x:32316,y:33094,varname:node_4191,prsc:2,spu:-0.35,spv:-0.55|UVIN-619-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:619,x:32131,y:33094,varname:node_619,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:1553,x:32694,y:32986,varname:node_1553,prsc:2|A-2284-R,B-2457-R;n:type:ShaderForge.SFN_Multiply,id:4992,x:32903,y:33071,varname:node_4992,prsc:2|A-1553-OUT,B-2524-R,C-6356-OUT,D-9838-A;n:type:ShaderForge.SFN_Vector2,id:6356,x:32694,y:33324,varname:node_6356,prsc:2,v1:0.1617647,v2:0.1617647;n:type:ShaderForge.SFN_Vector1,id:9116,x:32842,y:32826,varname:node_9116,prsc:2,v1:0;n:type:ShaderForge.SFN_VertexColor,id:9838,x:32694,y:33447,varname:node_9838,prsc:2;proporder:2457-2284-2524;pass:END;sub:END;*/

Shader "Shader Forge/kongniu" {
    Properties {
        _node_2457 ("node_2457", 2D) = "white" {}
        _node_2284 ("node_2284", 2D) = "white" {}
        _node_2524 ("node_2524", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform sampler2D _node_2457; uniform float4 _node_2457_ST;
            uniform sampler2D _node_2284; uniform float4 _node_2284_ST;
            uniform sampler2D _node_2524; uniform float4 _node_2524_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float4 node_6024 = _Time + _TimeEditor;
                float2 node_1023 = (i.uv0+node_6024.g*float2(0.5,0.4));
                float4 _node_2284_var = tex2D(_node_2284,TRANSFORM_TEX(node_1023, _node_2284));
                float2 node_4191 = (i.uv0+node_6024.g*float2(-0.35,-0.55));
                float4 _node_2457_var = tex2D(_node_2457,TRANSFORM_TEX(node_4191, _node_2457));
                float4 _node_2524_var = tex2D(_node_2524,TRANSFORM_TEX(i.uv0, _node_2524));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + ((_node_2284_var.r*_node_2457_var.r)*_node_2524_var.r*float2(0.1617647,0.1617647)*i.vertexColor.a);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,0.0),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
