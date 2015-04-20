using UnityEngine;
using System.Collections;

public class EduGame : MonoBehaviour {
    public int housesLeft = 0;

    public enum State {
        InGame,
        GameOver,
        Win
    };
    public State state = State.InGame;
    public AudioClip winSound;

    public float timeBeforeRestart = 3;
    public Texture loseTexture;
    public Texture winTexture;

    public void EnterGameOver() {
        if(state != State.InGame) {
            return;
        }
        Debug.Log("Entering game over state");
        state = State.GameOver;
        StartCoroutine(RestartSoon());
    }

    public void EnterWinState() {
        if(state != State.InGame) {
            return;
        }
        Debug.Log("Entering win state");
        state = State.Win;
        StartCoroutine(RestartSoon());
        AudioSource musicSource = GetComponentInChildren<AudioSource>();
        musicSource.Stop();
        musicSource.PlayOneShot(winSound);
    }

    private IEnumerator RestartSoon() {
        yield return new WaitForSeconds(timeBeforeRestart);
        Restart();
    }

    public void Restart() {
        Application.LoadLevel(0);
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            Restart();
        }
    }

    public void OnGUI() {
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        if(state == State.GameOver) {
            GUI.DrawTexture(
                rect,
                loseTexture,
                ScaleMode.ScaleToFit,
                alphaBlend: true
            );
        }
        else if(state == State.Win) {
            GUI.DrawTexture(
                rect,
                winTexture,
                ScaleMode.ScaleToFit,
                alphaBlend: true
            );
        }
        //GUI.Label(rect, housesLeft + " houses");
    }
}
