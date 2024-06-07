// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33724,y:32769,varname:node_4013,prsc:2|emission-5338-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:33090,y:33183,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.6314401,c2:0.4191176,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:1268,x:32345,y:33204,ptovrint:False,ptlb:node_1268,ptin:_node_1268,varname:node_1268,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e80817c84b505e242825bf471a0a0e5a,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7255,x:32754,y:32812,ptovrint:False,ptlb:node_7255,ptin:_node_7255,varname:node_7255,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:79c964ea1dbfc5e43878712b0444041a,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Step,id:3428,x:32730,y:33042,varname:node_3428,prsc:2|A-8824-A,B-1268-R;n:type:ShaderForge.SFN_Multiply,id:8414,x:33010,y:32905,varname:node_8414,prsc:2|A-7255-R,B-3428-OUT;n:type:ShaderForge.SFN_VertexColor,id:8824,x:32483,y:32694,varname:node_8824,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:7159,x:33066,y:33106,ptovrint:False,ptlb:node_7159,ptin:_node_7159,varname:node_7159,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:3348,x:32736,y:33365,ptovrint:False,ptlb:node_3348,ptin:_node_3348,varname:node_3348,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cc0423763bc0470429e25421eea78f02,ntxv:0,isnm:False|UVIN-4374-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6961,x:32736,y:33556,ptovrint:False,ptlb:node_6961,ptin:_node_6961,varname:node_6961,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c48b1bd0aee7d4b4ca0b69bfdcf604fe,ntxv:0,isnm:False|UVIN-4633-UVOUT;n:type:ShaderForge.SFN_Panner,id:4374,x:32558,y:33398,varname:node_4374,prsc:2,spu:0.2,spv:0.3|UVIN-2822-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:2822,x:32370,y:33408,varname:node_2822,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:4633,x:32541,y:33577,varname:node_4633,prsc:2,spu:-0.35,spv:-0.15|UVIN-6583-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:6583,x:32353,y:33587,varname:node_6583,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:2704,x:33110,y:33514,varname:node_2704,prsc:2|A-3348-R,B-6961-R,C-936-OUT,D-8414-OUT,E-4410-RGB;n:type:ShaderForge.SFN_ValueProperty,id:936,x:32808,y:33757,ptovrint:False,ptlb:liuwen,ptin:_liuwen,varname:node_936,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3468,x:33271,y:32942,varname:node_3468,prsc:2|A-8414-OUT,B-7159-OUT,C-1304-RGB;n:type:ShaderForge.SFN_Color,id:4410,x:33065,y:33715,ptovrint:False,ptlb:node_4410,ptin:_node_4410,varname:node_4410,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Add,id:5338,x:33550,y:33058,varname:node_5338,prsc:2|A-3468-OUT,B-2704-OUT;proporder:1304-7255-1268-7159-3348-6961-936-4410;pass:END;sub:END;*/

Shader "Shader Forge/rongjiefazhen" {
    Properties {
        _Color ("Color", Color) = (0.6314401,0.4191176,1,1)
        _node_7255 ("node_7255", 2D) = "white" {}
        _node_1268 ("node_1268", 2D) = "white" {}
        _node_7159 ("node_7159", Float ) = 1
        _node_3348 ("node_3348", 2D) = "white" {}
        _node_6961 ("node_6961", 2D) = "white" {}
        _liuwen ("liuwen", Float ) = 1
        _node_4410 ("node_4410", Color) = (1,1,1,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
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
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform sampler2D _node_1268; uniform float4 _node_1268_ST;
            uniform sampler2D _node_7255; uniform float4 _node_7255_ST;
            uniform float _node_7159;
            uniform sampler2D _node_3348; uniform float4 _node_3348_ST;
            uniform sampler2D _node_6961; uniform float4 _node_6961_ST;
            uniform float _liuwen;
            uniform float4 _node_4410;
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
////// Lighting:
////// Emissive:
                float4 _node_7255_var = tex2D(_node_7255,TRANSFORM_TEX(i.uv0, _node_7255));
                float4 _node_1268_var = tex2D(_node_1268,TRANSFORM_TEX(i.uv0, _node_1268));
                float node_8414 = (_node_7255_var.r*step(i.vertexColor.a,_node_1268_var.r));
                float4 node_3 = _Time + _TimeEditor;
                float2 node_4374 = (i.uv0+node_3.g*float2(0.2,0.3));
                float4 _node_3348_var = tex2D(_node_3348,TRANSFORM_TEX(node_4374, _node_3348));
                float2 node_4633 = (i.uv0+node_3.g*float2(-0.35,-0.15));
                float4 _node_6961_var = tex2D(_node_6961,TRANSFORM_TEX(node_4633, _node_6961));
                float3 emissive = ((node_8414*_node_7159*_Color.rgb)+(_node_3348_var.r*_node_6961_var.r*_liuwen*node_8414*_node_4410.rgb));
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
