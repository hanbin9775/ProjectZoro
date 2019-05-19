using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //주인공 따라다리는 카메라 코드

    private static CameraFollow instance;
    public float FollowSpeed = 2f;

    float camera_y_position;

    public static CameraFollow GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<CameraFollow>();
        }
        return instance;
    }

    public Transform playertrans; //target

    public bool CameraCanMove = true;

    Transform cameratrans;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {               // 인스턴스가 있는데 
            if (instance != this)
            {       // 그게 내 자신이 아니라면
                Destroy(gameObject);    // 내 자신을 파괴한다.	
            }
        }
        cameratrans = GetComponent<Transform>();
        camera_y_position = cameratrans.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetPosition = playertrans.TransformPoint(new Vector3(0, 2.1f, -15));
        if (CameraCanMove)
        {
            Vector3 newPoint = playertrans.position;
            newPoint.y = playertrans.position.y + 3f;
            newPoint.z = -10;
            //cameratrans.position = Vector3.Slerp(cameratrans.position, newPoint, FollowSpeed * Time.deltaTime);
            cameratrans.position = new Vector3(playertrans.position.x, camera_y_position, cameratrans.position.z);
        }
        else if (!CameraCanMove)
        {
            Vector3 newPoint = playertrans.position;
            newPoint.y = camera_y_position;
            newPoint.x = cameratrans.position.x;
            newPoint.z = -10;
            cameratrans.position = Vector3.Slerp(cameratrans.position, newPoint, FollowSpeed * Time.deltaTime);
        }
    }

}
