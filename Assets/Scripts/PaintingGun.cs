using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingGun : MonoBehaviour
{
    public LayerMask whatIsPainterable;
    public Transform tip, camera;
    public float maxdistance = 100f;
    public GameObject bullet;
    public GameObject paint;
    Rigidbody bulRig;
    Rigidbody ins_paintRig;

    Ray ray;
    RaycastHit hit;
    void Start()
    {
        Cursor.visible = false;
        bulRig = bullet.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            StartPainting();
        }
        StartCoroutine("Fade");
    }
    void StartPainting()
    {
        GameObject[] paints = new GameObject[100];
        if(Physics.Raycast(camera.position, camera.forward, out hit, maxdistance, whatIsPainterable))
        {
            Vector3 hitpos = hit.point;
           
            var heading = hitpos - tip.position;
            //GameObject ins_bullet = Instantiate(bullet, tip.position, Quaternion.Euler(0,0,0));
            //Rigidbody ins_bulRig = ins_bullet.GetComponent<Rigidbody>();
            //ins_bulRig.AddForce(heading.normalized * 10f, ForceMode.Impulse);
            Debug.DrawRay(camera.position, hitpos);

            GameObject ins_paint = Instantiate(paint, hitpos,Quaternion.Euler(hit.normal.x, hit.normal.y, hit.normal.z));
            ins_paintRig = ins_paint.GetComponent<Rigidbody>();

            //for (int i = 0; i < paints.Length; i++)
            //{
            //}
        }
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);
        ins_paintRig.useGravity = true;
    }
}
