using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UItest : MonoBehaviour
{
    public GameObject cube;
    public InputField myfield;
    public Text myText;

    int num = 0;
    int totalCube = 0;
    private void Start()
    {
        myfield.text = "0";
    }
    // Update is called once per frame
    void Update()
    {
       
        string s = myfield.text;
        try
        {
            num = int.Parse(s);
            myText.text = totalCube.ToString();
        }
        catch (System.Exception)
        {
            throw;
        }
    }
    public void MakeCube()
    {
        //cube ¸¸µé±â
        for (int i = 0; i < num; i++)
        {
            Instantiate(cube, transform.position, Quaternion.identity);
        }
        totalCube += num;
    }
}
