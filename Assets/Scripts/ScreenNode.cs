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
    GameData.GoToScreen(exit.To, exit.Exit);
  }
}
