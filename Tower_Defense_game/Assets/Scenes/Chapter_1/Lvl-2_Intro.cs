using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvl2Intro : MonoBehaviour
{
    //Load Scene
    public void ContinueButton()
    {
        SceneManager.LoadScene("Lvl_2");
    }
}