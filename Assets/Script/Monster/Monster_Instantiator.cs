using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Instantiator : MonoBehaviour
{
    //SIngleton
    private static Monster_Instantiator instance;

    public static Monster_Instantiator GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Monster_Instantiator>();
        }
        return instance;
    }

    public Transform[] Mon_pos;

    public GameObject[] Mon_prefab;

    public int ins_cnt;

    public GameObject[] check_index;
    

    // Start is called before the first frame update
    void Awake()
    {
        //Singleton Check
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (ins_cnt < 3 && ins_cnt>=0)
        {
            int temp = Random.Range(0, check_index.Length);
            if (check_index[temp] == null)
            {
                check_index[temp] = Instantiate(Mon_prefab[0], Mon_pos[temp].position, Mon_pos[temp].rotation);
                ins_cnt += 1;
            }
        }
    }
}
