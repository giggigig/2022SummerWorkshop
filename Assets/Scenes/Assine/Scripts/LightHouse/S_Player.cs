using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Player : MonoBehaviour
{
    public event System.Action OnReachedEndofLevel;
    public event System.Action OnLose;
    public float moveSpeed = 7;
    public float smoothMoveTime = .1f;
    public float turnSpeed = 8;
    [SerializeField]
    float MaxScale;
    [SerializeField]
    float MinScale;

    Vector3 scaleOrigin;
    float angle;
    float smoothInputMagnitude;
    float smoothMoveVelocity;
    Vector3 velocity;

    Rigidbody rigidbody;
    bool disabled =false;
    Guard guard;

    private void Start()
    {
        guard = FindObjectOfType<Guard>();
        scaleOrigin = transform.localScale;
        rigidbody = GetComponent<Rigidbody>(); //장애물 충돌 위한 rigidbody
        Guard.OnGuardHasSpottedPlayer += Disable;
    }

    void Update()
    {
        //Vector3 inputDirection = Vector3.zero;
        if (!disabled)
        {
            if (!guard.CanSeePlayer())
            {
                if(transform.localScale.x > MinScale)
                {
                    transform.localScale -= new Vector3(.1f, .1f, .1f) * smoothMoveTime;
                }
                else
                {
                    Disable();
                    if (OnReachedEndofLevel != null)
                    {
                        OnReachedEndofLevel();
                    }
                }
            }
            else
            {
                if (transform.localScale.x < MaxScale)
                {
                    transform.localScale += new Vector3(.1f, .1f, .1f) * smoothMoveTime;
                }
                else
                {
                    Disable();
                    if (OnLose != null)
                    {
                        OnLose();
                    }
                }
                
            }
        }

        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude; // ??
        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime); //각도 부드럽게 바꾸기

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg; // 입력한 방향으로 향하는 앵글각 구하기
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);//앵글의 중간값 구함 , 입력값(getAxis)이 0일때 간섭 방지

        velocity = transform.forward * moveSpeed * smoothInputMagnitude; //속도
    }

    void Disable()
    {
        disabled = true;
    }

    private void FixedUpdate()
    {
        rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Disable;
    }
}
