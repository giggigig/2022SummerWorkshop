using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cube;
    public GameObject tile;
    public Transform t_cube;
    public Transform t_tile;
    private Vector3 t_vector;

    [Header("floor")]
    private List<GameObject> floors = new List<GameObject>();
    public int row = 5;
    public int column = 5;
    private int[,] _gridArray;

    public InputField _powerText;
    public Toggle _cubeToggle;

    public int num;
    public bool isMakeCube;

    int rand;
    void Start()
    {
        t_vector = t_tile.position;

        if(floors.Count < column)
        {
            GameObject floor = Instantiate(tile);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        rand = Random.Range(1, 255);
        isMakeCube = _cubeToggle.isOn;
        string s = _powerText.text;
        if(s == "") { return; }
        num = int.Parse(s); 
        
        if(num >= 256)
        {
            isMakeCube = false;
            _cubeToggle.isOn = false;
        }
    }

    public void PowerNum()
    {
        num *= 2;
        _powerText.text = num.ToString();
        if (isMakeCube)
        {
            for (int i = 0; i < num; i++)
            {
                GameObject instance = Instantiate(cube,
                    new Vector3( t_cube.position.x + Random.Range(1,3), t_cube.position.y, t_cube.position.z + Random.Range(1, 3)), t_cube.rotation) as GameObject;
                instance.GetComponent<Renderer>().material.color = new Vector4(rand, rand, rand, 1);
            }
        }
    }
    public void TileGrid(int x, int y)
    {
        row = x;
        column = y;

        _gridArray = new int[row, column];

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                _gridArray[i, j] = 0;

                var position = Vector3.zero;


            }
        }

    }
}
