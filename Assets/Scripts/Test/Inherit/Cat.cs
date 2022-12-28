using UnityBean;
using UnityEngine;

public class Cat : Animal {
    [LazyWired] protected TestService service;

    public Cat() {
        BeanContainer.LazyDI(this);
    }
    
    public void Test() {
        Debug.Log("Cat1: " + repository.ReadValue());
        Debug.Log("Cat2: " + service.GetValue());
    }
}