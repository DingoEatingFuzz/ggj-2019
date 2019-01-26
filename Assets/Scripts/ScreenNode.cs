using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScreenNode {
  public string Name;
  public List<ScreenExit> Exits;
  public ScreenNode(string name, List<ScreenExit> exits) {
    this.Name = name;
    this.Exits = exits;
  }

  public void exit(string exitName) {
    var exit = Exits.Find(e => e.Id == exitName);
    GameData.GoToScreen(exit.To, exit.Exit);
  }
}
