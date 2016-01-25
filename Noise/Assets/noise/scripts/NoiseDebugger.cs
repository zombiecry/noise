using UnityEngine;
using System.Collections;
using System.IO;
public class NoiseDebugger : MonoBehaviour {
    public float mNoiseLength = 10.0f;
    public float mSampleNum = 100;
    public float mDisplayLength = 10.0f;

    public int mOctaveNum = 1;
    public float mPersistence = 1.0f;

    public int      mWidth;
    public int      mHeight;
    public float    mNoiseWidth;
    public float    mNoiseHeight;

	// Use this for initialization
	void Start () {
	
	}
	public void GenerateTexture() {
        int width = mWidth;
        int height = mHeight;
        float noiseWidth = mNoiseWidth;
        float noiseHeight = mNoiseHeight;
        Texture2D texture = new Texture2D(width, height);
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                Vector2 v = new Vector2(i * noiseWidth / texture.width, j * noiseHeight / texture.height);
                float y = Noise.instance.GetOctavesNoise(v, 1, 1);
                y = (y + 1) * 0.5f;
                Color color = new Color(y, y, y);
                texture.SetPixel(i, j, color);
            }
        }
        texture.Apply();
        byte[] bin = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../GeneratedNoise.png", bin);
        Object.Destroy(texture);
    }
	// Update is called once per frame
	void Update () {
	    if (Noise.instance == null)
        {
            return;
        }
        Vector3 lastPos = Vector3.zero;
        for (int i = 0; i <= mSampleNum; i++)
        {
            float y = Noise.instance.GetOctavesNoise(i * (mNoiseLength / mSampleNum), mOctaveNum, mPersistence);
            Vector3 curPos = new Vector3(i * (mDisplayLength / mSampleNum), y, 0);
            if (i == 0)
            {
                lastPos = curPos;
                continue;
            }
            Debug.DrawLine(curPos, lastPos);
            lastPos = curPos;
        }
	}
}
