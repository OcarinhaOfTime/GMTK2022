#ifndef NUTILS_INCLUDE
#define NUTILS_INCLUDE

inline float hash21(float2 p){
    return frac(sin(dot(p, float2(12.9898,78.233))) * 43758.5453123);
}

inline float2 hash22(float2 p){
    float x = frac(sin(dot(p, float2(12.9898,78.233))) * 43758.5453123);
    float y = frac(cos(dot(p, float2(13.9753,79.322))) * 43650.54236);

    return float2(x, y);
}

inline float3 hash23(float2 p){
    float x = frac(sin(dot(p, float2(12.9898,78.233))) * 43758.5453123);
    float y = frac(cos(dot(p, float2(13.9753,79.322))) * 43650.54236);
    float z = frac(sin(dot(p, float2(11.9753,73.322))) * 48650.862965);

    return float3(x, y, z);
}

inline float rand(float2 st){
    return frac(sin(dot(st, float2(12.9898,78.233))) * 43758.5453123);
}

inline float rand(float st){
    return frac(sin(st) * 43758.5453123);
}

inline float2 rand2(float2 st){
    st = float2( dot(st,float2(127.1,311.7)), dot(st,float2(269.5,183.3)));
    return -1.0 + 2.0*frac(sin(st) * 43758.5453123);
}	

inline float3 mod289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
inline float2 mod289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
inline float3 permute(float3 x) { return mod289(((x*34.0)+1.0)*x); }

inline float4 permute(float4 x){return fmod(((x*34.0)+1.0)*x, 289.0);}
inline float4 taylorInvSqrt(float4 r){return 1.79284291400159 - 0.85373472095314 * r;}
inline float3 fade(float3 t) {return t*t*t*(t*(t*6.0-15.0)+10.0);}

inline float cnoise3d(float3 P){
    float3 Pi0 = floor(P); // Integer part for indexing
    float3 Pi1 = Pi0 + 1.0; // Integer part + 1
    Pi0 = fmod(Pi0, 289.0);
    Pi1 = fmod(Pi1, 289.0);
    float3 Pf0 = frac(P); // Fractional part for interpolation
    float3 Pf1 = Pf0 - 1.0; // Fractional part - 1.0
    float4 ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
    float4 iy = float4(Pi0.yy, Pi1.yy);
    float4 iz0 = Pi0.zzzz;
    float4 iz1 = Pi1.zzzz;

    float4 ixy = permute(permute(ix) + iy);
    float4 ixy0 = permute(ixy + iz0);
    float4 ixy1 = permute(ixy + iz1);

    float4 gx0 = ixy0 / 7.0;
    float4 gy0 = frac(floor(gx0) / 7.0) - 0.5;
    gx0 = frac(gx0);
    float4 gz0 = 0.5 - abs(gx0) - abs(gy0);
    float4 sz0 = step(gz0, 0.0);
    gx0 -= sz0 * (step(0.0, gx0) - 0.5);
    gy0 -= sz0 * (step(0.0, gy0) - 0.5);

    float4 gx1 = ixy1 / 7.0;
    float4 gy1 = frac(floor(gx1) / 7.0) - 0.5;
    gx1 = frac(gx1);
    float4 gz1 = (0.5) - abs(gx1) - abs(gy1);
    float4 sz1 = step(gz1, (0.0));
    gx1 -= sz1 * (step(0.0, gx1) - 0.5);
    gy1 -= sz1 * (step(0.0, gy1) - 0.5);

    float3 g000 = float3(gx0.x,gy0.x,gz0.x);
    float3 g100 = float3(gx0.y,gy0.y,gz0.y);
    float3 g010 = float3(gx0.z,gy0.z,gz0.z);
    float3 g110 = float3(gx0.w,gy0.w,gz0.w);
    float3 g001 = float3(gx1.x,gy1.x,gz1.x);
    float3 g101 = float3(gx1.y,gy1.y,gz1.y);
    float3 g011 = float3(gx1.z,gy1.z,gz1.z);
    float3 g111 = float3(gx1.w,gy1.w,gz1.w);

    float4 norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
    g000 *= norm0.x;
    g010 *= norm0.y;
    g100 *= norm0.z;
    g110 *= norm0.w;
    float4 norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
    g001 *= norm1.x;
    g011 *= norm1.y;
    g101 *= norm1.z;
    g111 *= norm1.w;

    float n000 = dot(g000, Pf0);
    float n100 = dot(g100, float3(Pf1.x, Pf0.yz));
    float n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
    float n110 = dot(g110, float3(Pf1.xy, Pf0.z));
    float n001 = dot(g001, float3(Pf0.xy, Pf1.z));
    float n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
    float n011 = dot(g011, float3(Pf0.x, Pf1.yz));
    float n111 = dot(g111, Pf1);

    float3 fade_xyz = fade(Pf0);
    float4 n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
    float2 n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
    float n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x); 
    return 2.2 * n_xyz;
}

