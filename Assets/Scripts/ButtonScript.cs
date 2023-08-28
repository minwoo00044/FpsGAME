using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

//플레이어가 버턴누르면 달 켜지고 네비매시 재생성
//필요속성 : 다리 오브젝트, naveMeshSurface
public class ButtonScript : MonoBehaviour
{
    public GameObject bridge;
    public NavMeshSurface meshSurface;
    // Start is called before the first frame update
    void Start()
    {
        bridge.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bridge.SetActive(true);
            meshSurface.BuildNavMesh();
        }
    }
}
