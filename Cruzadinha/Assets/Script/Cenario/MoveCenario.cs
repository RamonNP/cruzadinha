using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCenario : MonoBehaviour
{
    public float velicity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * velicity, 0);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