inline float simplex_noise(float2 v){
    float4 C = float4(0.211324865405187,
            // (3.0-sqrt(3.0))/6.0
            0.366025403784439,
            // 0.5*(sqrt(3.0)-1.0)
            -0.577350269189626,
            // -1.0 + 2.0 * C.x
            0.024390243902439);
            
    float2 i  = floor(v + dot(v, C.yy));
    float2 x0 = v - i + dot(i, C.xx);

    float2 i1 = 0;
    i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
    float2 x1 = x0.xy + C.xx - i1;
    float2 x2 = x0.xy + C.zz;

    i = mod289(i);
    float3 p = permute(
            permute( i.y + float3(0.0, i1.y, 1.0))
                + i.x + float3(0.0, i1.x, 1.0 ));

    float3 m = max(0.5 - float3(
                        dot(x0,x0),
                        dot(x1,x1),
                        dot(x2,x2)
                        ), 0.0);

    m = m*m*m;
    float3 x = 2.0 * frac(p * C.www) - 1.0;
    float3 h = abs(x) - 0.5;
    float3 ox = floor(x + 0.5);
    float3 a0 = x - ox;

    m *= 1.79284291400159 - 0.85373472095314 * (a0*a0+h*h);

    float3 g = 0;
    g.x  = a0.x  * x0.x  + h.x  * x0.y;
    g.yz = a0.yz * float2(x1.x,x2.x) + h.yz * float2(x1.y,x2.y);
    return 130.0 * dot(m, g)*.5+.5;
}

inline float noise(float2 st){
    float2 i = floor(st);
    float2 f = frac(st);

    float a = rand(i);
    float b = rand(i + float2(1.0, 0.0));
    float c = rand(i + float2(0.0, 1.0));
    float d = rand(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(a, b, u.x) + (c - a)* u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
}

inline float snoise(float2 st){
    float2 i = floor(st);
    float2 f = frac(st);

    float a = rand(i);
    float b = rand(i + float2(1.0, 0.0));
    float c = rand(i + float2(0.0, 1.0));
    float d = rand(i + float2(1.0, 1.0));

    float2 u = f * f * (3.0 - 2.0 * f);
    float n = lerp(a, b, u.x) + (c - a)* u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
    return 2 * n - 1;
}

inline float value_noise(float2 st){
    float2 i = floor(st);
    float2 f = frac(st);

    float2 u = f*f*(3.0-2.0*f);

    return lerp( lerp( dot( hash21(i + float2(0.0,0.0) ), f - float2(0.0,0.0) ),
                     dot( hash21(i + float2(1.0,0.0) ), f - float2(1.0,0.0) ), u.x),
                lerp( dot( hash21(i + float2(0.0,1.0) ), f - float2(0.0,1.0) ),
                     dot( hash21(i + float2(1.0,1.0) ), f - float2(1.0,1.0) ), u.x), u.y);
}

inline float fbm(float2 st, int octaves, float lacunarity, float gain){
    float amp = .5;
    float val = 0;

    for(int i=0; i<octaves; i++){
        val += amp * noise(st);
        st *= lacunarity;
        amp *= gain;
    }

    return val;
}

inline float fbm(float2 st){
    float v = 0.0;
    float a = 0.5;
    float2 shift = 100;
    // Rotate to reduce axial bias
    float2x2 rot = float2x2(cos(0.5), sin(0.5),
                    -sin(0.5), cos(0.50));
    for (int i = 0; i < 5; ++i) {
        v += a * noise(st);
        st = mul(rot, st) * 2.0 + shift;
        a *= 0.5;
    }
    return v;
}

inline float turbulance(float2 st, int octaves, float lacunarity, float gain){
    float amp = .5;
    float val = 0;

    for(int i=0; i<octaves; i++){
        val += amp * abs(snoise(st));
        st *= lacunarity;
        amp *= gain;
    }

    return val;
}

inline float ridge(float2 st, int octaves, float lacunarity, float gain){
    float amp = .5;
    float val = 0;

    for(int i=0; i<octaves; i++){
        val += amp * abs(snoise(st));
        st *= lacunarity;
        amp *= gain;
    }

    val = 1 - val;
    val = val * val;
    return val;
}

inline float4 warp_fbm(float2 st){
    float2 q = float2(fbm(st), fbm(st + 1));
    float2 r;
    r.x = fbm(st + q + float2(1.7,9.2)+ 0.15*_Time.y);
    r.y = fbm(st + q + float2(8.3,2.8)+ 0.126*_Time.y);

    float f = fbm(st+r);
    float3 col = lerp(float3(0.101961,0.619608,0.666667),
        float3(0.666667,0.666667,0.498039),
        clamp((f*f)*4.0,0.0,1.0));

    col = lerp(col, float3(0,0,0.164706), clamp(length(q),0.0,1.0));
    col = lerp(col, float3(0.666667,1,1), clamp(length(r.x),0.0,1.0));

    return float4((f*f*f+.6*f*f+.5*f)*col,1.);
}

#endif