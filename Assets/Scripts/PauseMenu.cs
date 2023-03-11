using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string menuSceneName = "TowerDefenseMenu";
    public GameObject ui;
    public SceneFader sceneFader;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Toggle (bool menu=false)
    {
        ui.SetActive(!ui.activeSelf);
        if (ui.activeSelf)
        {
            foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
            {
                node.enabled = false;
            }
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
            foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
            {
                node.enabled = !menu;
            }
        }
    }

    public void Menu()
    {
        Toggle(true);
        Time.timeScale = 1f;
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
        PlayerStats.instance.StartSave();
        WaveSpawner.enemiesAlive = 0;
        sceneFader.FadeTo(menuSceneName);
    }
}
