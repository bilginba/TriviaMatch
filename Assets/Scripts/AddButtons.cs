using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AddButtons : MonoBehaviour
{
    [SerializeField]
    private Transform puzzlePanel;

    [SerializeField]
    private GameObject btn;

    private Info info;
    
    
    void Awake()
    {

        info = Resources.Load<Info>("Infos/" + SceneManager.GetActiveScene().name);
        int buttonCount = info.left_side.Count + info.right_side.Count;
        for(int i=0; i< buttonCount; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = i.ToString();
            button.transform.SetParent(puzzlePanel, false);
        }


        puzzlePanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80, 40);
        puzzlePanel.GetComponent<GridLayoutGroup>().constraintCount = 2;
        puzzlePanel.GetComponent<GridLayoutGroup>().spacing = new Vector2(40, 20);

        
         
        
    }

    
}
