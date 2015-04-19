using UnityEngine;

public class EduGame : MonoBehaviour {
    public int numberOfTerrorists;

    public enum State {
        InGame,
        GameOver,
    };
    public State state = State.InGame;

    public void EnterGameOver() {
        if(state != State.InGame) {
            return;
        }
        Debug.Log("Entering game over state");
        state = State.GameOver;
    }
}
