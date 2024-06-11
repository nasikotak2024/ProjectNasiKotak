using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerInScene();
    }

    private void FindPlayerInScene()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    public void SaveGame()
    {
        if (player != null)
        {
            PlayerPrefs.SetFloat("PlayerX", player.position.x);
            PlayerPrefs.SetFloat("PlayerY", player.position.y);
            PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();
            Debug.Log("Game Saved!");
        }
        else
        {
            Debug.LogError("Player reference is missing.");
        }
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("CurrentScene"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            string sceneName = PlayerPrefs.GetString("CurrentScene");

            StartCoroutine(LoadSceneAndSetPosition(sceneName, new Vector2(x, y)));
            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.LogError("No save data found!");
        }
    }

    private IEnumerator LoadSceneAndSetPosition(string sceneName, Vector2 position)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        FindPlayerInScene();
        if (player != null)
        {
            player.position = position;
        }
    }
}
