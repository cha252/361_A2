using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel1Script : MonoBehaviour
{
    public string level;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string[] scenesToLoad = { $"Level{level}_Base", $"Level{level}_Player", $"Level{level}_Items" };
        
        //Load the other parts of the level
        foreach(string scene in scenesToLoad)
        {
            //If the scene is not loaded yet
            if(!SceneManager.GetSceneByName(scene).isLoaded)
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
