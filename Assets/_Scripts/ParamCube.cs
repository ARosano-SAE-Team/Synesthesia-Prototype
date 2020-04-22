using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    [SerializeField] private int _band;
    [ReadOnly] public float bandOutput;
    public float _startScale, _scaleMultiplier;
    public bool _useBuffer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if (_useBuffer)
        {
            bandOutput = Audioscript._bandBuffer[_band];
            transform.localScale = new Vector3 (transform.localScale.x, (bandOutput * _scaleMultiplier) + _startScale, transform.localScale.z);
        }
        if (!_useBuffer)
        {
            bandOutput = Audioscript._freqBand[_band];
            transform.localScale = new Vector3 (transform.localScale.x, (bandOutput * _scaleMultiplier) + _startScale, transform.localScale.z);
        }
    }
}
