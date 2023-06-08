using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Move(Transform t)
	{
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		component.destination = t.position;
	}
}
