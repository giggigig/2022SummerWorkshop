using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;

    public float waitTime = .3f;
    public float turnSpeed = 9;
    public float timeToSpotPlayer = .5f;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

    float viewAngle;
    float playerVisibleTimer;

    public Transform Lightsource;
    public Transform player;
    Color originalSpotlightColor;

    void Start()
    {
        //Lightsource = transform.GetChild(2).transform;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle; //Guard 감시범위 앵글 = 스폿라이트 앵글
        originalSpotlightColor = spotlight.color; // 오리지널 컬러 담아놓기

        StartCoroutine(TurnToFace());

    }
    private void Update()
    {
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotlight.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTimer / timeToSpotPlayer); //timer->.5에 가까워질수록 레드/ timer = 0 원래 컬러

        //spotlight를 timeToSpotPlayer의 속도로 서서히 색이 변하게 함 
        //player를 볼수있으면 timer++/아니면 --
        //<Mathf.Clamp>

        //if (playerVisibleTimer >= timeToSpotPlayer)
        //{
        //    if (OnGuardHasSpottedPlayer != null)
        //    {
        //        OnGuardHasSpottedPlayer();
        //    }
        //}
    }
    public bool CanSeePlayer() //플레이어가 감시범위 내에 들어왔을때 true를 반환하는 bool함수
    {
        if (Vector3.Distance(Lightsource.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - Lightsource.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(Lightsource.forward, dirToPlayer); //플레이어와 가드사이 각도 :최소앵글반환
            if (angleBetweenGuardAndPlayer < viewAngle / 2f) // 플레이어가 감시 범위 내에 들어왔음
            {
                if (!Physics.Linecast(Lightsource.position, player.position, viewMask)) // 플레이어와 Guard사이에 장애물이 없다면
                    return true;
            }
        }
        return false;
    }


    IEnumerator TurnToFace()
    {
        while (true)
        {
            Vector3 dirToLookTarget = (player.transform.position - transform.position).normalized; //플레이어(타켓)을 향하는 벡터
            float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
            {
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }

    }
    //waypoint 이동시 자연스러운 회전 구현 위한 코루틴함수
    //<mathf.Atan2>


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, .3f);
        Gizmos.color = Color.red;      
        //Gizmos.DrawRay(Lightsource.position, (player.position - Lightsource.position).normalized * viewDistance);
        Gizmos.DrawRay(Lightsource.position, Lightsource.forward* viewDistance);
    }
    //에디터에서 Guard의 waypoint와 시야를 확인하기위한 Gizmo생성

}
