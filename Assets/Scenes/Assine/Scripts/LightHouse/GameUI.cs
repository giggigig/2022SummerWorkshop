using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<S_Player>().OnLose += ShowGameLoseUI;
        //Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<S_Player>().OnReachedEndofLevel += ShowGameWinUI;
    }
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }   
    }
    void ShowGameWinUI()
    {
        OnGameOver(gameWinUI);
    }
    void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
    }

    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        //Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<S_Player>().OnLose -= ShowGameLoseUI;
        FindObjectOfType<S_Player>().OnReachedEndofLevel -= ShowGameWinUI;
    }

    //SceneManagement�� �̿� ������ ����ǰ� �����̽��ٸ� ������ �� �����
    //gameover�϶� UI �� setactive���ִ� �ڵ�
}
