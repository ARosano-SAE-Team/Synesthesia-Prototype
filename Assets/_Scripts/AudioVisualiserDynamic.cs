using UnityEngine;

namespace _ScriptsDynamic
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioVisualiserDynamic : MonoBehaviour
    {
        AudioSource _audioSource;
        [SerializeField] public int sampleCountVar = 512;
        [SerializeField] private int bandCountVar = 8;
        [SerializeField] private float bufferDecreaseVar = 0.005f;
        [SerializeField] private float bufferDecreaseMultiplierVar = 1.2f;

        public static float[] _samples = new float[512];
        public static float[] _freqBand = new float[8];
        public static float[] _bandBuffer = new float[8];
        private float[] _bufferDecrease = new float[8];

        private int bandDifference = 0;


        private void Awake()
        {
            //Initialise Arrays
            _samples = new float[sampleCountVar];
            _freqBand = new float[bandCountVar];
            _bandBuffer = new float[bandCountVar];
            _bufferDecrease = new float[bandCountVar];
        }

        // Start is called before the first frame update
        void Start()
        {
        _audioSource = GetComponent<AudioSource> ();
        }

        // Update is called once per frame
        void Update()
        {
            GetSpectrumAudioSource();
            MakeFrequencyBands();
            BandBuffer();
        }

        private void GetSpectrumAudioSource()
        {
            _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
        }

        private void BandBuffer()
        {
            for (int g = 0; g < bandCountVar; ++g)
            {
                if (_freqBand[g] > _bandBuffer[g])
                {
                    _bandBuffer[g] = _freqBand[g];
                    _bufferDecrease[g] = bufferDecreaseVar;
                }

                if (_freqBand [g] < _bandBuffer[g])
                { 
                    _bandBuffer[g] -= _bufferDecrease [g];
                    _bufferDecrease[g] *= bufferDecreaseMultiplierVar;
                }
            }
        }

        private void MakeFrequencyBands()
        {
            /*
             *  22050 / 512 = 43 hertz per sample
         
             * 20 - 60 hertz
             * 60 - 250 hertz
             * 250 - 500 hertz
             * 500 - 2000 hertz
             * 2000 - 4000 hertz
             * 4000 - 6000 hertz
             * 6000 - 20000 hertz
         
             * 0 - 2 = 86 hertz
             * 1 - 4 = 172 hertz - 87-258
             * 2 - 8 = 344 hertz - 259-602
             * 3 - 16 = 688 hertz - 603-1290
             * 4 - 32 = 1376 hertz - 1291-2666
             * 5 - 64 = 2752 hertz - 2667-5718
             * 6 - 128 = 5504 hertz - 5419-10922
             * 7 - 256 = 11008 hertz - 10923-21930
             *
             * Total = 510 
             */

            bandDifference = 512 / sampleCountVar;
            print("Band Difference = " + bandDifference);
            int count = 0;

            for (int i = 0; i < bandCountVar; i++) 
            {
                float average = 0;
                int sampleCount = (int) (Mathf.Pow(2, i) * 2) / bandDifference;
                print("SampleCount = " + sampleCount);

                if (i == 7)
                {
                    //sampleCount += 2;
                }

                for (int j = 0; j < sampleCount; j++)
                {
                    print("count = " + count);
                    average += _samples[count] * (count + 1);
                    count++;
                }

                average /= count;
            
                _freqBand [i] = average * 10;
            }
        }
    }
}