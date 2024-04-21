using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private string mainScene = "Main";
    public void LoadGame()
    {
        SceneManager.LoadScene(mainScene);
    }
}
