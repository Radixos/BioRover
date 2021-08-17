using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public enum SceneCollection
    {
        MENU, GAMEPLAY, GAMEPLAY_ROAM
    }
    public SceneCollection changeTo;
    private Animator anim;
    
    void Start()
    {
 
    }

    // Update is called once per frame
    public void StartGame()
    {
        changeTo = SceneCollection.GAMEPLAY;
        SceneManager.LoadScene("MainGame_Mani");       
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}