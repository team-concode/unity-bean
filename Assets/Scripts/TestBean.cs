using UnityEngine;

public class TestBean : MonoBehaviour {
    private async void Start() {
        await BeanContainer.Initialize(OnBeanStart, OnBeanSuccess, OnBeanFailed);
    }

    private void OnBeanStart(string name) {
        Debug.Log("Starting " + name);
    }

    private void OnBeanSuccess(string name) {
        Debug.Log("Starting " + name + " Succeed.");
    }

    private void OnBeanFailed(string name) {
        Debug.LogError("Starting " + name + " Failed!");
    }

    public void DisplayBeanState(string key) {
    }
}
