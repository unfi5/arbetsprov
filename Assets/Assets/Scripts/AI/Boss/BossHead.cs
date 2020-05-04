using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    private Boss _boss;

    void Awake()
    {
        _boss = GetComponentInParent<Boss>();
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }

    public void EnemyHealth(int Damage, Transform Pos)
    {

        if (_boss.Health > 0)
        {
            _boss.EnemyHealth(Damage * 2);
        }
    }
}
