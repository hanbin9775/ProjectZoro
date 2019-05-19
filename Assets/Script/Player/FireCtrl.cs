using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;

    ObjectPooler objectPooler;

    public Transform firepos;
    public Transform fireposhead;
    public Transform fireposdown;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                FireHead();
            }
            else if (Input.GetKey(KeyCode.DownArrow)&&!PlayerController.GetInstance().grounded)
            {
                FireDown();
            }
            else
                Fire();

        }
        
    }

    void FireHead()
    {
        objectPooler.SpawnFromPool("Bullet", fireposhead.position, fireposhead.rotation * Quaternion.Euler(0f, 0f, 90f));
    }

    void FireDown()
    {
        objectPooler.SpawnFromPool("Bullet", fireposdown.position, fireposdown.rotation * Quaternion.Euler(0f, 0f, -90f));
    }

    void Fire()
    {
        objectPooler.SpawnFromPool("Bullet", firepos.position, firepos.rotation);
    }

}
