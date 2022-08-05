using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public Texture im1;
    public Texture im2;
    public Texture GameoverIm;
    public LandMine landmine;
    Material material;
    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.SetTexture("_MainTex", im1);
    }
    void Update()
    {
        if (landmine.isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                material.SetTexture("_MainTex", im2);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                material.SetTexture("_MainTex", im1);
            }
        }
        else
        {
            material.SetTexture("_MainTex", GameoverIm);
        }

    }
}
