using UnityBean;
using UnityEngine;

[Service]
public class NpcService : UnitService {
    public void Print() {
        Debug.Log("Npc");
    }
}