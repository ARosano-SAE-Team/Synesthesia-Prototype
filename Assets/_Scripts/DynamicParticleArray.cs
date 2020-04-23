using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DynamicParticleArray : MonoBehaviour
{
    [ReadOnly, SerializeField] private int _band;
    [ReadOnly, SerializeField] private AudioVisualiserDynamic AVD;
    [ReadOnly, SerializeField] private bool useBuffer;
    [ReadOnly, SerializeField] private ParticleSystem PS;
    [ReadOnly, SerializeField] private ParticleSystem.EmissionModule em;

    [SerializeField] private float startScale, scaleMultiplier;

    [ReadOnly] public float bandOutput;

    //called by array instantiator
    public void SetVariables(int band, float _startScale, float _scaleMultiplier, bool UseBuffer, AudioVisualiserDynamic AudioVisualusier)
    {
        _band = band;
        //print("Band Set! - " + band);
        AVD = AudioVisualusier;
        //print("AVD Set! - " + AVD);
        useBuffer = UseBuffer;
        //print("useBuffer Set! - " + useBuffer);
        startScale = _startScale;
        //print("startScale Set! - " + startScale);
        scaleMultiplier = _scaleMultiplier;
        //print("scaleMultiplier Set! - " + scaleMultiplier);
    }

    void OnEnable()
    {
        PS = gameObject.GetComponent<ParticleSystem>();
        em = PS.emission;
    }

    //perform scaling, dependant on whether useBuffer was set or not.
    void Update()
    {
        if (useBuffer)
        {
            bandOutput = AVD._bandBuffer[_band];
        }
        if (!useBuffer)
        {
            bandOutput = AVD._freqBand[_band];
        }

        em.rateOverTime = bandOutput * 5;

    }
}
