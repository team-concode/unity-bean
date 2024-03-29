# unity-bean
Unity bean is a lightweight dependency injection framework built specifically to target Unity 3D. There is a [Zenject](https://github.com/modesttree/Zenject) framework, but the injection part was a little inconvenient for me, so I made a new one.

If you are familiar with Java Spring's DI, you can use it right away. It provide some custom attributes which makes unity like JAVA Spring. Bean Container manage singleton componetns and it link automatically between component with 'AutoWired' Attribute.

### Attributes
* Controller
* Service
* Repository
* Module
* AutoWired (Also support Array Style)
* LazyWired (Also support Array Style)

### Sameple code 
```C#
using UnityBean;

[Repository]
public class TestRepository {
    public string ReadValue() {
        return "Hello";
    }
}
```

```C#
using UnityBean;

[Service]
public class TestService {
    [AutoWired] private TestRepository repository;

    public string GetValue() {
        return repository.ReadValue();
    }
}
```

```C#
using System.Threading.Tasks;
using UnityEngine;
using UnityBean;

[Controller]
public class TestController {
    [AutoWired] private TestService testService;

    // this will called while start up
    public async Task<bool> Initialize() {
        Test();
        return true;
    }

    private void Test() {
        Debug.Log(testService.GetValue());
    }
}
```

If you implement "public async Task<bool> Initialize()" method then the bean container will call that method automatically while initialize.
At the contstructor, you can not use the autowired fileld it still not linked yet at that time. You can use "Initialize()" method instead of constructor.


### Initialize
You need to initialize Bean Controller while starting the game.
```C#
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
```


### LazyWired
"AutoWired" attribute only works among the Beans. If you want to use the bean outside, you can use lazy DI.
```C#
using UnityEngine;
using UnityBean;

public class LazyWiredTest : MonoBehaviour {
    [LazyWired] private TestService testService;

    private void Start() {
        // All dependency with the LazyWired attribute injected here.
        BeanContainer.LazyDI(this); 
        Debug.Log(testService.GetValue());
    }
}
```

### Interface Array Injection
```C#
public interface UnitService {
    void Print();
}

[Service]
public class NpcService : UnitService {
    public void Print() {
        Debug.Log("Npc");
    }
}

[Service]
public class EnemyService : UnitService {
    public void Print() {
        Debug.Log("Enemy");
    }
}

[Service]
public class UnitServiceManager {
    // Bean Container link 'UnitService' inteface automatically
    // You can you this as a Factory or LookUpTable
    [AutoWired] private UnitService[] unitServices;

    public async Task<bool> Initialize() {
        foreach (var service in unitServices) {
            service.Print();
        }
        return true;
    }
}
```

