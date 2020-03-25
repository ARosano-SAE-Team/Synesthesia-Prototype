using System.Collections;
using System.Collections.Generic;
using _ScriptsDynamic;
using UnityEngine;

public class InstantiateDynamicCubes : MonoBehaviour
{
    public GameObject _sampleCubePrefab;
    GameObject[] _sampleCube = new GameObject[512];
    public float _maxScale;
    private int sampleAmount = 0;
    private AudioVisualiserDynamic AVD = null;

    private void Awake()
    {
        //Connect script to AudioVisualiserDynamic
        AVD = GameObject.FindGameObjectWithTag("AudioVisualiserController").GetComponent<AudioVisualiserDynamic>();

        //Init Arrays and variables
        sampleAmount = AVD.sampleCountVar;
        _sampleCube = new GameObject[sampleAmount];
    }
    // Start is called before the first frame update
    void Start()
    {
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
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            _instanceSampleCube.transform.position = Vector3.forward * 100;
            _sampleCube[i] = _instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < sampleAmount; i++)
        {
            if (_sampleCube != null)
            {
                _sampleCube[i].transform.localScale = new Vector3 (10, (AudioVisualiserDynamic._samples [i] * _maxScale) + 2, 10);
            }
        }
    }
}
