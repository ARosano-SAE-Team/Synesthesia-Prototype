using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateDynamicCubesRadius : MonoBehaviour
{
    [SerializeField] private GameObject _sampleCubePrefab;
    GameObject[] _sampleCube = new GameObject[8];
    [SerializeField, ReadOnly] float instAngle = 0f;
    public float _maxScale;
    [SerializeField, ReadOnly] private int sampleAmount = 0;
    private AudioVisualiserDynamic AVD = null;

    private void Awake()
    {
        //Connect script to AudioVisualiserDynamic
        AVD = GameObject.FindGameObjectWithTag("AudioVisualiserController").GetComponent<AudioVisualiserDynamic>();

        //Init Arrays and variables
        sampleAmount = AVD.sampleCountVar;

        //re-constructs the sample-cube array
        _sampleCube = new GameObject[sampleAmount];
    }
    // Start is called before the first frame update
    void Start()
    {
        //Calculate Angle required for a complete circle
        print("instAngle = " + instAngle + " And sample amount = " + sampleAmount + " Before calculation");
        instAngle = 360f / (float)sampleAmount;
        print("instAngle = " + instAngle + " And sample amount = " + sampleAmount + " After calculation");

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
            //Rotate this transform towards where the new cube will go
            this.transform.eulerAngles = new Vector3(0, -instAngle * i, 0);
            //Move the sample cube 100 units infront of this
            _instanceSampleCube.transform.position = Vector3.forward * 100;
            //Tie sample cube into the array
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
                //Apply the vertical scaling
                _sampleCube[i].transform.localScale = new Vector3 (10, (AVD._samples [i] * _maxScale) + 2, 10);
            }
        }
    }
}
