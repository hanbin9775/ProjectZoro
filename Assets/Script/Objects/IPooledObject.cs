using UnityEngine;

// interface를 구현하는 모든 파생 클래스는 interface에 선언된 모든 메소드와 프로퍼티를 구현해야 한다.
// interface는 class와 달리 상속이 아니라 구현 관계를 사용한다.
// 여러 개의 interface를 구현하는 것은 가능하지만, 여러 개의 클래스에서 상속 받을 수 없다.
// 서로 관련이 없는 많은 다수의 클래스에 걸쳐 공톧된 기능을 정의하기에 유용하다.
public interface IPooledObject
{
    void OnObjectSpawn();
}
