using UnityEngine;
using UnityEngine.SceneManagement;

public class goalScript : MonoBehaviour
{
    //String for level no.
    public string nextLevel;

    //Method to load the next level 
    void OnTriggerEnter()
    {
        SceneManager.LoadScene($"Level{nextLevel}_Loader", LoadSceneMode.Single);
    }
}
