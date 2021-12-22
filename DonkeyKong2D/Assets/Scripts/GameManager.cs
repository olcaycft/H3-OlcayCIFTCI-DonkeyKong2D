using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);  //dont destroy our gamemanager obj.
        NewGame();
    }

    private void LoadLevel(int index)
    {
        level = index;
        
        Camera camera=Camera.main;
        if (camera != null)
        {
            camera.cullingMask = 0; //camera not the render anything.
        }
        
        Invoke(nameof(LoadScene),1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }
    private void NewGame()
    {
        lives = 3;
        score = 0;
        
        LoadLevel(1);
    }

    public void LevelComplete()
    {
        score += 1000;

        int nextLevel = level + 1;
        

        if (nextLevel<SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        }
        else
        {
            LoadLevel(1);
        }
    }

    public void LevelFailed()
    {
        lives--;
        if (lives<=0)
        {
            NewGame();
        }
        else
        {
            LoadLevel(level);
        }
    }
}
