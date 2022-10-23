using UnityBean;

public class Animal {
    [LazyWired] protected TestRepository repository;

    private int member1;

    protected Animal() {
        BeanContainer.LazyDI(this);
    }
}
