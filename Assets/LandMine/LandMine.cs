using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LandMine : MonoBehaviour
{
    public int size = 10;
    public int percent = 20;
    public GameObject cube;
    public LayerMask hitmask;
    public GameObject Dancer;
    public GameObject GameOverUI;
    public TextMeshPro MineCountText;

    public bool isStart = true;
    
    int mineCount =0;
    int flagCount = 0;

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
                    mineCount++;
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
            MineCountText.text = (mineCount - flagCount).ToString();
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, hitmask))//����ĳ��Ʈ
                {
                    Square hitSquare = hit.transform.GetComponent<Square>();
                    if(!hitSquare.isFlaged)// ��� �ƴҶ�
                    {
                        if (!hitSquare.isClicked) // ó��Ŭ��
                        {
                            ClickedSquare(hitSquare);

                            if (!hitSquare.isMine) //���ھƴϸ�
                            {
                                hitSquare.Num(hitSquare.mineNum); //����ǥ��
                                if (hitSquare.mineNum == 0)//����Ȯ��
                                {
                                    ZeroExpand(hitSquare);
                                }
                            }
                            else //���ڸ�
                            {
                                hitSquare.mine.SetActive(true); //����ǥ��
                                hitSquare.GetComponent<MeshRenderer>().material.color = Color.red;
                                RevealMine(); //������� ǥ��
                                animator.enabled = false;
                                isStart = false; //���ӿ���
                            }
                        }
                        else//�̹� Ŭ��-�� Ŭ��
                        {
                            int flagNum = 0;
                            for (int i = 0; i < 9; i++)
                            {
                                if (hitSquare.boundary[i] != null)
                                {
                                    if (!hitSquare.boundary[i].isFlaged)
                                    {
                                        hitSquare.boundary[i].GetComponent<MeshRenderer>().material.color = Color.gray; //����ȭ -���� üũ
                                    }
                                    if (hitSquare.boundary[i].isFlaged) { flagNum++; }//��߰�������
                                    Debug.Log("flag" + flagNum);
                                }
                            }
                            if (hitSquare.mineNum == flagNum)
                            {
                                Debug.Log(flagNum);
                                for (int i = 0; i < 9; i++)
                                {
                                    if (hitSquare.boundary[i] != null && !hitSquare.boundary[i].isFlaged)
                                    {
                                        if (hitSquare.boundary[i].isMine) //���ڸ� == ��� �߸�����
                                        {
                                            hitSquare.boundary[i].mine.SetActive(true); //����ǥ��
                                            hitSquare.boundary[i].GetComponent<MeshRenderer>().material.color = Color.red;
                                            RevealMine(); //������� ǥ��
                                            animator.enabled = false;
                                            isStart = false; //���ӿ���
                                        }
                                        else
                                        {
                                            ClickedSquare(hitSquare.boundary[i]);
                                            hitSquare.boundary[i].Num(hitSquare.boundary[i].mineNum);
                                        }
                                    }
                                }
                            }
                        }
                    }          
                }
            }
            else if (Input.GetMouseButtonUp(0)) //���� �÷� �ǵ�����
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
                            flagCount--;
                        }
                        else
                        {
                            hitSquare.Flag.SetActive(true);
                            hitSquare.isFlaged = true;
                            flagCount++;
                        }
                    }
                }
            }
        }
        else
        {
            GameOverUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartScene();
            }
        }
        
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
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

    void ClickedSquare(Square s)
    {
        if (!s.isClicked)
        {
            s.transform.position = new Vector3(s.transform.position.x, s.transform.position.y, 0.1f);
        }
        s.isClicked = true;
        s.GetComponent<MeshRenderer>().material.color = Color.gray; //Ŭ����- ���ٲٱ�     
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

                //for (int j = 0; j < 3; j++)
                //{
                //    if (s.boundary[i].mineNum == 0)
                //       ZeroExpand(s.boundary[i]);
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
