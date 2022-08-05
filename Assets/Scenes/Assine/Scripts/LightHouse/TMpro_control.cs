using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TMpro_control : MonoBehaviour
{

    TextMeshProUGUI tm;
    bool enable =false;
    float space;
    private void Start()
    {
        tm = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (isActiveAndEnabled)
        {
            space += Time.deltaTime*5;
            if (space < 10)
            {
                tm.characterSpacing = space;
            }
        }
    }
}
