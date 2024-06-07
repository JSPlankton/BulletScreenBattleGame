// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:34015,y:33360,varname:node_4013,prsc:2|emission-8356-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32824,y:33209,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.9066937,c3:0.7058823,c4:1;n:type:ShaderForge.SFN_Tex2d,id:7284,x:32820,y:33431,ptovrint:False,ptlb:node_7284,ptin:_node_7284,varname:node_7284,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:36c639540c6a080498c9d04f2d5a2341,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:571,x:32824,y:33033,ptovrint:False,ptlb:node_571,ptin:_node_571,varname:node_571,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bee37f8bbd6e05843bd6c15a544deb90,ntxv:0,isnm:False|UVIN-4122-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:6702,x:32918,y:32830,ptovrint:False,ptlb:node_6702,ptin:_node_6702,varname:node_6702,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e366cbff1917c6b45a381a8a98b70244,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:9457,x:33335,y:33378,varname:node_9457,prsc:2|A-3177-OUT,B-6411-OUT;n:type:ShaderForge.SFN_Panner,id:4122,x:32651,y:33033,varname:node_4122,prsc:2,spu:0,spv:1|UVIN-8436-UVOUT,DIST-9515-OUT;n:type:ShaderForge.SFN_TexCoord,id:8436,x:32436,y:33033,varname:node_8436,prsc:2,uv:0;n:type:ShaderForge.SFN_Slider,id:9515,x:32332,y:33196,ptovrint:False,ptlb:node_9515,ptin:_node_9515,varname:node_9515,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.04273504,max:1;n:type:ShaderForge.SFN_Multiply,id:3177,x:33141,y:33097,varname:node_3177,prsc:2|A-571-R,B-1304-RGB,C-4290-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4290,x:32820,y:33371,ptovrint:False,ptlb:node_4290,ptin:_node_4290,varname:node_4290,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:6411,x:33043,y:33431,varname:node_6411,prsc:2|A-7284-RGB,B-66-OUT;n:type:ShaderForge.SFN_ValueProperty,id:66,x:32820,y:33617,ptovrint:False,ptlb:node_66,ptin:_node_66,varname:node_66,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.7;n:type:ShaderForge.SFN_Tex2d,id:7053,x:32800,y:33764,ptovrint:False,ptlb:node_7053,ptin:_node_7053,varname:node_7053,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0d511cad371450e4189e22d208c94eb2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Step,id:7169,x:33069,y:33946,varname:node_7169,prsc:2|A-7053-R,B-6641-A;n:type:ShaderForge.SFN_Multiply,id:8356,x:33642,y:33835,varname:node_8356,prsc:2|A-9457-OUT,B-7169-OUT;n:type:ShaderForge.SFN_VertexColor,id:6641,x:32304,y:33912,varname:node_6641,prsc:2;proporder:1304-571-9515-7284-4290-66-7053;pass:END;sub:END;*/

Shader "Shader Forge/cirongjie" {
    Properties {
        _Color ("Color", Color) = (1,0.9066937,0.7058823,1)
        _node_571 ("node_571", 2D) = "white" {}
        _node_9515 ("node_9515", Range(0, 1)) = 0.04273504
        _node_7284 ("node_7284", 2D) = "white" {}
        _node_4290 ("node_4290", Float ) = 1
        _node_66 ("node_66", Float ) = 0.7
        _node_7053 ("node_7053", 2D) = "white" {}
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
            uniform float4 _Color;
            uniform sampler2D _node_7284; uniform float4 _node_7284_ST;
            uniform sampler2D _node_571; uniform float4 _node_571_ST;
            uniform float _node_9515;
            uniform float _node_4290;
            uniform float _node_66;
            uniform sampler2D _node_7053; uniform float4 _node_7053_ST;
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
                float2 node_4122 = (i.uv0+_node_9515*float2(0,1));
                float4 _node_571_var = tex2D(_node_571,TRANSFORM_TEX(node_4122, _node_571));
                float4 _node_7284_var = tex2D(_node_7284,TRANSFORM_TEX(i.uv0, _node_7284));
                float4 _node_7053_var = tex2D(_node_7053,TRANSFORM_TEX(i.uv0, _node_7053));
                float3 emissive = (((_node_571_var.r*_Color.rgb*_node_4290)+(_node_7284_var.rgb*_node_66))*step(_node_7053_var.r,i.vertexColor.a));
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
