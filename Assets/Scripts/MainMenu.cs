using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject MM;
    public GameObject SM;
    public GameObject CR;
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void PlayVSCpu()
    {
        SceneManager.LoadScene("VSCPU");
    }
    public void QuitGame ()
    {
         Application.Quit();
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
        CR.SetActive(true);
        MM.SetActive(false);
     }
     public void Credits1 ()
     {
        Debug.Log("do something");
        CR.SetActive(false);
        MM.SetActive(true);
     }
     public void Alegers ()
     {
        SM.SetActive(true);
        MM.SetActive(false);
     }
     public void Alegers1 ()
     {
        SM.SetActive(false);
        MM.SetActive(true);
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