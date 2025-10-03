using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowSettingsScript : MonoBehaviour
{

    public KeyCode settingsKey = KeyCode.M;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(settingsKey))
        {
            SceneManager.LoadScene("Settings"); // or use the scene index
        }
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}