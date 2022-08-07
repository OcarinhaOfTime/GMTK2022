#ifndef VUTILS_INCLUDE
#define VUTILS_INCLUDE

#define PI 3.14159265359
#define TWO_PI 6.28318530718
#define HALF_PI 1.57079632679 

#define DEG2RAD 0.01745329252
#define RAD2DEG 57.2957795129 

inline float almost_identity(float x, float m, float n){
    if(x>m) return x;

    float a = 2*n-m;
    float b = 2*m-3*n;
    float t = x/m;

    return (a*t + b) *t * t + n; 
}

inline float impulse(float x, float k){
    float h = k*x;
    return h * exp(1-h);
}

inline float cubic_pulse(float x, float c, float w){
    x = abs(x-c);
    if(x>w) return 0;
    x /= w;

    return 1-x*x*(3-2*x);
}

inline float exp_step(float x, float k, float n){
    return exp(-k*pow(x, n));
}

inline float gain(float x, float k){
    float w = (x<0.5)?x:1.0-x;
    float a = .5*pow(2*w, k);
    return (x<0.5)?a:1.0-a;
}

inline float parabola(float x, float k){
    return pow(4*x*(1-x), k);
}

inline float pcurve(float x, float a, float b){
    float k = pow(a+b, a+b) / (pow(a,a) * pow(b,b));
    return k * pow(x, a) * pow(1-x, b);
}

inline float sinc(float x, float k){
    float a = PI * k * x - 1;
    return sin(a) / a;
}

inline float fsinc(float x, float k){
    float a = PI * k * x - 1;
    return abs(sin(a) / a);
}

inline float3 rgb2hsb(float3 c){
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz),
                 float4(c.gb, K.xy),
                 step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r),
                 float4(c.r, p.yzx),
                 step(p.x, c.r));
    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)),
                d / (q.x + e),
                q.x);
}

inline float3 hsb2rgb(float3 c){
    float3 rgb = clamp(abs(fmod(c.x*6.0+float3(0.0,4.0,2.0),
                    6.0)-3.0)-1.0,
                    0.0,
                    1.0 );
    rgb = rgb*rgb*(3.0-2.0*rgb);
    return c.z * lerp(1, rgb, c.y);
}

inline float poli_field(float2 st, float n){
    float a = atan2(st.x, st.y)+PI;
    float r = TWO_PI/n;

    return cos(floor(.5+a/r)*r-a) * length(st);
}

inline float2x2 rotate(float a){
    return float2x2(cos(a), -sin(a), sin(a), cos(a));
}

inline float2 rotate(float2 st, float a){
    return mul(float2x2(cos(a), -sin(a), sin(a), cos(a)), st);
}

inline float2 rotate_center(float2 st, float a){
    st -= .5;
    st = mul(float2x2(cos(a), -sin(a), sin(a), cos(a)), st);
    st += .5;
    return st;
}

inline float2x2 scale(float a){
    return float2x2(a, 0, 0, a);
}

inline float2 scale(float2 st, float a){
    return mul(float2x2(a, 0, 0, a), st);
}

inline float2 scale_center(float2 st, float a){
    st -= .5;
    st = mul(float2x2(a, 0, 0, a), st);
    st += .5;
    return st;
}

inline float step(float l, float g, float v){
    return step(l, v) * step(v, g);
}

inline float2 sample_displacement(sampler2D map, float2 uv, float2 speed, float magnitude){
    float2 d = tex2D(map, uv + speed * _Time.y) * 2 -1;
    return d * magnitude;
}

inline float inverse_lerp(float min, float max, float v){
    return saturate((v - min) / (max - min));
}
#endif