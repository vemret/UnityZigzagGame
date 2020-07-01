using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameStarted { get; private set; } //dışardan kimse set edemez sadece cagirabilir
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    public void StartGame() //sahneyi yeniden başlat
    {
        gameStarted = true;
    }

    public void RestartGame()
    {
        Invoke("Load", 1f); //gecikme 1sn
    }


    private void Load() // oyunda başka sahne olmadığından 0. yani kendi sahnemizi yükü edicez
    {
        SceneManager.LoadScene(0);
    }
}
