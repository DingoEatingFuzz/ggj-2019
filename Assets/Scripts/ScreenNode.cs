using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScreenNode {
    public List<ScreenExit> Exits;
    public ScreenNode(List<ScreenExit> exits) {
        this.Exits = exits;
    }

    public void exit(string exitName) {
        var exit = Exits.Find(e => e.Id == exitName);
        if (exit == null) {
            Debug.Log("You idiot " + exitName + " -----" );
        }

        var gina = GameObject.Find("Gina");
        var cs = gina.GetComponentInChildren<PlayerControlScript>();
        cs.gameData.GoToScreen(exit.To, exit.Exit);
    }
}
