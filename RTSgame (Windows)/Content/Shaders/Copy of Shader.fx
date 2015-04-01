//------- Constants --------
#define maxBones 59
#define MaxLights 2

int maxBoneNumber = maxBones;

float4x4 view;
float4x4 projection;
float4x4 world;
float4x4 lightViewProjection;
float4x4 bones[maxBones];
float3 lightDirection;
//float ambient;
float depthBias = 0.0025;
bool enableLighting;
bool textured;
float3 cameraPosition;
float shininess;
float specularIntensity;

float4 shadowColor;
float4 ambientColor;
float4 diffuseColor;

struct Light 
{
	float3 position;
    float4 color;   
    float power;
    float range;
};

Light lights[MaxLights];
int currentLights;

//------- Texture Samplers --------

Texture texture1;
sampler textureSampler = sampler_state { texture = (texture1); magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = mirror; AddressV = mirror;};

Texture shadowMap;
// old:
//sampler shadowMapSampler = sampler_state { texture = <shadowMap> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = clamp; AddressV = clamp;};

// enables shader model 3:
sampler shadowMapSampler = sampler_state { texture = <shadowMap> ; magfilter = POINT; minfilter = POINT; mipfilter=POINT; AddressU = clamp; AddressV = clamp;};


//------- Technique: Textured --------

struct TexturedVSIn
{
    float4 Position: POSITION0;
    float3 Normal:	 NORMAL0;
    float2 TexCoord: TEXCOORD0;
};

struct TexturedVSOut
{
    float4 Position: POSITION0;
    float3 Normal:	 TEXCOORD0;
    float2 TexCoord: TEXCOORD1;
	float4 WorldPos: TEXCOORD2;
	float3 EyeVector: TEXCOORD3;
};

TexturedVSOut TexturedVS(TexturedVSIn Input)
{	
	TexturedVSOut Output = (TexturedVSOut) 0;

	float4x4 Wvp = mul(mul(world, view), projection);
    
	Output.Position = mul(Input.Position, Wvp);	
	Output.TexCoord = Input.TexCoord;
	Output.Normal = normalize(mul(Input.Normal, world));
	Output.WorldPos = mul(Input.Position, world);
	Output.EyeVector = cameraPosition - Output.WorldPos;

	return Output;

}

float4 TexturedPS(TexturedVSOut Input) : COLOR
{
	float3 Normal = normalize(Input.Normal);
	float3 LightDirection = normalize(lightDirection);
	float3 EyeVector = normalize(Input.EyeVector);

	float4 BaseColor;

	if (textured)
		BaseColor = tex2D(textureSampler, Input.TexCoord);
	else BaseColor = float4(1, 1, 1, 0);
	float4 Ambient = float4(0.1, 0.1, 0.1, 0);
    float DirectionalIntensity = saturate(dot(LightDirection, Normal));

    float4 Diffuse = DirectionalIntensity * float4(0.5, 0.1, 0.1, 0);

	for (int i = 0; i < MaxLights; i++)
	{
		float4 PointIntensity = saturate(dot(-(normalize(Input.WorldPos - lights[i].position)), Normal));
		PointIntensity *= (lights[i].power * saturate((lights[i].range - length(Input.WorldPos - lights[i].position)) / lights[i].range));
		//PointIntensity *= (10 / dot(Input.WorldPos - lights[i].position, Input.WorldPos - lights[i].position));

		Diffuse += PointIntensity * lights[i].color;
	}

	float3 HalfAngle = normalize(EyeVector + LightDirection);
	float Specular = pow(saturate(dot(Normal, HalfAngle)), shininess);

	Specular *= specularIntensity;

	float4 FinalColor = BaseColor * (Ambient + Diffuse) + Specular;

	return FinalColor;
}

technique Textured
{
	pass Pass0
    {   
    	VertexShader = compile vs_1_1 TexturedVS();
        PixelShader  = compile ps_2_0 TexturedPS();
    }
}

//------- Technique: Animated --------

struct AnimatedVSIn
{
    float4 Position:	POSITION0;
    float3 Normal:		NORMAL0;
    float2 TexCoord:	TEXCOORD0;
    float4 BoneIndices: BLENDINDICES0;
    float4 BoneWeights: BLENDWEIGHT0;
};

TexturedVSOut AnimatedVS(AnimatedVSIn Input)
{
    TexturedVSOut Output = (TexturedVSOut) 0;
    
    // Blend between the weighted bone matrices.
    float4x4 SkinTransform = 0;
    
    SkinTransform += bones[Input.BoneIndices.x] * Input.BoneWeights.x;
    SkinTransform += bones[Input.BoneIndices.y] * Input.BoneWeights.y;
    SkinTransform += bones[Input.BoneIndices.z] * Input.BoneWeights.z;
    SkinTransform += bones[Input.BoneIndices.w] * Input.BoneWeights.w;
    
    // Skin the vertex position.
    float4 Position = mul(Input.Position, SkinTransform);
    
	//generate the world-view-projection matrix
    float4x4 Wvp = mul(mul(world, view), projection);

	Output.Position = mul(Position, Wvp);
	Output.Normal = normalize(mul(mul(Input.Normal, SkinTransform), world));
	Output.WorldPos = mul(Input.Position, world);
    Output.TexCoord = Input.TexCoord;
    Output.EyeVector = cameraPosition - Output.WorldPos;

    return Output;
}

