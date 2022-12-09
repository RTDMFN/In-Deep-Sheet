using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator sceneTransitionAnim;
    
    public GameState State;
    
    public Transform player;

    bool inside;

    void Awake(){
        instance = this;
    }

    void Start(){
        SpawnManager.instance.SpawnEntities();
        SwitchState(GameState.Outside);
        sceneTransitionAnim.Play("OpenScene");
    }

    void SwitchState(GameState state){
        switch(state){
            case GameState.Inside:
                HandleInsideState();
                break;
            case GameState.Outside:
                HandleOutsideState();
                break;
            case GameState.Paused:
                HandlePauseState();
                break;
            case GameState.Win:
                HandleWinState();
                break;
            case GameState.Lose:
                HandleLoseState();
                break;
            case GameState.Default:
                break;
        }
    }

    //Public Functions

    public void Win(){
        SwitchState(GameState.Win);
    }

    public void Lose(){
        SwitchState(GameState.Lose);
    }

    public void Pause(){
        if(State == GameState.Inside || State == GameState.Outside){
            SwitchState(GameState.Paused);
        }else if(State == GameState.Paused){
            if(inside){
                SwitchState(GameState.Inside);
            }else{
                SwitchState(GameState.Outside);
            }
        }
    }

    public void LookingInside(){
        SwitchState(GameState.Inside);
    }

    public void LookingOutside(){
        SwitchState(GameState.Outside);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------

    //Handler Functions
    void HandleWinState(){
        State = GameState.Win;
        Cursor.visible = true;
        MenuManager.instance.ShowWinScreen();
    }

    void HandleLoseState(){
        State = GameState.Lose;
        Cursor.visible = true;
        MenuManager.instance.ShowLoseScreen();
    }

    void HandlePauseState(){
        State = GameState.Paused;
        Cursor.visible = true;
        MenuManager.instance.ShowPauseScreen();
    }

    void HandleInsideState(){
        State = GameState.Inside;
        Cursor.visible = false;
        MenuManager.instance.ShowGameScreen();
        MenuManager.instance.HideStartText();
        inside = true;
        SpawnManager.instance.StartGame();
    }

    void HandleOutsideState(){
        State = GameState.Outside;
        Cursor.visible = false;
        MenuManager.instance.ShowGameScreen();
        inside = false;
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------

    //Scene Manager Functions

    public void LoadGameScene(){
        FindObjectOfType<AudioManager>().Play("Menu Select");
        StartCoroutine(ChangeScene("Game"));
    }

    public void LoadMainMenuScene(){
        FindObjectOfType<AudioManager>().Play("Menu Select");
        StartCoroutine(ChangeScene("MainMenu"));
    }

    IEnumerator ChangeScene(string name){
        sceneTransitionAnim.Play("CloseScene");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(name);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------------------------------------------
}

public enum GameState{
    Default,
    Inside,
    Outside,
    Win,
    Lose,
    Paused,

}
