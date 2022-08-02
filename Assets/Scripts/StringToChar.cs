using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StringToChar : MonoBehaviour
{
    public GameObject tm;

    public InputField inputText;
    TextMeshPro _text;

    float x;
    float z;
    Vector3 pos;

    Ray ray;
    RaycastHit hit;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        x = Random.Range(-25, 26);
        z = Random.Range(-25, 26);
        pos = new Vector3(x, 57f, z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                Vector3 hitpos = hit.transform.position;
                if (hit.transform.gameObject.CompareTag("TM"))
                {
                    TextMeshPro hitTM = hit.transform.gameObject.GetComponent<TextMeshPro>();
                    string s = hitTM.text;
                    Debug.Log(s);
                    char[] charArr = s.ToCharArray();
                    if (charArr.Length > 1)
                    {                    
                        for (int i = 0; i < charArr.Length; i++)
                        {
                            GameObject _charIns = Instantiate(tm, hitpos, Quaternion.Euler(hitpos));
                            _text = _charIns.GetComponent<TextMeshPro>();
                            _text.text = charArr[i].ToString();
                            _text.color = Color.red;
                        }
                    }
                    else
                    { 
                        GameObject _charIns = Instantiate(tm, hitpos, Quaternion.Euler(hitpos));
                        _text = _charIns.GetComponent<TextMeshPro>();
                        _text.text = (CharToint(charArr[0])).ToString();
                        _text.color = Color.blue;
                    }
                    Destroy(hit.collider.gameObject);
                }

            }
        }

        
    }
    public void TMGenrator()
    {
        GameObject instance = Instantiate(tm, pos, Quaternion.Euler(pos)) as GameObject;
        _text = instance.GetComponent<TextMeshPro>();
        _text.text = inputText.text;
    }
    public int CharToint(char a)
    {
        int i = (int)a;
        return i;
    }
}
