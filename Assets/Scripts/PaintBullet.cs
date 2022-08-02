using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBullet : MonoBehaviour
{
    public GameObject paint;
    Renderer ren;
    private void OnCollisionEnter(Collision collision)
    {
        ren = GetComponent<MeshRenderer>();
        if (!collision.transform.gameObject.CompareTag("Bullet"))
        {
            Vector3 c = collision.GetContact(0).point;
            //transform.position = c;
            //Instantiate(paint, c, Quaternion.Euler(c));
            //Destroy(this.gameObject);
            //StartCoroutine("AlphaFade");
        }
        
    }

    IEnumerator AlphaFade()
    {
        Color c = ren.material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            ren.material.color = c;
            yield return new WaitForSeconds(.1f);
        }
    }
}
