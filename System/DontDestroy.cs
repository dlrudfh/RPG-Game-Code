using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DontDestroy 오브젝트를 위한 스크립트
public class DontDestroy : MonoBehaviour
{
    // 나중에 추가될 Dontdestroy오브젝트를 위해 배열 형태로 생성해 둠
    [SerializeField] GameObject[] objects;
    public void Awake()
    {
        for(int x = 0; x < objects.Length; x++)
        {
            // 입력된 오브젝트들에 dontdestroy속성 부여
            DontDestroyOnLoad(objects[x]);
        }
    }
}
