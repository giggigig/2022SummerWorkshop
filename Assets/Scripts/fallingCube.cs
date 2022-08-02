using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingCube : MonoBehaviour
{
    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 3f)
        {
            Destroy(gameObject);
            timer = 0;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            Destroy(gameObject);
            var floorRenderer = collision.gameObject.GetComponent<Renderer>();
            floorRenderer.material.SetColor("_Color", Color.red);
        }

    }
}
