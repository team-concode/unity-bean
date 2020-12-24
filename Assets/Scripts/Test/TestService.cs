using UnityBean;

[Service]
public class TestService {
    [AutoWired] private TestRepository repository;

    public string GetValue() {
        return repository.ReadValue();
    }
}