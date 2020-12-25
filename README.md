# unity-bean

It provide some custom attributes which makes unity like JAVA Spring. 
Bean Container manage singleton componetns and it link automatically between component with 'AutoWired' Attribute.

### Attributes
* Controller
* Service
* Repository
* AutoWired
* DynamicWired

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


### DynamicWired
"AutoWired" attribute only works among the Beans. If you want to use the bean outside, you can use dynamic DI.
```C#
using UnityEngine;
using UnityBean;

public class DynamicWiredTest : MonoBehaviour {
    [DynamicWired] private TestService testService;

    private void Start() {
        BeanContainer.DynamicDI(this); // dynamic wire
        Debug.Log(testService.GetValue());
    }
}
```
