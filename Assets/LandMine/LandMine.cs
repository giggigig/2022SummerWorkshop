using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour
{
    public int size = 10;
    public int percent = 20;
    public GameObject cube;
    public LayerMask hitmask;
    public GameObject Dancer;

    bool isStart = true;
    
    int mineCount =0;

    Square[,] squares;

    Ray ray;
    RaycastHit hit;

    Color originColor;
    Animator animator;
 
    void Start()
    {
        animator = Dancer.GetComponent<Animator>();
        squares = new Square[size, size];
        //cube����
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int n = Random.Range(0, 100);
                squares[i, j] = Instantiate(cube, transform.position
                    + new Vector3(j, -i, 0), Quaternion.identity).GetComponent<Square>();
                squares[i, j].index = i * 10 + j;    
                if (n < percent)
                {
                    squares[i, j].SetMine();
                    Debug.Log("SetMine");
                }
            }
        }
        originColor = squares[0, 0].GetComponent<MeshRenderer>().material.color;

        SetBoundary();
        CheckMineCount();
    }

    void Update()
    {
        if (isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, hitmask))
                {
                    Square hitSquare = hit.transform.GetComponent<Square>();
                    if (!hitSquare.isClicked) // ó��Ŭ��
                    {
                        hitSquare.isClicked = true;
                        hitSquare.GetComponent<MeshRenderer>().material.color = Color.gray; //Ŭ����- ���ٲٱ�

                        if (!hitSquare.isMine) //���ھƴϸ�
                        {                         
                            hitSquare.Num(hitSquare.mineNum); //����ǥ��
                            if(hitSquare.mineNum == 0)//����Ȯ��
                            {
                               ZeroExpand(hitSquare);
                            }
                        }
                        else //���ڸ�
                        {
                            hitSquare.mine.SetActive(true); //����ǥ��
                            hitSquare.GetComponent<MeshRenderer>().material.color = Color.red;
                            RevealMine();
                            animator.enabled = false;
                            isStart = false;
                        }
                    }
                    else//�̹� Ŭ��
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (hitSquare.boundary[i] != null)
                            {
                                hitSquare.boundary[i].GetComponent<MeshRenderer>().material.color = Color.gray; //����ȭ -���� üũ
                            }
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0)) 
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                       if (!squares[i, j].isClicked)
                       {
                            squares[i, j].GetComponent<MeshRenderer>().material.color = originColor;
                       }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1)) // �÷��� �����
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, hitmask))
                {
                    Square hitSquare = hit.transform.GetComponent<Square>();
                    bool flag = hitSquare.isFlaged;
                    if (!hitSquare.isClicked)
                    {
                        if (flag)
                        {
                            hitSquare.Flag.SetActive(false);
                            hitSquare.isFlaged = false;
                        }
                        else
                        {
                            hitSquare.Flag.SetActive(true);
                            hitSquare.isFlaged = true;
                        }
                    }
                }
            }
        }
        
    }

    void SetBoundary()
    {

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int col = -1;
                int row = -1;
                for (int k = 0; k < 9; k++)
                {
                    if (i > 0 && i < size - 1 && j > 0 && j < size - 1)
                    {
                        squares[i, j].boundary[k] = squares[i + row, j + col];
                        squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                    }
                    else if (i == 0 && j > 0 && j < size - 1)
                    {
                        if (row > -1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }

                    }
                    else if (i == size - 1 && j > 0 && j < size - 1)
                    {
                        if (row < 1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }

                    }
                    else if (j == 0 && i > 0 && i < size - 1)
                    {
                        if (col > -1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }

                    }
                    else if (j == size - 1 && i > 0 && i < size - 1)
                    {
                        if (col < 1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }
                    }
                    else if (i == 0 && j == 0)
                    {
                        if (col > -1 && row > -1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }
                    }
                    else if (i == 0 && j == size - 1)
                    {
                        if (col < 1 && row > -1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }
                    }
                    else if (i == size - 1 && j == 0)
                    {
                        if (col > -1 && row < 1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }
                    }
                    else if (i == size - 1 && j == size - 1)
                    {
                        if (col < 1 && row < 1)
                        {
                            squares[i, j].boundary[k] = squares[i + row, j + col];
                            squares[i, j].boundary[k].boundary[8 - (k)] = squares[i, j];
                        }
                    }
                    col++;
                    if (col >1)
                    {
                        col = -1;
                        row++;
                    }
                }
            }
        }
    }

    void CheckMineCount()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < 9; k++)
                {
                    if(squares[i, j].boundary[k] != null && squares[i, j].boundary[k].isMine)
                    {
                        squares[i, j].mineNum++;
                    }
                }
            }
        }
    }

    void ZeroExpand(Square s)
    {
        for (int i = 0; i < 9; i++)
        {
            if (s.boundary[i] != null)
            {
                s.boundary[i].GetComponent<MeshRenderer>().material.color = Color.gray;
                s.boundary[i].isClicked = true;
                s.boundary[i].Num(s.boundary[i].mineNum);

                //if(s.boundary[i].mineNum == 0)
                //{
                //    ZeroExpand(s.boundary[i]);
                //}
            }
        }
    }

    void RevealMine()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (squares[i, j].isMine)
                {
                    squares[i, j].mine.SetActive(true); //����ǥ��
                    squares[i, j].GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }



    /** ���� Ȯ�� ����Լ�

if(hitSquare.mineNum == 0)
{
    for (int i = 0; i < 9; i++)
    {
        if (hitSquare.boundary[i] != null)
        {
            hitSquare.boundary[i].GetComponent<MeshRenderer>().material.color = Color.gray;
            hitSquare.boundary[i].isClicked = true;
            hitSquare.boundary[i].Num(hitSquare.boundary[i].mineNum);

            if(hitSquare.boundary[i].mineNum == 0)//����
            {
                for (int j = 0; j < 9; j++)
                {
                    hitSquare.boundary[i].boundary[j].GetComponent<MeshRenderer>().material.color = Color.gray;
                    hitSquare.boundary[i].boundary[j].isClicked = true;
                    hitSquare.boundary[i].boundary[j].Num(hitSquare.boundary[i].mineNum);
                }
            }
        }
    }
}

    **/
}
