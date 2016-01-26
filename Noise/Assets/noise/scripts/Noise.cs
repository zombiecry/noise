using UnityEngine;
using System.Collections;
//noise instance
[ExecuteInEditMode]
public class Noise : MonoBehaviour {
    static public Noise    instance;
    private readonly int[] permutation = { 151,160,137,91,90,15,                 // Hash lookup table as defined by Ken Perlin.  This is a randomly
    131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,        // arranged array of all numbers from 0-255 inclusive.
    190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
    88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
    77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
    102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
    135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
    5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
    223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
    129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
    251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
    49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
    138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
    };
    private int[]          mRands;
    public  int            mRepeat;
    void Awake() 
    {
        instance = this;
        mRands = new int[512];
        for (int x = 0; x < 512; x++) 
        {
            mRands[x] = permutation[ x % 256 ];
        }
    }
    float PerlinNoise(float val) 
    {
        float x = val;
        if (mRepeat > 0) 
        {
            x = x % mRepeat;
        }
        int xi = (int)x & 255;                      //restrict to [0,255]
        float xf = x - (int)x;
        float u = Fade(xf);
        int randa = mRands[xi];
        int randb = mRands[IncrGridIndex(xi)];      //random number for selecting point gradient
        float ga = GradHash(randa, xf);
        float gb = GradHash(randb, xf - 1);
        float y = Mathf.Lerp(ga, gb, u);
        return y;
    }
    float PerlinNoise(Vector2 val) {
        float x = val.x;
        float y = val.y;
        if (mRepeat > 0) {
            x = x % mRepeat;
            y = y % mRepeat;
        }
        int xi = (int)x & 255;                      //restrict to [0,255]
        int yi = (int)y & 255;
        float xf = x - (int)x;
        float yf = y - (int)y;
        float u = Fade(xf);
        float v = Fade(yf);
        int randaa = mRands[mRands[xi]+yi];
        int randab = mRands[mRands[xi] + IncrGridIndex(yi)];
        int randbb = mRands[mRands[IncrGridIndex(xi)] + IncrGridIndex(yi)];
        int randba = mRands[mRands[IncrGridIndex(xi)] + yi];
		// ab ......... bb
		// .			.
		// .			.
		// .			.
		// aa ......... ba
        float gaa = GradHash(randaa, xf  , yf);
        float gab = GradHash(randab, xf  , yf-1);
		float gbb = GradHash(randbb, xf-1, yf-1);
        float gba = GradHash(randba, xf-1, yf);
        
        float ya = Mathf.Lerp(gaa, gba, u);
        float yb = Mathf.Lerp(gab, gbb, u);
        float yfinal = Mathf.Lerp(ya, yb, v); 
        return yfinal;
    }
    public float GetOctavesNoise(float v, int octaveNum,float persistence)
    {
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float total = 0.0f;
        float maxTotal = 0.0f;
        for (int i = 0; i < octaveNum; i++)
        {
            total += PerlinNoise(frequency * v) * amplitude;
            maxTotal += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        return total / maxTotal;
    }
    public float GetOctavesNoise(Vector2 v,int octaveNum,float persistence)
    {
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float total = 0.0f;
        float maxTotal = 0.0f;
        for (int i = 0; i < octaveNum; i++)
        {
            total += PerlinNoise(frequency * v) * amplitude;
            maxTotal += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }
        return total / maxTotal;
    }
    
    private float Fade(float t) 
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }
    private int IncrGridIndex(int i)
    {
        if (mRepeat > 0)
        {
            return (i + 1) % mRepeat;
        }
        return i + 1;
    }
	private float GradHash(int hash,float x)
    {
        switch (hash & 0x1)
        {
            case 0x0:
                return x;
            case 0x1:
                return -x;
            default:
                return 0;
        }
    }
    private float GradHash(int hash,float x,float y) {
        switch (hash & 0x11) {
            case 0x0:
                return x;
            case 0x1:
                return -x;
            case 0x10:
                return y;
            case 0x11:
                return -y;
            default:
                return 0.0f;
        }
    }
}
