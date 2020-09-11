using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text levelText;

    void Awake()
    {
        GameObject.Find("LevelText").GetComponent<Text>().text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }

    
}
