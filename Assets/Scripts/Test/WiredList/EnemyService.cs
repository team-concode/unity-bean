using UnityBean;
using UnityEngine;

[Service]
public class EnemyService : UnitService {
    public void Print() {
        Debug.Log("Enemy");
    }
}
