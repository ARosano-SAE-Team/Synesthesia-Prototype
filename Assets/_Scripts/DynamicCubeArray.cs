using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCubeArray : MonoBehaviour
{
    [ReadOnly, SerializeField] private int _band;
    [ReadOnly, SerializeField] private AudioVisualiserDynamic AVD;
    [ReadOnly, SerializeField] private bool useBuffer;
    [SerializeField] private float startScale, scaleMultiplier;

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

    //perform scaling, dependant on whether useBuffer was set or not.
    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AVD._bandBuffer[_band] * scaleMultiplier) + startScale, transform.localScale.z);
        }
        if (!useBuffer)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AVD._freqBand[_band] * scaleMultiplier) + startScale, transform.localScale.z);
        }
    }
}
