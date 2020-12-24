using System.Threading.Tasks;

[Repository]
public class TestRepository {
    public async Task<bool> Initialize() {
        return true;
    }

    public string ReadValue() {
        return "Hello";
    }
}
