// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33328,y:32722,varname:node_4013,prsc:2|emission-9747-OUT,alpha-1929-OUT;n:type:ShaderForge.SFN_Tex2d,id:5821,x:32432,y:33132,ptovrint:False,ptlb:node_5821,ptin:_node_5821,varname:node_5821,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:4d58956b0f40fac43bdffa8925a9771d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Step,id:5259,x:32723,y:33212,varname:node_5259,prsc:2|A-5821-R,B-721-A;n:type:ShaderForge.SFN_Multiply,id:1929,x:32912,y:33212,varname:node_1929,prsc:2|A-9210-R,B-5259-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4520,x:32722,y:32546,ptovrint:False,ptlb:node_4520,ptin:_node_4520,varname:node_4520,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_VertexColor,id:721,x:32440,y:33331,varname:node_721,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9747,x:32916,y:32833,varname:node_9747,prsc:2|A-9210-RGB,B-5259-OUT,C-4520-OUT;n:type:ShaderForge.SFN_Tex2d,id:9210,x:32386,y:32853,ptovrint:False,ptlb:node_9210,ptin:_node_9210,varname:node_9210,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e2173f670ccae4c44a7cf2bb5a2e941f,ntxv:0,isnm:False;proporder:5821-4520-9210;pass:END;sub:END;*/

Shader "Shader Forge/rongjiexian 2" {
    Properties {
        _node_5821 ("node_5821", 2D) = "white" {}
        _node_4520 ("node_4520", Float ) = 1
        _node_9210 ("node_9210", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            uniform sampler2D _node_5821; uniform float4 _node_5821_ST;
            uniform float _node_4520;
            uniform sampler2D _node_9210; uniform float4 _node_9210_ST;
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
                float4 _node_9210_var = tex2D(_node_9210,TRANSFORM_TEX(i.uv0, _node_9210));
                float4 _node_5821_var = tex2D(_node_5821,TRANSFORM_TEX(i.uv0, _node_5821));
                float node_5259 = step(_node_5821_var.r,i.vertexColor.a);
                float3 emissive = (_node_9210_var.rgb*node_5259*_node_4520);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_node_9210_var.r*node_5259));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
