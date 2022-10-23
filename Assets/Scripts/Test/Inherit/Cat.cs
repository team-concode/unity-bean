using UnityBean;
using UnityEngine;

public class Cat : Animal {
    [LazyWired] protected TestService service;

    public Cat() {
        BeanContainer.LazyDI(this);
    }
    
    public void Test() {
        Debug.LogError("Cat1: " + repository.ReadValue());
        Debug.LogError("Cat2: " + service.GetValue());
    }
}