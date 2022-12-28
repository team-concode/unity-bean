using UnityEngine;
using UnityBean;

public class LazyWiredTest : MonoBehaviour {
    [LazyWired] private TestService testService;

    private void Start() {
        BeanContainer.LazyDI(this);
        Debug.Log(testService.GetValue());
    }
}
