using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_A_bullet : MonoBehaviour, IPooledObject
{
    public int damage = 10;

    public void OnObjectSpawn()
    {
        //2초가 지나면 다시 Pool로 돌아감
        StartCoroutine(GetBacktoPool());
    }

    IEnumerator GetBacktoPool()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerHealth.GetInstance().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
