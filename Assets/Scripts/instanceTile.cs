using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instanceTile : MonoBehaviour
{
    public int life = 3;
    public float livetime =1f;
    public Color baseColor;
    bool isCheck;
    float timeCheck = 0;
    Material mymat;
    float colorSize = 0;

    // Start is called before the first frame update
    void Start()
    {
        mymat = GetComponent<MeshRenderer>().material;
        mymat.color = baseColor;
        colorSize = (float)1 / life;
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
        }
        if (!isCheck)
        {
            timeCheck += Time.deltaTime;
            if(timeCheck >= livetime)
            {
                timeCheck = 0;
                isCheck = true;
            }
        }
        mymat.color = new Color(mymat.color.r, mymat.color.g, mymat.color.b, colorSize*life);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Player")&& isCheck)
        {
            life -= 1;
            isCheck = false;

           // mymat.color = new Color(mymat.color.r, mymat.color.g, mymat.color.b, 1 - ((float)1 / life));
        }
    }
}
