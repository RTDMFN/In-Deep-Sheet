using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Transform crosshair;

    public Animator sceneTransitionAnim;

    void Start(){
        Cursor.visible = false;
        sceneTransitionAnim.Play("OpenScene");
    }

    void Update(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshair.position = (Vector2)mousePos;
    }

    public void LoadGameScene(){
        FindObjectOfType<AudioManager>().Play("Menu Select");
        StartCoroutine(ChangeScene());
    }

    public void QuitGame(){
        Application.Quit();
    }

    IEnumerator ChangeScene(){
        sceneTransitionAnim.Play("CloseScene");
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Game");
    }
}
