// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:34113,y:33009,varname:node_4013,prsc:2|emission-1520-OUT;n:type:ShaderForge.SFN_Tex2d,id:3795,x:32818,y:32913,ptovrint:False,ptlb:node_3795,ptin:_node_3795,varname:node_3795,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c48b1bd0aee7d4b4ca0b69bfdcf604fe,ntxv:0,isnm:False|UVIN-9480-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:2865,x:32818,y:33138,ptovrint:False,ptlb:node_2865,ptin:_node_2865,varname:node_2865,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cc0423763bc0470429e25421eea78f02,ntxv:0,isnm:False|UVIN-2903-UVOUT;n:type:ShaderForge.SFN_Panner,id:9480,x:32595,y:32913,varname:node_9480,prsc:2,spu:0.1,spv:0.2|UVIN-7873-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:7873,x:32409,y:32913,varname:node_7873,prsc:2,uv:0;n:type:ShaderForge.SFN_Panner,id:2903,x:32609,y:33138,varname:node_2903,prsc:2,spu:-0.07,spv:-0.18|UVIN-8587-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8587,x:32409,y:33138,varname:node_8587,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:1282,x:33052,y:33070,varname:node_1282,prsc:2|A-3795-R,B-2865-R;n:type:ShaderForge.SFN_Multiply,id:3720,x:33451,y:33100,varname:node_3720,prsc:2|A-1282-OUT,B-532-RGB,C-2256-R,D-832-OUT;n:type:ShaderForge.SFN_Tex2d,id:532,x:33052,y:33296,ptovrint:False,ptlb:node_532,ptin:_node_532,varname:node_532,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5593c1ce05ed09b47a4047a315c67ff3,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2256,x:33030,y:33525,ptovrint:False,ptlb:node_2256,ptin:_node_2256,varname:node_2256,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:6c42bbdbbc39ab84ba564b7d7958bf6b,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:832,x:33339,y:33397,ptovrint:False,ptlb:node_832,ptin:_node_832,varname:node_832,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Multiply,id:1520,x:33693,y:33239,varname:node_1520,prsc:2|A-3720-OUT,B-3072-A;n:type:ShaderForge.SFN_VertexColor,id:3072,x:33629,y:33555,varname:node_3072,prsc:2;proporder:3795-2865-532-2256-832;pass:END;sub:END;*/

Shader "Shader Forge/niuwen" {
    Properties {
        _node_3795 ("node_3795", 2D) = "white" {}
        _node_2865 ("node_2865", 2D) = "white" {}
        _node_532 ("node_532", 2D) = "white" {}
        _node_2256 ("node_2256", 2D) = "white" {}
        _node_832 ("node_832", Float ) = 2
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
            Blend SrcAlpha One
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
            uniform sampler2D _node_3795; uniform float4 _node_3795_ST;
            uniform sampler2D _node_2865; uniform float4 _node_2865_ST;
            uniform sampler2D _node_532; uniform float4 _node_532_ST;
            uniform sampler2D _node_2256; uniform float4 _node_2256_ST;
            uniform float _node_832;
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
                float4 node_5566 = _Time + _TimeEditor;
                float2 node_9480 = (i.uv0+node_5566.g*float2(0.1,0.2));
                float4 _node_3795_var = tex2D(_node_3795,TRANSFORM_TEX(node_9480, _node_3795));
                float2 node_2903 = (i.uv0+node_5566.g*float2(-0.07,-0.18));
                float4 _node_2865_var = tex2D(_node_2865,TRANSFORM_TEX(node_2903, _node_2865));
                float4 _node_532_var = tex2D(_node_532,TRANSFORM_TEX(i.uv0, _node_532));
                float4 _node_2256_var = tex2D(_node_2256,TRANSFORM_TEX(i.uv0, _node_2256));
                float3 emissive = (((_node_3795_var.r*_node_2865_var.r)*_node_532_var.rgb*_node_2256_var.r*_node_832)*i.vertexColor.a);
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
