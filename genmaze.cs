using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class genmaze : MonoBehaviour
{
	public Material mat;

	[Range(3f, 200f)]
	public int xSize;

	[Range(3f, 200f)]
	public int ySize;

	[Range(1f, 50f)]
	public int sizeWall;

	public GameObject wall;

	private Vector3 initialPos;

	private GameObject[] walls;

	private float middle;

	private GameObject Maze;

	public List<Cell> cells;

	private int eastLimited;

	private int southLimited;

	private int northLimited;

	private int westLimited;

	private int currentRow;

	public List<Cell> lastCellVisited;

	private int indexCell;

	private int currentI;

	private Cell currentCell;

	private List<Cell> neighboringCell;

	private GameObject parent;

	private int range;

	public static genmaze instanceid;

	private void Awake()
	{
		if (instanceid == null)
		{
			instanceid = this;
		}
		else if (instanceid != this)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		Create();
	}

	private void Update()
	{
	}

	private void Create()
	{
		middle = (float)sizeWall / 2f;
		initialPos = Vector3.zero;
		base.transform.position = initialPos;
		lastCellVisited = new List<Cell>();
		neighboringCell = new List<Cell>();
		cells = new List<Cell>();
		currentI = 0;
		range = sizeWall;
		wall.transform.localScale = new Vector3((float)sizeWall / 10f, sizeWall, sizeWall);
		createWalls();
		createCell();
	}

	private void createWalls()
	{
		Maze = new GameObject("Maze");
		for (int i = 0; i < ySize; i++)
		{
			for (int j = 0; j <= xSize; j++)
			{
				GameObject gameObject = Object.Instantiate(wall, new Vector3(initialPos.x + (float)(j * sizeWall), 1f, initialPos.z + (float)(i * sizeWall)), Quaternion.identity);
				gameObject.transform.parent = Maze.transform;
				gameObject.GetComponent<Renderer>().material = mat;
			}
		}
		for (int k = 0; k <= ySize; k++)
		{
			for (int l = 0; l < xSize; l++)
			{
				GameObject gameObject2 = Object.Instantiate(wall, new Vector3(initialPos.x + (float)(l * sizeWall) + middle, 1f, initialPos.z + (float)(k * sizeWall) - middle), Quaternion.Euler(0f, 90f, 0f));
				gameObject2.transform.parent = Maze.transform;
				gameObject2.AddComponent<BoxCollider>();
				gameObject2.GetComponent<Renderer>().material = mat;
			}
		}
	}

	private void createCell()
	{
		walls = new GameObject[Maze.transform.childCount];
		int num = (xSize + 1) * ySize;
		int num2 = (xSize + 1) * ySize + xSize;
		for (int i = 0; i < Maze.transform.childCount; i++)
		{
			walls[i] = Maze.transform.GetChild(i).gameObject;
		}
		for (int j = 0; j < xSize * ySize; j++)
		{
			cells.Add(new Cell());
			currentRow = j / xSize;
			cells[j].south = walls[j + num];
			cells[j].north = walls[j + num2];
			cells[j].east = walls[currentRow + j + 1];
			cells[j].west = walls[currentRow + j];
			cells[j].current = j;
		}
		mazeCreation(cells);
	}

	private void AddNears(Cell currentCell)
	{
		int num = currentCell.current / xSize + 1;
		int num2 = currentCell.current % xSize;
		int num3 = num2;
		int num4 = xSize - 1;
		int num5 = currentCell.current / xSize;
		if (num2 != xSize - 1 && !cells[currentCell.current + 1].isVisited)
		{
			neighboringCell.Add(cells[currentCell.current + 1]);
		}
		if (num3 != 0 && !cells[currentCell.current - 1].isVisited)
		{
			neighboringCell.Add(cells[currentCell.current - 1]);
		}
		if (currentCell.current > num4 && !cells[currentCell.current - xSize].isVisited)
		{
			neighboringCell.Add(cells[currentCell.current - xSize]);
		}
		if (num5 != ySize - 1 && !cells[currentCell.current + xSize].isVisited)
		{
			neighboringCell.Add(cells[currentCell.current + xSize]);
		}
	}

	private void breakWall(Cell curr, Cell choose)
	{
		if (choose.current == curr.current + 1)
		{
			Object.Destroy(curr.east);
			currentCell = cells[currentCell.current + 1];
		}
		if (choose.current == curr.current - 1)
		{
			Object.Destroy(curr.west);
			currentCell = cells[currentCell.current - 1];
		}
		if (choose.current == curr.current - xSize)
		{
			Object.Destroy(curr.south);
			currentCell = cells[currentCell.current - xSize];
		}
		if (choose.current == curr.current + xSize)
		{
			Object.Destroy(curr.north);
			currentCell = cells[currentCell.current + xSize];
		}
	}

	private void mazeCreation(List<Cell> allCells)
	{
		int num = 0;
		int index = Random.Range(0, allCells.Count);
		currentCell = allCells[index];
		while (num < allCells.Count)
		{
			AddNears(currentCell);
			int index2 = Random.Range(0, neighboringCell.Count);
			if (neighboringCell.Any())
			{
				lastCellVisited.Add(currentCell);
				breakWall(currentCell, neighboringCell[index2]);
				neighboringCell.Clear();
				num++;
				currentCell.isVisited = true;
			}
			else
			{
				index = Random.Range(0, lastCellVisited.Count);
				currentCell = lastCellVisited[index];
			}
		}
	}

	public void DestroyMaze()
	{
		Object.Destroy(Maze);
	}
}
