using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadBody : MonoBehaviour
{
    private bool _wait = false;

    private void Update()
    {
        if(!_wait)
        {
            Collider[] enemyDetection = Physics.OverlapSphere(gameObject.transform.position, 10);
            int i = 0;
            while (i < enemyDetection.Length)
            {

                if (enemyDetection[i].tag == "Enemy" && enemyDetection[i].GetComponent<Enemy>() != null)
                {
                    if (enemyDetection[i].GetComponent<Enemy>().BodyNotice == false && enemyDetection[i].GetComponent<Enemy>().seeTarget == false)
                    {
                        enemyDetection[i].GetComponent<Enemy>().NoticeBody(this.transform);
                    }
                }

                i++;
            }
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        _wait = true;   
        yield return new WaitForSeconds(1);
        _wait = false;
    }
}
