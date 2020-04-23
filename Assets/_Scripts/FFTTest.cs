using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FFTTest : MonoBehaviour
{
    const int SAMPLE_COUNT = 512;

    public FFTWindow window;
    public ParticleSystem pSys;

    [System.Serializable]
    public class FrequencyMap
    {
        public string name;
        public float frequency;
        public float width;
        [Range(0, 0.1f)]
        public float threshold;
        public Color color;
        public bool active;
        public float average;
    }

    public List<FrequencyMap> maps = new List<FrequencyMap>();

    AudioSource source;
    float[] spectrum = new float[SAMPLE_COUNT];
    float nextSampleTime = 0;
    float hertzPerBin;
    private void Awake()
    {
        Application.targetFrameRate = 60;

        source = GetComponent<AudioSource>();
        hertzPerBin = (float)AudioSettings.outputSampleRate / 2f / SAMPLE_COUNT;

        Debug.Log("OUTPUT SAMPLE RATE: " + AudioSettings.outputSampleRate);
    }

    int GetBinIndex(float freq)
    {
        return (int)(freq / hertzPerBin);
    }

    float GetNormalizedFreq(float freq)
    {
        return Mathf.InverseLerp(0, hertzPerBin * (SAMPLE_COUNT / 2), freq);
    }

    private void Update()
    {
        if (Time.time > nextSampleTime)
        {
            source.GetSpectrumData(spectrum, 1, window);
            nextSampleTime = Time.time + 0.05f;
        }

        foreach (var m in maps)
        {
            int start = GetBinIndex(m.frequency - (m.width / 2));
            int end = GetBinIndex(m.frequency + (m.width / 2));
            m.active = false;
            float val = 0;
            for (int i = start; i < end; i++)
            {
                if (i >= spectrum.Length)
                {
                    //This shouldn't happen...
                    break;
                }
                val += spectrum[i];
                if (spectrum[i] >= m.threshold)
                    m.active = true;
            }

            m.average = val / (end - start);
        }

        foreach (var m in maps)
        {
            if (m.active)
            {
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                emitParams.startColor = m.color;
                pSys.Emit(emitParams, 1);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Handles.color = Color.white;
        for (int i = 0; i < spectrum.Length / 2; i++)
        {
            Handles.DrawLine(new Vector3(i / (SAMPLE_COUNT / 2f), 0, 0), new Vector3(i / (SAMPLE_COUNT / 2f), spectrum[i], 0));
        }

        foreach (var m in maps)
        {
            float start = GetNormalizedFreq(m.frequency - (m.width / 2));
            float end = GetNormalizedFreq(m.frequency + (m.width / 2));
            Handles.color = m.color;
            Vector3 a = new Vector3(start, m.threshold, 0);
            Vector3 b = new Vector3(end, m.threshold, 0);
            Handles.DrawLine(a, b);
            Handles.Label(Vector3.Lerp(a, b, 0.5f), m.name + (m.active ? "!" : ""));
        }
    }
#endif

}
