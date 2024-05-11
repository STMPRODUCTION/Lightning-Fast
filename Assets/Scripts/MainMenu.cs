using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void QuitGame ()
    {
         Application .Quit();
         Debug.Log("QUIT!");
    }
     public void nextlevel ()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
     }
     public void MainMeniu ()
     {
         SceneManager.LoadScene("MainMenu");
     }
     public void Credits ()
     {
        Debug.Log("do something");
     }
     public void Settings ()
     {
        Debug.Log("do something");
     }
    public void tutorial()
    {
        SceneManager.LoadScene("tutorial");
    }
    public void Retry ()
     {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
     }
}