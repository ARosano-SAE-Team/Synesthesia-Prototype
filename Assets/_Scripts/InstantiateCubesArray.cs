using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCubesArray : MonoBehaviour
{
    [Header("Object to array and set band - must have DynamicCubeArray script")]
    [SerializeField] private GameObject _sampleCubePrefab;
    [Header("Connect Master spectrumSource here")]
    [SerializeField] private AudioVisualiserDynamic AVD = null;
    [Header("Distance between center of cubes")]
    [SerializeField] private float cubeDistance = 1;
    [Header("Cube Variables")]
    [SerializeField] private float startScale = 2;
    [SerializeField] private float scalingMultiplier = 10;
    [SerializeField] private bool useBuffer = false;

    private GameObject[] _sampleCube = new GameObject[512];
    private int sampleAmount = 0;
    private int instAngle = 0;

    private void Awake()
    {
        //Init Arrays and variables
        sampleAmount = AVD.bandCountVar;

        //re-constructs the sample-cube array
        _sampleCube = new GameObject[sampleAmount];
    }
    // Start is called before the first frame update
    void Start()
    {
        //Calculate Angle required for a complete circle
        instAngle = 360 / sampleAmount;

        for (int i = 0; i < sampleAmount; i++)
        {
            //Instantiate the Cube
            GameObject _instanceSampleCube = Instantiate(_sampleCubePrefab);
            //Reset the position to this transform
            _instanceSampleCube.transform.position = this.transform.position;
            //Parent this transform to make it neater in the hiearchy
            _instanceSampleCube.transform.parent = this.transform;
            //Name the Cube so its neater in the hiearchy
            _instanceSampleCube.name = "sampleCube" + i;
            //Check if component exists, then set the band on the cube, set the master AVD and set useBuffer
            if (_instanceSampleCube.GetComponent<DynamicCubeArray>() != null)
            {
                _instanceSampleCube.GetComponent<DynamicCubeArray>().SetVariables(i, startScale, scalingMultiplier, useBuffer, AVD);
            } else
            {
                Debug.LogError("ParamCube Sript not found!");
            }
            //Move the cube forward, multiplied by its number and distance variable
            _instanceSampleCube.transform.position = Vector3.forward * i * cubeDistance;
            //Tie sample cube into the array
            _sampleCube[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Don't run, this is for unbanded pure sample scaling - we are using the bands in AVD
        //visualisation();
    }

    //Apply Audio Visalisation to child cubes
    private void visualisation()
    {
        for (int i = 0; i < sampleAmount; i++)
        {
            if (_sampleCube != null)
            {
                //Apply the vertical scaling
                _sampleCube[i].transform.localScale = new Vector3(10, (AVD._samples[i] * scalingMultiplier) + 2, 10);
            }
        }
    }
}
