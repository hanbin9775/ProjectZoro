using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    public float _amount = 0.1f;
    public float _duration = 0.5f;

    Vector3 originPos;

    void Start()
    {
        instance = this;
        
    }
    
    public void shakefunc()
    {
        Debug.Log("Shake!");
        StartCoroutine(Shake());
    }

    private void Update()
    {
        originPos = transform.localPosition;
    }

    public IEnumerator Shake()
    {
        float timer = 0;
        while (timer <= _duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;

            transform.position = new Vector3(PlayerController.GetInstance().myTrans.position.x,transform.position.y,transform.position.z);
            yield return null;
            
        }
        transform.localPosition = originPos;
    }
}
