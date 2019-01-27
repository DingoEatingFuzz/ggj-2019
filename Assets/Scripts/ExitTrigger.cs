using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public string ExitName;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControlScript cs = (PlayerControlScript)collision.gameObject.GetComponent("PlayerControlScript");
            var sceneName = SceneManager.GetActiveScene().name;

            Debug.Log("ExitTrigger triggered in " + sceneName);
            var node = GameData.InnerDerp.Map[sceneName];
            node.exit(ExitName);
        }

    }
}
