using UnityEngine;
using UnityBean;

public class DynamicWiredTest : MonoBehaviour {
    [LazyWired] private TestService testService;

    private void Start() {
        BeanContainer.LazyDI(this);
        Debug.Log(testService.GetValue());
    }
}