technique Animated
{
    pass Pass0
    {
        VertexShader = compile vs_1_1 AnimatedVS();
        PixelShader = compile ps_2_0 TexturedPS();
    }
}

//------- Technique: ShadowMap --------

 struct ShadowMapVSOut
 {
    float4 Position: POSITION0;
    float  Depth:	 TEXCOORD0;
 };
  
 ShadowMapVSOut ShadowMapVS(float4 Position: POSITION0)
 {
	ShadowMapVSOut Output = (ShadowMapVSOut) 0;
 
    Output.Position = mul(Position, mul(world, lightViewProjection));
    Output.Depth = Output.Position.z / Output.Position.w;
 
    return Output;
 }
 
 float4 ShadowMapPS(ShadowMapVSOut Input) : COLOR
 {
    return float4(Input.Depth, 0, 0, 0);
 }
 
 technique ShadowMap
 {
    pass Pass0
    {
        VertexShader = compile vs_1_1 ShadowMapVS();
        PixelShader = compile ps_2_0 ShadowMapPS();
    }
 }

 //------- Technique: Shadowed --------

 struct ShadowedVSOut
 {
    float4 Position: POSITION0;
	float3 Normal:	 TEXCOORD0;
	float2 TexCoord: TEXCOORD1;
	float4 WorldPos: TEXCOORD2;
	float3 EyeVector: TEXCOORD3;
 };
 
 ShadowedVSOut ShadowedVS(TexturedVSIn Input)
 {
    ShadowedVSOut Output = (ShadowedVSOut) 0;

    float4x4 Wvp = mul(mul(world, view), projection);
    
    Output.Position = mul(Input.Position, Wvp);
    //Output.Normal =  normalize(mul(Input.Normal, world));
	Output.Normal =  mul(Input.Normal, world);
    Output.TexCoord = Input.TexCoord;
    Output.WorldPos = mul(Input.Position, world);
	Output.EyeVector = cameraPosition - Output.WorldPos;
    
    return Output;
 }
 
 float4 ShadowedPS(ShadowedVSOut Input) : COLOR
 {
	float lightPower = length(Input.Normal);
	//float lightPower = Input.Normal.xyz;
	float3 Normal = normalize(Input.Normal);
	float3 LightDirection = normalize(lightDirection);
	float3 EyeVector = normalize(Input.EyeVector);

	float4 BaseColor;

	if (textured)
		BaseColor = tex2D(textureSampler, Input.TexCoord);
	else
		BaseColor = float4(1, 1, 1, 0);
	
	BaseColor *= lightPower;

	float4 Ambient = ambientColor;
    float DirectionalIntensity = saturate(dot(LightDirection, Normal));

	float4 Diffuse = DirectionalIntensity * diffuseColor;

	for (int i = 0; i < MaxLights; i++)
	{
		float4 PointIntensity = saturate(dot(-(normalize(Input.WorldPos - lights[i].position)), Normal));
		PointIntensity *= (lights[i].power * saturate((lights[i].range - length(Input.WorldPos - lights[i].position)) / lights[i].range));
		//PointIntensity *= (10 / dot(Input.WorldPos - lights[i].position, Input.WorldPos - lights[i].position));

		Diffuse += PointIntensity * lights[i].color;
	}

	float3 HalfAngle = normalize(EyeVector + LightDirection);
	float Specular = pow(saturate(dot(Normal, HalfAngle)), shininess);

	Specular *= specularIntensity;

	float4 FinalColor = BaseColor * (Ambient + Diffuse) + Specular;

    float4 LightingPosition = mul(Input.WorldPos, lightViewProjection);
    
    // Find the position in the shadow map for this pixel
	float2 ShadowTexCoord = 0.5 * LightingPosition.xy / 
                            LightingPosition.w + float2(0.5, 0.5);
    ShadowTexCoord.y = 1 - ShadowTexCoord.y;

    // Get the current depth stored in the shadow map
    float Shadowdepth = tex2D(shadowMapSampler, ShadowTexCoord).r;    
    
    // Calculate the current pixel depth
    // The bias is used to prevent floating point errors that occur when
    // the pixel of the occluder is being drawn
    float Ourdepth = (LightingPosition.z / LightingPosition.w) - depthBias;

    // Check to see if this pixel is in front or behind the value in the shadow map
    if (Shadowdepth < Ourdepth)
    {
        // Shadow the pixel by lowering the intensity
        //FinalColor *= float4(0.65, 0.65, 0.9, 0);
		FinalColor *= shadowColor;
    };

    return FinalColor;
 }
 
 technique Shadowed
 {
	pass Pass0
    {
        VertexShader = compile vs_1_1 ShadowedVS();
        PixelShader = compile ps_2_0 ShadowedPS();
    }
 }

  //------- Technique: AnimatedShadowMap --------

 struct AnimatedShadowMapVSIn
 {
    float4 Position:	POSITION0;
	float4 BoneIndices: BLENDINDICES0;
    float4 BoneWeights: BLENDWEIGHT0;
 };
  
 ShadowMapVSOut AnimatedShadowMapVS(AnimatedShadowMapVSIn Input)
 {
	ShadowMapVSOut Output = (ShadowMapVSOut) 0;
 
	// Blend between the weighted bone matrices.
    float4x4 SkinTransform = 0;
    
    SkinTransform += bones[Input.BoneIndices.x] * Input.BoneWeights.x;
    SkinTransform += bones[Input.BoneIndices.y] * Input.BoneWeights.y;
    SkinTransform += bones[Input.BoneIndices.z] * Input.BoneWeights.z;
    SkinTransform += bones[Input.BoneIndices.w] * Input.BoneWeights.w;
    
    // Skin the vertex position.
    float4 Position = mul(Input.Position, SkinTransform);

    Output.Position = mul(Position, mul(world, lightViewProjection));
    Output.Depth = Output.Position.z / Output.Position.w;
 
    return Output;
 }
  
 technique AnimatedShadowMap
 {
    pass Pass0
    {
        VertexShader = compile vs_1_1 AnimatedShadowMapVS();
        PixelShader = compile ps_2_0 ShadowMapPS();
    }
 }

 //------- Technique: AnimatedShadowed --------
 
 ShadowedVSOut AnimatedShadowedVS(AnimatedVSIn Input)
 {
     ShadowedVSOut Output = (ShadowedVSOut) 0;

	// Blend between the weighted bone matrices.
    float4x4 SkinTransform = 0;
    
    SkinTransform += bones[Input.BoneIndices.x] * Input.BoneWeights.x;
    SkinTransform += bones[Input.BoneIndices.y] * Input.BoneWeights.y;
    SkinTransform += bones[Input.BoneIndices.z] * Input.BoneWeights.z;
    SkinTransform += bones[Input.BoneIndices.w] * Input.BoneWeights.w;
    
    // Skin the vertex position.
    float4 Position = mul(Input.Position, SkinTransform);

    float4x4 Wvp = mul(mul(world, view), projection);
    
    Output.Position = mul(Position, Wvp);
    Output.Normal = normalize(mul(mul(Input.Normal, SkinTransform), world));
    Output.TexCoord = Input.TexCoord;
    Output.WorldPos = mul(Position, world);
	Output.EyeVector = cameraPosition - Output.WorldPos;
    
    return Output;
 }
  
 technique AnimatedShadowed
 {
	pass Pass0
    {
        VertexShader = compile vs_1_1 AnimatedShadowedVS();
        PixelShader = compile ps_2_0 ShadowedPS();
    }
 }








