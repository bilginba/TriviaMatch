using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;

    void Awake()
    {
        GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>().text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }
}
