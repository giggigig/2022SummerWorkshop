using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject[] Lights;
    public int ColorNum =0;
    public float SwitchTime = 3f;
    //float curTime = 0;

    //private State state = State.Green;
    //public enum State
    //{
    //    Green,
    //    Yellow,
    //    Red
    //}
    Material[] materials;
    private void Awake()
    {
        materials = new Material[Lights.Length];
        try
        {
            for (int i = 0; i < Lights.Length; i++)
            {
                materials[i] = Lights[i].GetComponent<MeshRenderer>().material;
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("NULL MATERIAL");
        }
    }
    void Start()
    {
        StartCoroutine(SwitchColor());
    }

    void Update()
    {
        //curTime += Time.deltaTime;
        //if(curTime > SwitchTime)
        //{
        //    if (ColorNum > 2)
        //    {
        //        ColorNum = 0;
        //    }
        //    ColorNum++;
        //    curTime = 0;
        //}
    }
    IEnumerator SwitchColor()
    {
        while (true)
        {
            ColorNum = 0;
            LightColorOn(materials[ColorNum], Color.green);
            LightColorOff(materials[ColorNum+1]);
            LightColorOff(materials[ColorNum + 2]);
            yield return new WaitForSeconds(SwitchTime);

            ColorNum = 1;
            LightColorOn(materials[ColorNum], Color.yellow);
            LightColorOff(materials[ColorNum-1]);

            yield return new WaitForSeconds(SwitchTime);
            ColorNum = 2;
            LightColorOn(materials[ColorNum], Color.red);
            LightColorOff(materials[ColorNum - 1]);

            yield return new WaitForSeconds(SwitchTime);
        }
    }

    void LightColorOn(Material m, Color c)
    {
        //m.color = c;
        m.SetColor("_EmissionColor",c);
    }
    void LightColorOff(Material m)
    {
        m.color = Color.black;
        m.SetColor("_EmissionColor", Color.black);
    }
}