// -------------------
// HARDWARE INSTANCING
// -------------------


// Vertex shader helper function shared between the two techniques.
TexturedVSOut VertexShaderCommon(TexturedVSIn input, float4x4 instanceTransform)
{
    TexturedVSOut output;

    // Apply the world and camera matrices to compute the output position.
    float4 worldPosition = mul(input.Position, instanceTransform);
    float4 viewPosition = mul(worldPosition, view);
    output.Position = mul(viewPosition, projection);

	output.TexCoord = input.TexCoord;
	output.Normal = normalize(mul(input.Normal, instanceTransform));
	output.WorldPos = worldPosition;
	output.EyeVector = cameraPosition - viewPosition;

    return output;
}


// Hardware instancing reads the per-instance world transform from a secondary vertex stream.
TexturedVSOut HardwareInstancingVertexShader(TexturedVSIn input,
                                                  float4x4 instanceTransform : BLENDWEIGHT)
{
    return VertexShaderCommon(input, mul(world, transpose(instanceTransform)));
}

// Hardware instancing technique.
technique HardwareInstancing
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 HardwareInstancingVertexShader();
		PixelShader = compile ps_2_0 TexturedPS();
    }
}



 //-----------------------------
 // Hardware shadow technique

 technique ShadowedHardware
 {
	pass Pass1
    {
		VertexShader = compile vs_2_0 HardwareInstancingVertexShader();
        PixelShader = compile ps_2_0 ShadowedPS();
    }
 }



 //------- Technique: ShadowMap --------

  ShadowMapVSOut ShadowMapVSHardware2(float4 Position: POSITION0, float4x4 instanceTransform : BLENDWEIGHT)
 {
	ShadowMapVSOut Output = (ShadowMapVSOut) 0;
    
    Output.Position = mul(Position, mul(instanceTransform, lightViewProjection));
    Output.Depth = Output.Position.z / Output.Position.w;
 
    return Output;
 }

 // Hardware instancing reads the per-instance world transform from a secondary vertex stream.
ShadowMapVSOut ShadowMapVSHardware1(float4 Position: POSITION0,
                                                  float4x4 instanceTransform : BLENDWEIGHT)
{
    return ShadowMapVSHardware2(Position, mul(world, transpose(instanceTransform)));
}

 technique ShadowMapHardware
 {
    pass Pass0
    {
        VertexShader = compile vs_2_0 ShadowMapVSHardware1();
        PixelShader = compile ps_2_0 ShadowMapPS();
    }
 }

