using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

// GameManager class that utilizes a singleton pattern

public class GameManager : Singleton<GameManager> {

    // Track a vertex selected 
    public GameObject selectedVertex;

    // Boolean to see if game is done and a boolean to store is plane color is changed already (plane color will change after the user completes the game)
    bool completed;
    bool changedPlanecolor;

    // Prefabs for the edges and vertices
    public Vertex vertexPrefab;
    public Edge edgePrefab;

    // List of edges and vertices
    public List<Vertex> static_vertex_list;
    public List<Vertex> dynamic_vertex_list;
    public Edge[,] edges_list;

    string[] adjMatrixLines;
    string[] posLines;

    // offset to use for static graph
    int staticGraphOffset;

    // Vertex mapping that the user defines
    public Hashtable mapping;


    // Use this for initialization 
    void Start() {
        selectedVertex = null;
        completed = false;
        changedPlanecolor = false;
        mapping = new Hashtable();
    }

    private void Awake()
    {
        static_vertex_list = new List<Vertex>();
        dynamic_vertex_list = new List<Vertex>();
    }

    // Update is called once per frame
    void Update()
    {
#if !completed
        Hashtable isomorphism = GameManager.Instance.mapping;
        if (GameManager.Instance.mapping.Count == GameManager.Instance.static_vertex_list.Count)
        {
            foreach (Vertex vertex in GameManager.Instance.static_vertex_list)
            {
                Vertex mappedVertex = (Vertex)isomorphism[vertex.information.vertexNumber];
                List<int> adjacentToMappedVertexNum = mappedVertex.adjacentVertexNum.ToList<int>();
                List<int> adjacentToMappedVertexNumConverted = new List<int>();
                foreach (int index in adjacentToMappedVertexNum)
                {
                    Vertex v = (Vertex)isomorphism[index];
                    adjacentToMappedVertexNumConverted.Add(v.information.vertexNumber);
                }
                List<int> adjacentToStaticVertexNum = vertex.adjacentVertexNum.ToList<int>();
                Debug.Log("dynamic vertices" + string.Join(",", adjacentToMappedVertexNumConverted));
                Debug.Log("static vertices" + string.Join(",", adjacentToStaticVertexNum));

                if (!adjacentToMappedVertexNumConverted.All(adjacentToStaticVertexNum.Contains)) return;
            }
            GameObject[] graphComponents = UnityEngine.Object.FindObjectsOfType<GameObject>();
            for (int i = 0; i < graphComponents.Length; i++)
            {
                if (graphComponents[i].name == "Plane")
                {
                    graphComponents[i].GetComponent<Renderer>().material.color = Color.green;
                    continue;
                }
            }
            completed = true;
        } 
#endif
    }

    // changes the graph
    public void ChangeGraph()
    {   
        // graphComponenets stores all gameobjects rendered until now
        GameObject[] graphComponents = UnityEngine.Object.FindObjectsOfType<GameObject>();
        completed = false;
        changedPlanecolor = false;

        // we find the gameobjects that are either vertices or edges, and destroy them
        for (int i = 0; i < graphComponents.Length; i++)
        {
            if (graphComponents[i].name == "Sphere(Clone)" || graphComponents[i].name == "Cylinder(Clone)") Destroy(graphComponents[i]);
            if (graphComponents[i].name == "Plane") graphComponents[i].GetComponent<Renderer>().material.color = Color.white;
        }
        GameManager.Instance.drawGraph(vertexPrefab, edgePrefab);
    }


    public void drawGraph(Vertex vertexPrefab, Edge edgePrefab)
    {
        // Get the list of directories in Assets/Resources that has information of graphs
        string[] directoryList = Directory.GetDirectories("Assets/Resources");
        List<string> directoryNameList = new List<string>();

        // Parse the directoryList to extract the directory names
        foreach (string s in directoryList)
        {
            var dir = new DirectoryInfo(s);
            directoryNameList.Add(dir.Name);
        }

        int rand = UnityEngine.Random.Range(0, directoryNameList.Count);
        
        // Get the path for the adjacency matrix and position matrix
        string adjMatrixPath = Path.Combine(directoryNameList[rand], "AdjMatrix");
        string posPath = Path.Combine(directoryNameList[rand], "Pos");

        //Draw the graph
        GameManager.Instance.buildGraph(Resources.Load(adjMatrixPath, typeof(TextAsset)) as TextAsset, Resources.Load(posPath, typeof(TextAsset)) as TextAsset, vertexPrefab, edgePrefab, false);
        GameManager.Instance.buildGraph(Resources.Load(adjMatrixPath, typeof(TextAsset)) as TextAsset, Resources.Load(posPath, typeof(TextAsset)) as TextAsset, vertexPrefab, edgePrefab, true);
    }

