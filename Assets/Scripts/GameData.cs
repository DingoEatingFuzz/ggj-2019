using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour {
  static public ScreenGraph Map = new ScreenGraph() {
    { "FrontDoor",
      new ScreenNode("FrontDoor", new List<ScreenExit> {
        new ScreenExit { Id = "Left", To = "CircleKey", Exit = "Right" },
        new ScreenExit { Id = "Right", To = "DoubleJump", Exit = "Left" }
      })
    },
    { "CircleKey",
      new ScreenNode("CircleKey", new List<ScreenExit> {
        new ScreenExit { Id = "Left", To = "DoubleJump", Exit = "Right" },
        new ScreenExit { Id = "Right", To = "DoubleJump", Exit = "Left" }
      })
    },
    { "DoubleJump",
      new ScreenNode("DoubleJump", new List<ScreenExit> {
        new ScreenExit { Id = "Left", To = "CircleKey", Exit = "Right" },
        new ScreenExit { Id = "Right", To = "FrontDoor", Exit = "Left" }
      })
    },
  };

  static public void GoToScreen(string To, string Exit) {
    var currentScene = SceneManager.GetActiveScene();
    var node = GameData.Map[To];
    var player = GameObject.Find("TempCharacter");

    SceneManager.LoadScene(To, LoadSceneMode.Additive);
    SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(To));

    // Find the exit game object to position player (e.g., LeftExit for Left)
    var exit = GameObject.Find(Exit + "Exit");

    // Move the player to the exit coordinates
    player.transform.position = exit.transform.position;

    // Maybe unload scenes? It's not necessary, but it's probably good for memory at some point
    //SceneManager.UnloadSceneAsync(currentScene);
  }
}
