using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public GridInfo gridInfo;
    public GridInfo[,] gridInfos;
    public Transform gridsTransform;
    public Vector2Int start = new Vector2Int(0, 0);
    public Vector2Int end = new Vector2Int(9, 9);
    public bool[,] grid;
    public Button startButton;

    public void StartBFS()
    {
        foreach (var x in gridsTransform.GetComponentsInChildren<GridInfo>()) Destroy(x.gameObject);
        // Tạo bản đồ mẫu: tất cả đều đi được
        gridInfos = new GridInfo[width, height];
        grid = new bool[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = true;
                gridInfos[x, y] = Instantiate(gridInfo, gridsTransform);
                gridInfos[x, y].index = new Vector2Int(x, y);
            }
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                int _randomIndex = Random.RandomRange(0, 6);
                if(_randomIndex == 0)
                {
                    grid[x, y] = false;
                    gridInfos[x, y].image.color = Color.grey;
                }
            }
        grid[0, 0] = true;
        grid[9, 9] = true;
        gridInfos[0, 0].image.color = Color.blue;
        gridInfos[9, 9].image.color = Color.green;
        List<Vector2Int> path = BFS(start, end);

        if (path.Count > 0)
        {
            Debug.Log("Path found:");
            float time = 0.5f;
            foreach (var p in path)
            {
                Debug.Log(p);
                StartCoroutine(DelayCall(time, p, Color.red));
                time += 0.5f;
                //gridInfos[p.x, p.y].StartColorChange(Color.red);
            }
        }
        else
        {
            Debug.Log("No path found.");
        }
    }

    List<Vector2Int> BFS(Vector2Int start, Vector2Int goal)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        float time = 0.1f;
        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);
            foreach (var dir in directions)
            {
                Vector2Int neighbor = current + dir;

                if (IsInBounds(neighbor) && grid[neighbor.x, neighbor.y] && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    StartCoroutine(DelayCall(time,neighbor,Color.yellow));
                    time += 0.1f;
                    //gridInfos[neighbor.x, neighbor.y].StartColorChange(Color.yellow);
                    cameFrom[neighbor] = current;
                }
            }
        }
        return new List<Vector2Int>(); // Không tìm được đường
    }

    IEnumerator DelayCall(float time, Vector2Int vector2Int, Color color) {
        yield return new WaitForSeconds(time);
        if(gridInfos[vector2Int.x, vector2Int.y].image.color != Color.blue && gridInfos[vector2Int.x, vector2Int.y].image.color != Color.green)
        gridInfos[vector2Int.x, vector2Int.y].StartColorChange(color);
    }

    bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
}