    // Build graph from given textfile, vertex/edge Prefabs and whether or not the graph is static
    public void buildGraph(TextAsset adjMatrix, TextAsset pos, Vertex vertexPrefab, Edge edgePrefab, bool staticGraph)
    {
        // Initialize the vertex_list for new graph
        static_vertex_list = new List<Vertex>();
        dynamic_vertex_list = new List<Vertex>();

        this.vertexPrefab = vertexPrefab;
        this.edgePrefab = edgePrefab;

        // Parse the textfiles
        adjMatrixLines = Regex.Split(adjMatrix.text, "\n");
        posLines = Regex.Split(pos.text, "\n");

        // Initialize e
        edges_list = new Edge[adjMatrixLines.Length - 1, adjMatrixLines.Length - 1];

        // Store the smallest y-value for the plane
        int minYValue = Int32.MaxValue;
        int maxYValue = Int32.MinValue;

        // Store the smallest x-value for the plane
        int minXValue = Int32.MaxValue;
        int maxXValue = Int32.MinValue;

        // instantiate vertices and edges based on csv file.
        for (int i = 0; i < adjMatrixLines.Length - 1; i++)
        {
            string valueLineAdj = adjMatrixLines[i];
            string valueLinePos = posLines[i];

            string[] valuesAdj = Regex.Split(valueLineAdj, ",");
            string[] valuesPos = Regex.Split(valueLinePos, ",");
            

            // Get the x-y-z coordinate of each vertex
            int x_coord = Int32.Parse(valuesPos[0]);
            int y_coord = Int32.Parse(valuesPos[1]);
            int z_coord = Int32.Parse(valuesPos[2]);
            Vector3 position = new Vector3(x_coord, y_coord, z_coord);

            // Update the smallest and largest x-values
            if (x_coord < minXValue) minXValue = x_coord;
            if (x_coord > maxXValue) maxXValue = x_coord;

            // Update the smallest and largest y-values
            if (y_coord < minYValue) minYValue = y_coord;
            if (y_coord > maxYValue) maxYValue = y_coord;

            if (!staticGraph) staticGraphOffset = maxXValue - minXValue;
            

            // instantiate vertices

            if (!staticGraph)
            {
                Vertex obj = Instantiate(vertexPrefab, position, Quaternion.identity);
                obj.incidentEdges = new HashSet<Edge>();
                obj.tag = "movableVertex";
                obj.information.vertexNumber = i;
                dynamic_vertex_list.Add(obj);
            } else
            {
                position = new Vector3(x_coord + staticGraphOffset + 4, y_coord, z_coord);
                Vertex obj = Instantiate(vertexPrefab, position, Quaternion.identity);
                obj.incidentEdges = new HashSet<Edge>();
                obj.tag = "nonMovableVertex";
                obj.GetComponent<Vertex>().ChangeOpacity();
                obj.information.staticVertex = true;
                obj.information.vertexNumber = i;
                obj.gameObject.layer = 2;
                static_vertex_list.Add(obj);
            }
            
           
            
                
            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    int test = Int32.Parse(valuesAdj[j]);
                    if (test == 1)
                    {
                        if (staticGraph)
                        {
                            Vector3 other_pos = static_vertex_list[j].transform.position;
                            Vector3 edge_pos = Vector3.Lerp(position, other_pos, 0.5f);
                            Edge e = Instantiate(edgePrefab, edge_pos, Quaternion.identity);
                            e.staticEdge = true;
                            e.ChangeOpacity();
                            e.tag = "nonMovableEdge";
                            var offset = other_pos - position;
                            var scale = new Vector3(0.5f, offset.magnitude / 2, 0.5f);
                            e.transform.up = offset;
                            e.transform.localScale = scale;
                            e.adjacentVertices = new HashSet<Vertex>();
                            edges_list[i, j] = e;
                        } else
                        {
                            Vector3 other_pos = dynamic_vertex_list[j].transform.position;
                            Vector3 edge_pos = Vector3.Lerp(position, other_pos, 0.5f);
                            Edge e = Instantiate(edgePrefab, edge_pos, Quaternion.identity);
                            e.tag = "movableEdge";
                            var offset = other_pos - position;
                            var scale = new Vector3(0.5f, offset.magnitude / 2, 0.5f);
                            e.transform.up = offset;
                            e.transform.localScale = scale;
                            e.adjacentVertices = new HashSet<Vertex>();
                            edges_list[i, j] = e;
                        }
                    }
                }
            }
        }

        // Only populate vertexlist and edge list for dynamic graph - no need for static graphs
        if (staticGraph)
        {
            for (int i = 0; i < adjMatrixLines.Length - 1; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (edges_list[i, j] != null)
                    {
                        static_vertex_list[i].incidentEdges.Add(edges_list[i, j]);
                        static_vertex_list[j].incidentEdges.Add(edges_list[i, j]);
                        edges_list[i, j].adjacentVertices.Add(static_vertex_list[i]);
                        edges_list[i, j].adjacentVertices.Add(static_vertex_list[j]);
                    }
                }
            }
        } else
        {
            for (int i = 0; i < adjMatrixLines.Length - 1; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (edges_list[i, j] != null)
                    {
                        dynamic_vertex_list[i].incidentEdges.Add(edges_list[i, j]);
                        dynamic_vertex_list[j].incidentEdges.Add(edges_list[i, j]);
                        edges_list[i, j].adjacentVertices.Add(dynamic_vertex_list[i]);
                        edges_list[i, j].adjacentVertices.Add(dynamic_vertex_list[j]);
                    }
                }
            }
        }

        // Debug.Log(static_vertex_list.Count);
        if (staticGraph)
        {
            foreach (Vertex vertex in static_vertex_list)
            {
                vertex.recordAdjacentVerices();
            }
        } else
        {
            foreach (Vertex vertex in dynamic_vertex_list)
            {
                vertex.recordAdjacentVerices();
            }
        }
        


        // Adjust the height of the plane and the camerarig according to the min max coordinates of the vertices
        GameObject[] graphComponents = UnityEngine.Object.FindObjectsOfType<GameObject>();
        for (int i = 0; i < graphComponents.Length; i++)
        {
            if (graphComponents[i].name == "Plane")
            {
                Vector3 planeCoord = graphComponents[i].transform.position;
                planeCoord.y = minYValue - 5;
                graphComponents[i].transform.position = planeCoord;
            }
            if (graphComponents[i].name == "[CameraRig]")
            {
                Vector3 planeCoord = graphComponents[i].transform.position;
                planeCoord.y = (minYValue + maxYValue) / 2;
                planeCoord.x = planeCoord.x + 2;
                graphComponents[i].transform.position = planeCoord;
            }
        }

    }

    public void CheckAll()
    {
        Boolean valid_coloring = true;
        Boolean is_finished = true;
        int color_used = 0;
        List<Color> color_list = new List<Color>();

        for (int i = 0; i < adjMatrixLines.Length - 1; i++)
        {
            string valueLine = adjMatrixLines[i];
            string[] values = Regex.Split(valueLine, ",");
            Color this_color = static_vertex_list[i].rend.material.color;
            if (this_color == Color.white)
            {
                is_finished = false;
            }
            else
            {
                if (!color_list.Contains(this_color))
                {
                    color_used++;
                    color_list.Add(this_color);
                }
            }

            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    int test = Int32.Parse(values[j]);
                    if (test == 1)
                    {
                        Color other_color = static_vertex_list[j].rend.material.color;
                        if (this_color != Color.white && this_color != Color.white && this_color == other_color)
                        {
                            valid_coloring = false;
                            edges_list[i, j].rend.material.color = Color.black;
                        }
                        else
                        {
                            edges_list[i, j].rend.material.color = Color.white;
                        }
                    }
                }
            }
        }

     
    }
    public void moveEdges(Vertex v)
    {
        HashSet<Edge> adjEdges = v.incidentEdges;
        foreach (Edge edge in adjEdges)
        {
            Vertex other = null;
            foreach (Vertex o in edge.adjacentVertices)
            {
                if (o != v) other = o;
            }
            //edge.adjacentVertices.ToList<Vertex>();

            Vector3 curr_pos = v.transform.position;
            Vector3 other_pos = other.gameObject.transform.position;

            Vector3 edge_pos = Vector3.Lerp(curr_pos, other_pos, 0.5f);
            edge.transform.position = edge_pos;

            var offset = other_pos - curr_pos;
            var scale = new Vector3(0.5f, offset.magnitude / 2, 0.5f);
            edge.transform.up = offset;
            edge.transform.localScale = scale;
        }
    }

}

