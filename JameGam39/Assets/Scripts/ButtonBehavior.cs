using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehavior : MonoBehaviour
{
    [SerializeField] private string mainScene = "Main";
    [SerializeField] private string mainMenu = "Menu";
    [SerializeField] private RectTransform fader;
    

    private void Start(){
        fader.gameObject.SetActive(true);
        fader.DOScale(new Vector2(1,1), 0f);
        fader.DOScale(Vector2.zero, 1f).SetEase(Ease.InOutExpo).OnComplete (() => {
            fader.gameObject.SetActive(false);
        });
    }

    public void OpenGameScene()
    {
        fader.gameObject.SetActive(true);
        fader.DOScale(Vector2.zero, 0f);
        fader.DOScale(new Vector2(1,1), 1f).SetEase(Ease.InOutExpo).OnComplete (() => {
            Invoke("LoadScene", 0.5f);
        });
    }

    public void OpenMainMenu(){
        DeathScreen.instance.deathScreen.SetActive(false);
        fader.gameObject.SetActive(true);
        fader.DOScale(Vector2.zero, 0f);
        fader.DOScale(new Vector2(1,1), 1f).SetEase(Ease.InOutExpo).OnComplete (() => {
            Invoke("MainMenu", 0.5f);
        });
    }

    public void LoadScene(){
        SceneManager.LoadScene(mainScene);
    }

    public void MainMenu(){
        SceneManager.LoadScene(mainMenu);
    }
    
    public void Quit() 
    {
        Application.Quit();
    }
}
