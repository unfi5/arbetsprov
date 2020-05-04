using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    public Enemy Enemy;

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }

    public void EnemyHealth(int Damage, Transform Pos)
    {
        if(Enemy.Health > 0)
        {
            Enemy.EnemyHealth(Damage * 3, Pos);
        }
    }
}
