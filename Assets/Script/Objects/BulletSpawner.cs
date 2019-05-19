using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void FIxedUpdate()
    {
       objectPooler.SpawnFromPool("Bullet", transform.position, Quaternion.identity);
    }
}
