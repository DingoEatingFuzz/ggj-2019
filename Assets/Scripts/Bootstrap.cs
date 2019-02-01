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
        var startPos = new Vector3(154, 145, 0);
        var player = Instantiate(gina, startPos, new Quaternion());
        player.name = "Gina";
        PlayerControlScript cs = (PlayerControlScript)player.GetComponentInChildren(typeof(PlayerControlScript));
        cs.spawnPosition = startPos;
        SceneManager.LoadScene("FrontDoor", LoadSceneMode.Additive);

        Debug.Log("script: " + cs);
        cs.gameData = cs.gameObject.AddComponent<GameData>();

        // var nextScene = SceneManager.GetSceneByName("FrontDoor");
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByName("FrontDoor"));
        StartCoroutine(cs.gameData.SetActive("FrontDoor"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
