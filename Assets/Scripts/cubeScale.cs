using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cubeScale : MonoBehaviour
{
    public InputField speedInput;
    public int speed = 1;

    float s = 0;
    int pm = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(speedInput.text == "")
        {
            return;
        }

        speed = int.Parse(speedInput.text);

        s += Time.deltaTime * pm * speed;
        if (s >= 1 || s<=0)
        {
            pm *= -1;
        }
        else if (s>1)
        {
            pm = -1;
        }
        else if (s < 0)
        {
            pm = 1;
        }
        transform.localScale = new Vector3(s,s,s);
    }
}
