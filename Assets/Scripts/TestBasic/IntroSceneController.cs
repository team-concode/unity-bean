using UnityEngine;

public class IntroSceneController : MonoBehaviour {
    private async void Awake() {
        await UnityBean.BeanContainer.Initialize((bean) => {
            Debug.Log("Starting " + bean);
        }, (bean) => {
            Debug.Log("Success starting " + bean);
        }, (bean) => {
            Debug.LogError("Failed starting " + bean);
        });
    }
}
