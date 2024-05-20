using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCAI : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject AI1;
    [SerializeField] private GameObject AI2;
    [SerializeField] private GameObject AI3;
    private GameObject selectedOne;
    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        
        // Invoke the WhoToChase method every 8 seconds, starting after 0 seconds
        InvokeRepeating("WhoToChase", 0f, 8f);
    }

    private void Update()
    {
        _agent.SetDestination(selectedOne.transform.position);
    }

    void WhoToChase()
    {
        int selectedEnemy = Random.Range(1, 5);
        Debug.Log(selectedEnemy);
        
        if (selectedEnemy == 1 && player.activeSelf)
        {
            selectedOne = player;
        }
        else if (selectedEnemy == 2 && AI1.activeSelf)
        {
            selectedOne = AI1;
        }
        else if (selectedEnemy == 3 && AI2.activeSelf)
        {
            selectedOne = AI2;
        }
        else if (selectedEnemy == 4 && AI3.activeSelf)
        {
            selectedOne = AI3;
        }
        else
        {
            WhoToChase();
        }
    }
}