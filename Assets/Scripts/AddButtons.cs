using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        int buttonCount = info.left_side.Length + info.right_side.Length;
        for(int i=0; i< buttonCount; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = i.ToString();
            button.transform.SetParent(puzzlePanel, false);
        }


        if(buttonCount < 10)
        {
            puzzlePanel.GetComponent<GridLayoutGroup>().constraintCount = 2;
        }
        else if(buttonCount >= 10)
        {
            puzzlePanel.GetComponent<GridLayoutGroup>().constraintCount = 3;
        }
        
    }

    
}
