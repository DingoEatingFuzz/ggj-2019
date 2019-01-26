using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public string ScreenName;
    public string ExitName;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        var node = GameData.Map[ScreenName];
        node.exit(ExitName);
    }
}
