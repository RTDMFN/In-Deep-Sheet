using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI winTimeText;

    public Image allyImage;

    public float timeInside;

    public GameObject gameScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject howToStartText;

    void Awake(){
        instance = this;
    }

    void Start(){
        UpdateAllySprite();
        StartGame();
    }

    void Update(){
        UpdateTimerText();
    }

    public void ShowPauseScreen(){
        HideAllScreens();
        pauseScreen.transform.localScale = Vector3.one;
    }

    public void ShowGameScreen(){
        HideAllScreens();
        gameScreen.transform.localScale = Vector3.one;
    }

    public void ShowWinScreen(){
        HideAllScreens();
        winScreen.transform.localScale = Vector3.one;
    }

    public void ShowLoseScreen(){
        HideAllScreens();
        loseScreen.transform.localScale = Vector3.one;
    }

    void HideAllScreens(){
        pauseScreen.transform.localScale = Vector3.zero;
        winScreen.transform.localScale = Vector3.zero;
        loseScreen.transform.localScale = Vector3.zero;
        gameScreen.transform.localScale = Vector3.zero;
    }

    void StartGame(){
        pauseScreen.SetActive(true);
        gameScreen.SetActive(true);
        winScreen.SetActive(true);
        loseScreen.SetActive(true);
        pauseScreen.transform.localScale = Vector3.zero;
        winScreen.transform.localScale = Vector3.zero;
        loseScreen.transform.localScale = Vector3.zero;
    }

    void UpdateTimerText(){
        if(GameManager.instance.State == GameState.Inside){
            timeInside += Time.deltaTime;
            timerText.text = timeInside.ToString("#.00") + "s";
            winTimeText.text = timerText.text;
        }
    }

    void UpdateAllySprite(){
        allyImage.sprite = SpawnManager.instance.ally.sprite;
    }

    public void HideStartText(){
        howToStartText.SetActive(false);
    }
}
