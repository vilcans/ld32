using UnityEngine;

public class EduGame : MonoBehaviour {
    public int numberOfTerrorists;

    public enum State {
        InGame,
        GameOver,
        Win
    };
    public State state = State.InGame;

    public Texture loseTexture;
    public Texture winTexture;

    public void EnterGameOver() {
        if(state != State.InGame) {
            return;
        }
        Debug.Log("Entering game over state");
        state = State.GameOver;
    }

    public void OnGUI() {
        if(state == State.GameOver) {
            GUI.DrawTexture(
                new Rect(0, 0, Screen.width, Screen.height),
                loseTexture,
                ScaleMode.ScaleToFit,
                alphaBlend: true
            );
        }
        else if(state == State.Win) {
            GUI.DrawTexture(
                new Rect(0, 0, Screen.width, Screen.height),
                loseTexture,
                ScaleMode.ScaleToFit,
                alphaBlend: true
            );
        }
    }
}
