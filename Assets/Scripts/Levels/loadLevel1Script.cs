using UnityEngine;
using UnityEngine.SceneManagement;

public class loadLevel1Script : MonoBehaviour
{
    private string[] scenesToLoad = {"Level1_Base", "Level1_Player", "Level1_Items"};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
