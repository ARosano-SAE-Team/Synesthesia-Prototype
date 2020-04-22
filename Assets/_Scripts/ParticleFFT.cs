using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(AudioSource), typeof(ParticleSystem))]
    public class ParticleFFT : MonoBehaviour
    {
        public FFTWindow window;
        AudioSource src;
        ParticleSystem pSys;

        //something more useable than a raw buffer
        [Range(0, 1)]
        public float[] downSampled = new float[8];

        //FFT buffer
        float[] samples = new float[64];

        //which index of the downsampled buffer to associate with which param
        [Range(0, 7)] public int redIndex = 0;
        [Range(0, 7)] public int greenIndex = 0;
        [Range(0, 7)] public int blueIndex = 0;
        [Range(0, 7)] public int sizeIndex = 0;

        void Start()
        {
            //cache soe referencess
            src = GetComponent<AudioSource>();
            pSys = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            //get FFT data
            src.GetSpectrumData(samples, 0, window);

            //put FFT data into a useful mathematical range
            for (int i = 0; i < samples.Length; i++)
                samples[i] = Mathf.Pow(samples[i], 0.1f);

            //downsample FFT data
            float accumulator = 0;
            int downSampledIndex = 0;
            for (int i = 1; i < samples.Length + 1; i++)
            {
                accumulator += samples[i - 1];
                if (i % 8 == 0)
                {
                    downSampled[downSampledIndex] = accumulator / 8f;
                    accumulator = 0;
                    downSampledIndex++;
                }
            }

            //get the ParticleSystem Main Module
            var main = pSys.main;
            //get the startcolor struct 
            ParticleSystem.MinMaxGradient startColor = main.startColor;
            //Now we're back in normal color land
            Color color = startColor.color;
            color.r = downSampled[redIndex];
            color.g = downSampled[greenIndex];
            color.b = downSampled[blueIndex];
            //reassign the color
            startColor.color = color;
            //Gradient goes here if you dont want to use flat color
            //startColor.colorMin = ?????
            //startColor.colorMax = ?????
            //reassign the gradient
            main.startColor = startColor;
            main.startSizeMultiplier = downSampled[sizeIndex];

        }

    }
}
