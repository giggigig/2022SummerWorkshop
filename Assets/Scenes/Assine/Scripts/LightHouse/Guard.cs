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
        viewAngle = spotlight.spotAngle; //Guard ���ù��� �ޱ� = ��������Ʈ �ޱ�
        originalSpotlightColor = spotlight.color; // �������� �÷� ��Ƴ���

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
        spotlight.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTimer / timeToSpotPlayer); //timer->.5�� ����������� ����/ timer = 0 ���� �÷�

        //spotlight�� timeToSpotPlayer�� �ӵ��� ������ ���� ���ϰ� �� 
        //player�� ���������� timer++/�ƴϸ� --
        //<Mathf.Clamp>

        //if (playerVisibleTimer >= timeToSpotPlayer)
        //{
        //    if (OnGuardHasSpottedPlayer != null)
        //    {
        //        OnGuardHasSpottedPlayer();
        //    }
        //}
    }
    public bool CanSeePlayer() //�÷��̾ ���ù��� ���� �������� true�� ��ȯ�ϴ� bool�Լ�
    {
        if (Vector3.Distance(Lightsource.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - Lightsource.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(Lightsource.forward, dirToPlayer); //�÷��̾�� ������� ���� :�ּҾޱ۹�ȯ
            if (angleBetweenGuardAndPlayer < viewAngle / 2f) // �÷��̾ ���� ���� ���� ������
            {
                if (!Physics.Linecast(Lightsource.position, player.position, viewMask)) // �÷��̾�� Guard���̿� ��ֹ��� ���ٸ�
                    return true;
            }
        }
        return false;
    }


    IEnumerator TurnToFace()
    {
        while (true)
        {
            Vector3 dirToLookTarget = (player.transform.position - transform.position).normalized; //�÷��̾�(Ÿ��)�� ���ϴ� ����
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
    //waypoint �̵��� �ڿ������� ȸ�� ���� ���� �ڷ�ƾ�Լ�
    //<mathf.Atan2>


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, .3f);
        Gizmos.color = Color.red;      
        //Gizmos.DrawRay(Lightsource.position, (player.position - Lightsource.position).normalized * viewDistance);
        Gizmos.DrawRay(Lightsource.position, Lightsource.forward* viewDistance);
    }
    //�����Ϳ��� Guard�� waypoint�� �þ߸� Ȯ���ϱ����� Gizmo����

}
