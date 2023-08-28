using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

//�÷��̾ ���ϴ����� �� ������ �׺�Ž� �����
//�ʿ�Ӽ� : �ٸ� ������Ʈ, naveMeshSurface
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
