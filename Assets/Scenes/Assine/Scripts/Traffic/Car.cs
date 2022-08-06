using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float smoothMoveTime = .1f;
    public float viewDistance ;
    public float minViewDistance =.2f;

    [SerializeField]
    float curDistance;


    [SerializeField]
    Signal signal = Signal.Green;

    GameObject[] TLob;
    TrafficLight[] TL;
    Rigidbody rigid;

    float inputMagnitude;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    float deceleration;
    Vector3 velocity;
    bool isStoped;

    enum Signal
    {
        Green,
        Yellow,
        Red
    }
    // Start is called before the first frame update
    void Start()
    {

        TLob = GameObject.FindGameObjectsWithTag("TL");
        TL = new TrafficLight[TLob.Length];
        for (int i = 0; i < TLob.Length; i++)
        {
            TL[i] = TLob[i].GetComponent<TrafficLight>();
        }

        rigid = GetComponent<Rigidbody>();
        StartCoroutine(CarAccelerator());
        inputMagnitude = 1;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = transform.forward * moveSpeed* smoothInputMagnitude;

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime* deceleration); //각도 부드럽게 바꾸기

        CheckTrafficLight();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + velocity * Time.deltaTime);
    }
    IEnumerator CarAccelerator()
    {
        while (true)
        {
            switch (signal)
            {
                case Signal.Green:
                    if (isStoped)
                    {
                        inputMagnitude += 1;
                        isStoped = false;
                        deceleration = 1;
                    }
                    yield return new WaitForSeconds(0.1f);
                    break;

                case Signal.Red:
                    isStoped = true;
                    inputMagnitude = 0;
                    //deceleration = curDistance / viewDistance; --> 감속구간 collider, damp 마지막 값 time 에 col 사이즈 재서 
                    yield return new WaitForSeconds(0.1f); 
                    break;

                case Signal.Yellow:
                    inputMagnitude += .5f;
                    yield return new WaitForSeconds(0.1f);
                    break;

            }
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
    void CheckTrafficLight()
    {
        for (int i = 0; i < TLob.Length; i++)
        {
            if (TL[i]!= null)
            {
                curDistance = Vector3.Distance(transform.position, TL[i].transform.position);
                if (curDistance < viewDistance)
                {
                    if (TL[i].ColorNum == 0)
                    {
                        signal = Signal.Green;
                    }
                    else if (TL[i].ColorNum == 1)
                    {
                        signal = Signal.Yellow;
                    }
                    else if (TL[i].ColorNum == 2)
                    {
                        signal = Signal.Red;
                    }

                    if(curDistance > minViewDistance)
                    {
                        continue;
                    }
                }
            }
        } 
    }
}
