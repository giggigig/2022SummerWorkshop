using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //float ho = Input.GetAxis("Horizontal");
        //float ve = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3( 1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, 1);
        }
    }
}
