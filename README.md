# unity-bean

It provide some custom attributes which makes unity like JAVA Spring. 
Bean Container manage singleton componetns and it link automatically between component with 'AutoWired' Attribute.

### Attributes
* Controller
* Service
* Repository
* Component 
* AutoWired

### Sameple code 
```C#
[UnityBean.Controller]
public class TestController {
    [UnityBean.AutoWired] 
    private TestService testService;

    public async Task<bool> Initialize() {
        Test();
        return true;
    }

    private void Test() {
        Debug.Log(testService.GetValue());
    }
}
```

```C#
[UnityBean.Service]
public class TestService {
    [UnityBean.AutoWired] 
    private TestRepository repository;
    
    public async Task<bool> Initialize() {
        return true;
    }

    public string GetValue() {
        return repository.ReadValue();
    }
}
```

```C#
[UnityBean.Repository]
public class TestRepository {
    public async Task<bool> Initialize() {
        return true;
    }

    public string ReadValue() {
        return "Hello";
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


### Manual Link
"AutoWired" attribute only works between the Bean. If you want to use the bean out side, you could use manual link mehtod.
```
public class ManualLinkTest : MonoBehaviour {
    private TestService testService => UnityBean.BeanContainer.GetBean<TestService>();

    private void Start() {
        Debug.Log(testService.GetValue());
    }
}
```
