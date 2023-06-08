using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public genmaze mazemanager;

	private genmaze ma;

	public GameObject Player;

	private GameObject player;

	public GameObject Enemy;

	private GameObject[] enemy = new GameObject[10];

	private Transform t;

	private GameObject maa;

	private void Start()
	{
		player = Object.Instantiate(Player);
		player.transform.position = new Vector3(14f, 3f, 4f);
		for (int i = 0; i < 10; i++)
		{
			enemy[i] = Object.Instantiate(Enemy);
			enemy[i].transform.position = new Vector3(16f, 6f, 4f);
		}
	}

	private void Update()
	{
		t = player.transform;
		for (int i = 0; i < 10; i++)
		{
			enemy[i].GetComponent<EnemyScript>().Move(t);
		}
	}
}
