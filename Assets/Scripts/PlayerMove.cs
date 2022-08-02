using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 3f;
    public int maxJumpCount = 2;
    public float mSpeed = 10f;
    Rigidbody rigid;
    float mouseX, mouseY;
    int jumpcount =0;
    bool jumpEnable = true;
    float sensitivity;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Look();
        Move();
        Jump();
    }
    private void Move()
    {
        transform.localPosition += (transform.right * Input.GetAxis("Horizontal")
            + transform.forward * Input.GetAxis("Vertical")) * Time.deltaTime * speed;
    }
    void Look()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");

        Camera.main.transform.rotation = Quaternion.Euler(-mouseY * mSpeed, mouseX * mSpeed, 0) ;
        transform.rotation = new Quaternion(0, Camera.main.transform.rotation.y , 0, Camera.main.transform.rotation.w);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpEnable)
        {
            jumpcount++;
            if (jumpcount >= maxJumpCount)
            {
                jumpEnable = false;
                jumpcount = 0;
            }
            rigid.AddForce(transform.up * speed, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            jumpEnable = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        jumpEnable = false;
    }
}
