using UnityEngine;
using UnityBean;

public class DynamicWiredTest : MonoBehaviour {
    [DynamicWired] private TestService testService;

    private void Start() {
        BeanContainer.DynamicDI(this);
        Debug.Log(testService.GetValue());
    }
}
