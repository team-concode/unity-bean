using UnityEngine;

public class ManualLinkTest : MonoBehaviour {
    private TestService testService => UnityBean.BeanContainer.GetBean<TestService>();

    private void Start() {
        Debug.Log(testService.GetValue());
    }
}
