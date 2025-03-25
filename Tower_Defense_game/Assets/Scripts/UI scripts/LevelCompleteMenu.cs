using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{

    [SerializeField] GameObject Lvl_complete;
    public void Complete_Level()
    {
        Lvl_complete.SetActive(true);
        Time.timeScale = 0;
    }

    public void Next_Level()
    {
                
        SceneManager.LoadScene("Lvl_2-Intro");

    }

    public void Next_Level_2()
    {
                
        SceneManager.LoadScene("Lvl_3-Intro");

    }

    public void Next_Level_3()
    {
                
        SceneManager.LoadScene("Lvl_1-CH2-Intro");

    }

    public void Next_Level_Ch2_1()
    {
                
        SceneManager.LoadScene("Lvl_2-CH2-Intro");

    }

    public void Next_Level_Ch2_2()
    {
                
        SceneManager.LoadScene("Lvl_3-CH2-Intro");

    }

    public void Next_Level_Ch2_3()
    {
                
        SceneManager.LoadScene("Lvl_1-CH3-Intro");

    }

    public void Next_Level_Ch3_1()
    {
                
        SceneManager.LoadScene("Lvl_2-CH3-Intro");

    }

     public void Next_Level_Ch3_2()
    {
                
        SceneManager.LoadScene("Lvl_3-CH3-Intro");

    }

    public void Next_Level_Ch3_3()
    {
                
        SceneManager.LoadScene("Finish_Game");

    }

    public void Finish_Game()
    {
                
        SceneManager.LoadScene("MainMenu");

    }

    





}
