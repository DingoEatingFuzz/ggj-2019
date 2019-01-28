using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour {
  public void GoToScreen(string To, string Exit) {
    Debug.Log("Going to Scene: " + To + ", Exit: " + Exit);
    var currentScene = SceneManager.GetActiveScene();
    var node = InnerDerp.Map[To];
    var player = GameObject.Find("Gina");

    var toScene = SceneManager.GetSceneByName(To);
    if (!toScene.isLoaded) {
      try {
        SceneManager.LoadScene(To, LoadSceneMode.Additive);
      } catch {
        Debug.Log("Leaving early due to no next scene");
        return;
      }
    }

    StartCoroutine(this.MovePlayerToScene(To, Exit, player));
  }

  public IEnumerator MovePlayerToScene(string sceneName, string exit, GameObject player) {
    // Set the active scene to the new scene
    yield return null;
    SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

    // Wait 1 frame for ActiveScene to change
    yield return null;

    SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName(sceneName));

    // Find all exits, since they share names across scenes
    var allExits = new List<GameObject>(GameObject.FindGameObjectsWithTag("exit"));
    // The correct exit matches exit name AND scene name
    var exitObj = allExits.Find(e => e.name == exit + "Exit" && e.scene.name == sceneName);
    if (exitObj == null) {
      Debug.Log("Could not find exit (" + exit + ")");
    } else {
            // Move the inner gina object to the exit coordinates
            player.GetComponentInChildren<PlayerControlScript>().spawnPosition = exitObj.transform.position;
            player.transform.GetChild(1).position = exitObj.transform.position;
    }

    // Finish coroutine
    yield break;
  }

  public IEnumerator SetActive(string name) {
    // Wait 1 frame
    yield return null;
    // Activate scene
    SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    // Finish coroutine
    yield break;
  }

  public class InnerDerp : MonoBehaviour
    {
        public static ScreenGraph Map = new ScreenGraph() {
          { "FrontDoor",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "LivingRoom1", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "LivingRoom2", Exit = "TopLeft" },
            })
          },
          { "LivingRoom1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "FrontDoor", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "LivingRoom2", Exit = "TopLeft" },
            })
          },
          { "LivingRoom2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "TopLeft", To = "", Exit = "Right" },
              new ScreenExit { Id = "TopRight", To = "LivingRoom3", Exit = "BottomLeft" },
              new ScreenExit { Id = "BottomLeft", To = "Laundry1", Exit = "Right" },
              new ScreenExit { Id = "BottomRight", To = "Kitchen1", Exit = "BottomLeft" },
            })
          },
          { "LivingRoom3",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "BottomLeft", To = "", Exit = "Right" },
              new ScreenExit { Id = "TopRight", To = "Hallway1", Exit = "Left" },
              new ScreenExit { Id = "BottomRight", To = "FrontDoor", Exit = "Left" }
            })
          },

          { "Kitchen1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "TopLeft", To = "FamilyRoom1", Exit = "Right" },
              new ScreenExit { Id = "TopRight", To = "Kitchen2", Exit = "TopLeft" },
              new ScreenExit { Id = "BottomLeft", To = "", Exit = "Right" },
            })
          },
          { "Kitchen2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Topleft", To = "", Exit = "Left" },
              new ScreenExit { Id = "TopRight", To = "FamilyRoom1", Exit = "Left" },
              new ScreenExit { Id = "BottomLeft", To = "", Exit = "Right" },
              new ScreenExit { Id = "BottomRight", To = "LivingRoom1", Exit = "Left" }
            })
          },

          { "FamilyRoom1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "FamilyRoom2", Exit = "BottomLeft" },
              new ScreenExit { Id = "Right", To = "", Exit = "TopLeft" },
            })
          },
          { "FamilyRoom2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "TopRight", To = "Kitchen2", Exit = "BottomLeft" },
              new ScreenExit { Id = "BottomLeft", To = "", Exit = "Left" },
              new ScreenExit { Id = "BottomRight", To = "", Exit = "Left" },
            })
          },


          { "Hallway1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "", Exit = "" },
              new ScreenExit { Id = "Right", To = "Hallway2", Exit = "Left" },
            })
          },
          { "Hallway2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "MBedroom1", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "Hallway3", Exit = "Left" },
            })
          },
          { "Hallway3",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "Bedroom1", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "Hallway4", Exit = "Left" },
            })
          },
          { "Hallway4",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "LivingRoom3", Exit = "BottomRight" },
              new ScreenExit { Id = "Right", To = "Hallway1", Exit = "Left" },
            })
          },

          { "Bedroom1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "Bedroom2", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "Hallway1", Exit = "Left" },
            })
          },
          { "Bedroom2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Right", To = "Hallway1", Exit = "Left" },
            })
          },

          { "MBedroom1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "MBedroom2", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "Hallway1", Exit = "Left" },
            })
          },
          { "MBedroom2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Right", To = "Hallway1", Exit = "Left" },
            })
          },


          { "Laundry1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "Laundry1", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "Laundry2", Exit = "Left" },
            })
          },
          { "Laundry2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "Laundry3", Exit = "Right" },
              new ScreenExit { Id = "Right", To = "Laundry2", Exit = "Left" },
            })
          },
          { "Laundry3",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "Laundry4", Exit = "TopRight" },
              new ScreenExit { Id = "Right", To = "", Exit = "" },
            })
          },
          { "Laundry4",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "BottomLeft", To = "Garage1", Exit = "TopRight" },
              new ScreenExit { Id = "TopRight", To = "", Exit = "BottomRight" },
            })
          },


          { "Garage1",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "TopRight", To = "", Exit = "" },
              new ScreenExit { Id = "BottomRight", To = "Garage2", Exit = "Left" },
            })
          },
          { "Garage2",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "FamilyRoom2", Exit = "BottomRight" },
              new ScreenExit { Id = "Right", To = "Garage3", Exit = "Left" },
            })
          },
          { "Garage3",
            new ScreenNode(new List<ScreenExit> {
              new ScreenExit { Id = "Left", To = "Garage2", Exit = "Right" },
            })
          },
        };
    }
}
