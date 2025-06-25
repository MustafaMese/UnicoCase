using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneService : ISceneService
{
    private GameServices _service;

    public Task Initialize(GameServices services)
    {
        _service = services;

        _service.RegisterService<ISceneService, SceneService>(this);

        return Task.CompletedTask;
    }

    public void ChangeScene(bool next)
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + (next ? 1 : 0);
        SceneManager.LoadScene(nextSceneIndex % SceneManager.sceneCountInBuildSettings);
    }

    public int CurrentScene => SceneManager.GetActiveScene().buildIndex;
}

public interface ISceneService
{
    Task Initialize(GameServices services);
}