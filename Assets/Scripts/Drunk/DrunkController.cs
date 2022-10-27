using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrunkController : MonoBehaviour
{
    public GameObject panel;
    public TMP_InputField sizeField;
    public GameObject warningField;
    public Slider speedSlider;
    public TMP_Text moveCounter;
    public GameObject cameraText;

    public Material mat;

    private GameObject[][][] spheres3D;
    private GameObject[][] spheres2D;

    private int size;
    private bool started = false;
    private bool in3D = false;

    private Tuple<int, int, int> drunkAt3D = Tuple.Create(0, 0, 0);
    private Tuple<int, int> drunkAt2D = Tuple.Create(0, 0);
    private Vector3 goal = new Vector3();

    private Dictionary<Vector3, GameObject> lines = new Dictionary<Vector3, GameObject>();

    private float lastTime;
    private int moveNumber
    {
        get => _moveNumber;

        set
        {
            _moveNumber = value;
            moveCounter.text = _moveNumber.ToString();
        }
    }

    private int _moveNumber;
    private StartMode startMode = StartMode.Random;
    
    public enum StartMode
    {
        Random,
        Corners,
        Middle
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (speedSlider.value == 1 || Time.realtimeSinceStartup > lastTime + (1 - speedSlider.value))
            {
                MoveDrunk();
                lastTime = Time.realtimeSinceStartup;
            }
        }
    }

    public void ToMenuClick()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SelectStartMode(int mode)
    {        
        startMode = (StartMode)mode;
    }

    private void MoveDrunk()
    {
        if (in3D)
        {
            Vector3 oldPos = new Vector3(drunkAt3D.Item1, drunkAt3D.Item2, drunkAt3D.Item3);
            spheres3D[drunkAt3D.Item1][drunkAt3D.Item2][drunkAt3D.Item3].GetComponent<Renderer>().material.color = UnityEngine.Color.gray;
            List<Tuple<int, int, int>> moves = new List<Tuple<int, int, int>>();
            if (drunkAt3D.Item1 > 0)
            {
                moves.Add(Tuple.Create(drunkAt3D.Item1 - 1, drunkAt3D.Item2, drunkAt3D.Item3));
            }
            if (drunkAt3D.Item1 < size - 1)
            {
                moves.Add(Tuple.Create(drunkAt3D.Item1 + 1, drunkAt3D.Item2, drunkAt3D.Item3));
            }
            if (drunkAt3D.Item2 > 0)
            {
                moves.Add(Tuple.Create(drunkAt3D.Item1, drunkAt3D.Item2 - 1, drunkAt3D.Item3));
            }
            if (drunkAt3D.Item2 < size - 1)
            {
                moves.Add(Tuple.Create(drunkAt3D.Item1, drunkAt3D.Item2 + 1, drunkAt3D.Item3));
            }
            if (drunkAt3D.Item3 > 0)
            {
                moves.Add(Tuple.Create(drunkAt3D.Item1, drunkAt3D.Item2, drunkAt3D.Item3 - 1));
            }
            if (drunkAt3D.Item3 < size - 1)
            {
                moves.Add(Tuple.Create(drunkAt3D.Item1, drunkAt3D.Item2, drunkAt3D.Item3 + 1));
            }
            drunkAt3D = moves[UnityEngine.Random.Range(0, moves.Count)];
            Vector3 newPos = new Vector3(drunkAt3D.Item1, drunkAt3D.Item2, drunkAt3D.Item3);
            Vector3 endPos = new Vector3(newPos.x - (newPos.x - oldPos.x) / 2, newPos.y - (newPos.y - oldPos.y) / 2, newPos.z - (newPos.z - oldPos.z) / 2);
            if (!lines.ContainsKey(endPos))
            {
                GameObject newLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                newLine.GetComponent<Renderer>().material = mat;
                newLine.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
                newLine.transform.localPosition = endPos;
                if (newLine.transform.localPosition.x != drunkAt3D.Item1)
                {
                    newLine.transform.localEulerAngles = new Vector3(0, 0, 90);
                }
                else if (newLine.transform.localPosition.z != drunkAt3D.Item3)
                {
                    newLine.transform.localEulerAngles = new Vector3(90, 0, 0);
                }
                lines.Add(endPos, newLine);
            }            
            lines[endPos].GetComponent<Renderer>().material.color = UnityEngine.Color.HSVToRGB(((moveNumber++) / 100f) % 1, 0.5f, 0.5f);
            if (goal == new Vector3(drunkAt3D.Item1,drunkAt3D.Item2,drunkAt3D.Item3))
            {
                spheres3D[drunkAt3D.Item1][drunkAt3D.Item2][drunkAt3D.Item3].GetComponent<Renderer>().material.color = UnityEngine.Color.magenta;
                started = false;
            }
            else
            {
                spheres3D[drunkAt3D.Item1][drunkAt3D.Item2][drunkAt3D.Item3].GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
            }
        }
        else
        {
            Vector3 oldPos = new Vector3(drunkAt2D.Item1, drunkAt2D.Item2, 0);
            spheres2D[drunkAt2D.Item1][drunkAt2D.Item2].GetComponent<Renderer>().material.color = UnityEngine.Color.gray;
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
            if (drunkAt2D.Item1 > 0)
            {
                moves.Add(Tuple.Create(drunkAt2D.Item1 - 1, drunkAt2D.Item2));
            }
            if (drunkAt2D.Item1 < size - 1)
            {
                moves.Add(Tuple.Create(drunkAt2D.Item1 + 1, drunkAt2D.Item2));
            }
            if (drunkAt2D.Item2 > 0)
            {
                moves.Add(Tuple.Create(drunkAt2D.Item1, drunkAt2D.Item2 - 1));
            }
            if (drunkAt2D.Item2 < size - 1)
            {
                moves.Add(Tuple.Create(drunkAt2D.Item1, drunkAt2D.Item2 + 1));
            }            
            drunkAt2D = moves[UnityEngine.Random.Range(0, moves.Count)];
            Vector3 newPos = new Vector3(drunkAt2D.Item1, drunkAt2D.Item2, 0);
            Vector3 endPos = new Vector3(newPos.x - (newPos.x - oldPos.x) / 2, newPos.y - (newPos.y - oldPos.y) / 2, 0);
            if (!lines.ContainsKey(endPos))
            {
                GameObject newLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                newLine.GetComponent<Renderer>().material = mat;
                newLine.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
                newLine.transform.localPosition = endPos;
                if (newLine.transform.localPosition.x != drunkAt2D.Item1)
                {
                    newLine.transform.localEulerAngles = new Vector3(0, 0, 90);
                }
                lines.Add(endPos, newLine);
            }
            lines[endPos].GetComponent<Renderer>().material.color = UnityEngine.Color.HSVToRGB(((moveNumber++) / 100f) % 1, 0.5f, 0.5f);
            if (goal == new Vector3(drunkAt2D.Item1, drunkAt2D.Item2, 0))
            {
                spheres2D[drunkAt2D.Item1][drunkAt2D.Item2].GetComponent<Renderer>().material.color = UnityEngine.Color.magenta;
                started = false;
            }
            else
            {
                spheres2D[drunkAt2D.Item1][drunkAt2D.Item2].GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
            }
        }
    }

    public void Initialize3D()
    {
        size = 0;
        moveNumber = 0;
        if (int.TryParse(sizeField.text,out size))
        {
            spheres3D = new GameObject[size][][];
            for(int x = 0; x < size; x++)
            {
                spheres3D[x] = new GameObject[size][];
                for(int y = 0; y < size; y++)
                {
                    spheres3D[x][y] = new GameObject[size];
                    for(int z = 0; z < size; z++)
                    {
                        GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        newSphere.GetComponent<Renderer>().material = mat;
                        newSphere.transform.position = new Vector3(x, y, z);
                        newSphere.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        spheres3D[x][y][z] = newSphere;
                    }
                }                
            }
            in3D = true;
            panel.SetActive(false);
            started = true;
            switch (startMode)
            {
                case StartMode.Random:
                    drunkAt3D = Tuple.Create(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size));
                    goal = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size));
                    while (goal == new Vector3(drunkAt3D.Item1, drunkAt3D.Item2, drunkAt3D.Item3))
                    {
                        goal = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size));
                    }
                    break;
                case StartMode.Corners:
                    drunkAt3D = Tuple.Create(0, 0, 0);
                    goal = new Vector3(size-1, size-1, size-1);                    
                    break;
                case StartMode.Middle:
                    drunkAt3D = Tuple.Create(size/2, size/2, size/2);
                    goal = new Vector3(0, 0, 0);                    
                    break;
            }
            spheres3D[drunkAt3D.Item1][drunkAt3D.Item2][drunkAt3D.Item3].GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;
            spheres3D[Mathf.RoundToInt(goal.x)][Mathf.RoundToInt(goal.y)][Mathf.RoundToInt(goal.z)].GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            GetComponent<CameraControl>().enabled = true;
            cameraText.SetActive(true);
        }
        else
        {
            warningField.SetActive(true);
        }
    }
    public void Initialize2D()
    {
        size = 0;
        moveNumber = 0;
        if (int.TryParse(sizeField.text, out size))
        {
            spheres2D = new GameObject[size][];
            for (int x = 0; x < size; x++)
            {
                spheres2D[x] = new GameObject[size];
                for (int y = 0; y < size; y++)
                {
                    GameObject newSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    newSphere.GetComponent<Renderer>().material = mat;
                    newSphere.transform.position = new Vector3(x, y, 0);
                    newSphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    spheres2D[x][y] = newSphere;
                }
            }
            in3D = false;
            panel.SetActive(false);
            started = true;
            switch (startMode)
            {
                case StartMode.Random:
                    drunkAt2D = Tuple.Create(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size));
                    goal = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), 0);
                    while (goal == new Vector3(drunkAt2D.Item1, drunkAt2D.Item2, 0))
                    {
                        goal = new Vector3(UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size), 0);
                    }
                    break;
                case StartMode.Corners:
                    drunkAt2D = Tuple.Create(0, 0);
                    goal = new Vector3(size - 1, size - 1, 0);
                    break;
                case StartMode.Middle:
                    drunkAt2D = Tuple.Create(size / 2, size / 2);
                    goal = new Vector3(0, 0, 0);
                    break;
            }
            spheres2D[drunkAt2D.Item1][drunkAt2D.Item2].GetComponent<Renderer>().material.color = UnityEngine.Color.yellow;            
            spheres2D[Mathf.RoundToInt(goal.x)][Mathf.RoundToInt(goal.y)].GetComponent<Renderer>().material.color = UnityEngine.Color.red;
            GetComponent<CameraControl>().enabled = true;
            cameraText.SetActive(true);
        }
        else
        {
            warningField.SetActive(true);
        }
    }

    public static System.Drawing.Color Rainbow(float progress)
    {
        float div = (Math.Abs(progress % 1) * 6);
        int ascending = (int)((div % 1) * 255);
        int descending = 255 - ascending;
        
        switch ((int)div)
        {
            case 0:
                return System.Drawing.Color.FromArgb(255, 255, ascending, 0);
            case 1:
                return System.Drawing.Color.FromArgb(255, descending, 255, 0);
            case 2:
                return System.Drawing.Color.FromArgb(255, 0, 255, ascending);
            case 3:
                return System.Drawing.Color.FromArgb(255, 0, descending, 255);
            case 4:
                return System.Drawing.Color.FromArgb(255, ascending, 0, 255);
            default: // case 5:
                return System.Drawing.Color.FromArgb(255, 255, 0, descending);
        }
    }

}
