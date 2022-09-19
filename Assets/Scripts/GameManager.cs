using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static HashSet<string> completedLevels = new HashSet<string>();
    public static Tilemap floor;

    private static GameManager instance = null;
    [SerializeField] private GameObject congratulations;


    private void Start()
    {
        instance = this;

        floor = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Restart();
    }

    private static void Restart()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void Finish()
    {
        Debug.Log("Completed Level!");
        completedLevels.Add(SceneManager.GetActiveScene().name);
        instance.StartCoroutine(LoadNextScene());
        instance.congratulations.SetActive(true);
    }

    private static IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Overworld");
    }
}
