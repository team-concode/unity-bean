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
[Controller]
public class TestController {
    [AutoWired] 
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
[Service]
public class TestService {
    [AutoWired] 
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
[Repository]
public class TestRepository {
    public async Task<bool> Initialize() {
        return true;
    }

    public string ReadValue() {
        return "Hello";
    }
}
```

### Initialize
You need to initialize Bean Controller while starting the game.
```C#
public class IntroSceneController : MonoBehaviour {
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
}
```
