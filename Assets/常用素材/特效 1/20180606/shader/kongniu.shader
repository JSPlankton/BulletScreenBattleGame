// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33424,y:32741,varname:node_4013,prsc:2|alpha-2311-OUT,refract-3426-OUT;n:type:ShaderForge.SFN_Tex2d,id:2121,x:32637,y:33121,ptovrint:False,ptlb:node_2121,ptin:_node_2121,varname:node_2121,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:34b33a3250e98304a8b7d615f1bdb69b,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2835,x:32374,y:32920,ptovrint:False,ptlb:node_2835,ptin:_node_2835,varname:node_2835,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cc0423763bc0470429e25421eea78f02,ntxv:0,isnm:False|UVIN-9834-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6594,x:32374,y:32712,ptovrint:False,ptlb:node_6594,ptin:_node_6594,varname:node_6594,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c48b1bd0aee7d4b4ca0b69bfdcf604fe,ntxv:0,isnm:False|UVIN-8108-UVOUT;n:type:ShaderForge.SFN_Panner,id:8108,x:32180,y:32712,varname:node_8108,prsc:2,spu:0.9,spv:1|UVIN-8000-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8000,x:32001,y:32712,varname:node_8000,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:9834,x:32191,y:32920,varname:node_9834,prsc:2,spu:-1.5,spv:-1.3|UVIN-8516-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8516,x:32012,y:32920,varname:node_8516,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:5997,x:32542,y:32835,varname:node_5997,prsc:2|A-6594-R,B-2835-R;n:type:ShaderForge.SFN_Multiply,id:3426,x:32821,y:32861,varname:node_3426,prsc:2|A-5997-OUT,B-4782-OUT,C-9970-A,D-2121-R;n:type:ShaderForge.SFN_Vector2,id:4782,x:32542,y:32955,varname:node_4782,prsc:2,v1:0.1911765,v2:0.1869593;n:type:ShaderForge.SFN_VertexColor,id:9970,x:32934,y:33127,varname:node_9970,prsc:2;n:type:ShaderForge.SFN_Vector1,id:2311,x:33094,y:32825,varname:node_2311,prsc:2,v1:0;proporder:2835-6594-2121;pass:END;sub:END;*/

Shader "Shader Forge/kongniu" {
    Properties {
        _node_2835 ("node_2835", 2D) = "white" {}
        _node_6594 ("node_6594", 2D) = "white" {}
        _node_2121 ("node_2121", 2D) = "white" {}
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
            uniform sampler2D _node_2121; uniform float4 _node_2121_ST;
            uniform sampler2D _node_2835; uniform float4 _node_2835_ST;
            uniform sampler2D _node_6594; uniform float4 _node_6594_ST;
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
                float4 node_1302 = _Time + _TimeEditor;
                float2 node_8108 = (i.uv0+node_1302.g*float2(0.9,1));
                float4 _node_6594_var = tex2D(_node_6594,TRANSFORM_TEX(node_8108, _node_6594));
                float2 node_9834 = (i.uv0+node_1302.g*float2(-1.5,-1.3));
                float4 _node_2835_var = tex2D(_node_2835,TRANSFORM_TEX(node_9834, _node_2835));
                float4 _node_2121_var = tex2D(_node_2121,TRANSFORM_TEX(i.uv0, _node_2121));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + ((_node_6594_var.r*_node_2835_var.r)*float2(0.1911765,0.1869593)*i.vertexColor.a*_node_2121_var.r);
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
