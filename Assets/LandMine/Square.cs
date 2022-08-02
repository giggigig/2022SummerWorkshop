using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Square : MonoBehaviour
{
    public int index;
    public int mineNum = 0;
    public bool isMine = false;
    public bool isClicked = false;
    public bool isFlaged = false;
    public GameObject mine;
    public GameObject Flag;

    public Square[] boundary = new Square[9];

    public TextMeshPro tm;

    enum state
    {
        Default,
        Clicked,
        Flag
    }

    public void Num(int a)
    {
        tm.gameObject.SetActive(true);
        tm.text = a.ToString();
    }
    public void SetMine()
    {
        isMine = true;
    }
}
