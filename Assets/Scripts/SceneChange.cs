using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneChange : MonoBehaviour
{
    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Tutorial"); 
    }
    public void ChangeToStartScene()
    {
        SceneManager.LoadScene("Main"); 
    }
}