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
    var node = GameData.Map[To];
    // Go to the matching scene
    SceneManager.LoadScene(To);
    // Move the player to the exit coordinates
  }
}
