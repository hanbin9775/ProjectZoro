using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour, IPooledObject
{
    public int damage = 20;
    public float bulletspeed = 35f;
    


    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        Vector3 force = transform.right*bulletspeed;

        GetComponent<Rigidbody>().velocity = force;

        //2초가 지나면 다시 Pool로 돌아감
        StartCoroutine(GetBacktoPool());
    }

    IEnumerator GetBacktoPool()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Monster")
        {
            //다시 Pool로 돌아감
            gameObject.SetActive(false);
        }
    }

}
