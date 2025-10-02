using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGamplay()
    {
        SceneManager.LoadScene(Scenes.TestLevel1Textures_Updated.ToString(), LoadSceneMode.Single);
    }
}

public enum Scenes
{
    MainMenu,
    TestLevel1Textures_Updated,
    Ending
}