using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public GameObject gina;
    // Start is called before the first frame update
    void Start()
    {
        var player = Instantiate(gina, new Vector3(0, 0, 0), new Quaternion());
        player.name = "Gina";
        SceneManager.LoadScene("FrontDoor", LoadSceneMode.Additive);

        // var nextScene = SceneManager.GetSceneByName("FrontDoor");
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName("FrontDoor"));
        GameData.CurrentScene = "FrontDoor";
        StartCoroutine(GameData.SetActive("FrontDoor"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
