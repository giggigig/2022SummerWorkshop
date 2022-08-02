using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HalfCylinder : MonoBehaviour
{
    public float speed = 1;
    public TextMeshPro _numText;
    public TextMeshPro _pressText;
    float _time = 0;
    Camera _camera;
    bool isStop;
    float runingtime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _camera = FindObjectOfType<Camera>().GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        string s = "F";
        int camzoom = 1;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isStop)
            {
                isStop = false;
            }
            isStop = true;
        }

        if (!isStop)
        {
            _time += Time.deltaTime * speed;
            if (_time >= 1 || _time <= 0)
            {
                speed *= -1f;
            }
            Wawa();
        }
        else
        {
            float check = Mathf.Abs( 0.5f - _time);
            if(check>0.1f)
            {
                BooOrGreat(1);
            }
            else if(check < 0.1f)
            {
                BooOrGreat(2);
            }
            //마우스 스크롤
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 10f;
            _camera.fieldOfView += scroll;


            if (_camera.fieldOfView >= 60)
            {
                camzoom = 1;
            }
            else if (_camera.fieldOfView < 50)
            {
                camzoom = (int)(7 - (_camera.fieldOfView / 10));
                BooOrGreat(3);
            }

        }
        transform.localScale = new Vector3(1, _time, 1);
        transform.position = new Vector3(0, _time, 0);

        _numText.transform.position = new Vector3(_numText.transform.position.x, _time, _numText.transform.position.z);

        string v = camzoom.ToString();
        _numText.text = _time.ToString(s + v);

    }

    void Wawa()
    {
        runingtime = Time.deltaTime * 10f;
        float s = Mathf.Sin(runingtime)*10f;
        Debug.Log(s);
         _pressText.transform.localScale = new Vector3(s, s, s);
    }
    void BooOrGreat(int i)
    {
        if (i == 1)
        {
            _pressText.text = "Boo~~~~~ :(";
        }
        else if( i == 2)
        {
            _pressText.text = "Great!";
        }
        else if (i == 3)
        {
            _pressText.text = "you dummy~~";
        }
    }
}
