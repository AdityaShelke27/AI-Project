using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindPathAStar : MonoBehaviour
{
    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;

    List<PathMarker> open = new();
    List<PathMarker> closed = new();

    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;
    bool done = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            BeginSearch();
        }
        if (Input.GetKeyDown(KeyCode.C) && !done)
        {
            Search(lastPos);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GetPath();
        }
    }
    void GetPath()
    {
        RemoveAllMarkers();

        PathMarker begin = lastPos;

        while(begin.parent != null)
        {
            GameObject pathBlock = Instantiate(pathP, new(begin.location.x * maze.scale, 0, begin.location.z * maze.scale), Quaternion.identity);
            begin = begin.parent;
        }
    }
    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        List<MapLocation> mapLocations = new();

        for (int i = 0; i < maze.depth; i++)
        {
            for (int j = 0; j < maze.width; j++)
            {
                if (maze.map[i, j] != 1)
                {
                    mapLocations.Add(new MapLocation(i, j));
                }
            }
        }

        mapLocations.Shuffle();

        Vector3 startLocation = new(mapLocations[0].x * maze.scale, 0, mapLocations[0].z * maze.scale);
        startNode = new(new(mapLocations[0].x, mapLocations[0].z), 0, 0, 0, Instantiate(start, startLocation, Quaternion.identity), null);

        Vector3 goalLocation = new(mapLocations[1].x * maze.scale, 0, mapLocations[1].z * maze.scale);
        goalNode = new(new(mapLocations[1].x, mapLocations[1].z), 0, 0, 0, Instantiate(end, goalLocation, Quaternion.identity), null);

        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPos = startNode;
    }
    void Search(PathMarker thisNode)
    {
        if(thisNode == null) return;
        if(thisNode.Equals(goalNode))
        {
            done = true;
            return;
        }

        foreach(MapLocation dir in maze.directions)
        {
            MapLocation neighbor = thisNode.location + dir;

            if (maze.map[neighbor.x, neighbor.z] == 1) continue;
            
            if (neighbor.x >= maze.width || neighbor.x < 1) continue;
            if (neighbor.z >= maze.depth || neighbor.z < 1) continue;
            if (IsClosed(neighbor)) continue;

            float G = thisNode.G + Vector2.Distance(thisNode.location.ToVector(), neighbor.ToVector());
            float H = thisNode.H + Vector2.Distance(thisNode.location.ToVector(), goalNode.location.ToVector());
            float F = G + H;

            GameObject pathBlock = Instantiate(pathP, new(neighbor.x * maze.scale, 0, neighbor.z * maze.scale), Quaternion.identity);

            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = $"G: {G.ToString("0.00")}";
            values[1].text = $"H: {H.ToString("0.00")}";
            values[2].text = $"F: {F.ToString("0.00")}";

            if(!UpdateMarker(neighbor, G, H, F, thisNode))
            {
                open.Add(new(neighbor, G, H, F, pathBlock, thisNode));
            }
        }

        open = open.OrderBy(p => p.F).ThenBy(n => n.H).ToList();
        PathMarker pm = open[0];
        closed.Add(pm);
        open.RemoveAt(0);
        pm.marker.GetComponent<MeshRenderer>().material = closedMaterial;
        lastPos = pm;
    }
    bool UpdateMarker(MapLocation location, float g, float h, float f, PathMarker prt)
    {
        foreach(PathMarker p in open)
        {
            if(p.location.Equals(location))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prt;

                return true;
            }
        }
        return false;
    }
    bool IsClosed(MapLocation marker)
    {
        foreach(PathMarker p in closed)
        {
            if(p.location.Equals(marker))
            {
                return true;
            }
        }

        return false;
    }
    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");

        foreach (GameObject marker in markers)
            Destroy(marker);
    }
}

class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject marker;
    public PathMarker parent;

    public PathMarker(MapLocation _location, float g, float h, float f, GameObject _marker, PathMarker _parent)
    {
        location = _location;
        G = g;
        H = h;
        F = f;
        marker = _marker;
        parent = _parent;
    }

    public override bool Equals(object obj)
    {
        if(obj == null || !this.GetType().Equals(obj.GetType())) return false;

        PathMarker other = obj as PathMarker;

        return location.Equals(other.location);
    }

    public override int GetHashCode()
    {
        return 0;
    }
}