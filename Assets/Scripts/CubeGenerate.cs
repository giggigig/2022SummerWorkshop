using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerate : MonoBehaviour
{
    public GameObject cube;
    public Transform t;
    Ray _ray;
    RaycastHit _raycastHit;
    Camera _camera;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            ShootRay();
           
        }
        if(timer > 3)
        {
            GameObject InsCube = Instantiate(cube, t);
            timer = 0;
        } 
    }

    private void ShootRay()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_ray, out _raycastHit))
        {
            if (!_raycastHit.transform.gameObject.CompareTag("floor"))
            {
                Vector3 hitPosition = _raycastHit.point;
                GameObject hitObj = _raycastHit.transform.gameObject;
                GameObject InsCube = Instantiate(cube, hitPosition, Quaternion.Euler(hitPosition));
                GameObject InsCube2 = Instantiate(cube, hitPosition, Quaternion.Euler(hitPosition));
                InsCube.transform.localScale = hitObj.transform.localScale /2;
                InsCube2.transform.localScale = hitObj.transform.localScale / 2;
                Destroy(hitObj);
            }
        }
    }
}
