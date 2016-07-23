using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MapDraw : MonoBehaviour
{
    public GameObject wall;
    public Transform origin;

    public int population = 100;
    public int cross = 5;

    private System.Diagnostics.Process process;
    private System.Diagnostics.ProcessStartInfo startInfo;

    private bool busy = false;
    private List<GameObject> walls;

    // Use this for initialization
    void Start()
    {
        process = new System.Diagnostics.Process();
        startInfo = new System.Diagnostics.ProcessStartInfo();
        walls = new List<GameObject>();
    }

    void OnCollisionEnter(Collision newCollision)
    {
        if (newCollision.gameObject.tag == "Player" && !busy)
        {
            busy = true;
            Debug.Log("load");
            loadMap();
            busy = false;
        }
    }

    private void loadMap()
    {
        if (walls.Count > 0)
            foreach (GameObject o in walls)
                Destroy(o);

        walls.Clear();

        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/C AGAC.exe " + population + " 20 20 " + cross + " 1 mylog";
        process.StartInfo = startInfo;
        process.Start();
        process.WaitForExit();

        string[] curMap = File.ReadAllLines("mylog");

        Debug.Log("creating");
        for (int i = 0; i < (curMap.Length - 2); i++)
        {
            for (int j = 0; j < curMap[i].Length; j++)
            {
                if (curMap[i][j] == '1')
                    walls.Add(Instantiate(wall, origin.position, origin.rotation) as GameObject);
                origin.position += origin.right;
            }
            origin.position -= origin.right * curMap[i].Length;
            origin.position -= origin.forward;
        }
        origin.position += origin.forward * (curMap.Length - 2);
    }
}
