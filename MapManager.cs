using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject mapPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
    }

    public void ToggleMap()
    {
        if (mapPanel == null) return;

        bool isActive = mapPanel.activeSelf;
        mapPanel.SetActive(!isActive);
        Time.timeScale = !isActive ? 0f : 1f;
    }
}