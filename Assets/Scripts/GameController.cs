using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite backgroundImg;

    private Info info;

    public List<Dictionary<string, int>> puzzleValues;

    public List<Button> btns = new List<Button>();

    private int guessCount;
    private int countCorrect;
    private int gameGuesses;

    private bool firstGuess, secondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private int firstSelectionID, secondSelectionID;
    void Awake()
    {
        info = Resources.Load<Info>("Infos/" + SceneManager.GetActiveScene().name);
    }
    void Start()
    {
        GetButtons();
        AddButtonListeners();
        info.AddInfos();
        LoadPuzzleContent();
    }

    void GetButtons()
    {
        GameObject[] gameObjs = GameObject.FindGameObjectsWithTag("PuzzleButton");
        for(int i=0; i<gameObjs.Length; i++)
        {
            btns.Add(gameObjs[i].GetComponent<Button>());
            btns[i].image.sprite = backgroundImg;
        }
    }

    void AddButtonListeners()
    {
        foreach(Button btn in btns)
        {
            btn.onClick.AddListener(() => SelectButton());
        }
    }

    void LoadPuzzleContent()
    {
        string tempInfo;
        for (int i = 0; i < info.right_side.Length; i++)
        {
            int rnd = Random.Range(0, info.right_side.Length);
            tempInfo = info.right_side[rnd];
            info.right_side[rnd] = info.right_side[i];
            info.right_side[i] = tempInfo;
        }
        int halfBtnCount = btns.Count / 2;
        for (int i = 0; i < btns.Count; i++)
        {
            if(i < halfBtnCount)
            {
                btns[i].GetComponent<Button>().GetComponentInChildren<Text>().text = info.left_side[i];
            }
            else
            {
                btns[i].GetComponent<Button>().GetComponentInChildren<Text>().text = info.right_side[i - halfBtnCount];
            }
            
        }
    }

    public void SelectButton()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(name);
            firstSelectionID = info.infoContents[btns[firstGuessIndex].GetComponent<Button>().GetComponentInChildren<Text>().text];
        }
        else if (!secondGuess)
        {
            guessCount++;
            secondGuess = true;
            secondGuessIndex = int.Parse(name);
            secondSelectionID = info.infoContents[btns[secondGuessIndex].GetComponent<Button>().GetComponentInChildren<Text>().text];
            StartCoroutine(CheckMatches());
        }
    }

    IEnumerator CheckMatches()
    {
        yield return new WaitForSeconds(0f);
        if (firstSelectionID == secondSelectionID)
        {
            if(btns[firstGuessIndex].name != btns[secondGuessIndex].name)
            {
                btns[firstGuessIndex].interactable = false;
                btns[firstGuessIndex].GetComponent<Image>().color = Color.green;
                btns[secondGuessIndex].interactable = false;
                btns[secondGuessIndex].GetComponent<Image>().color = Color.green;

                isGameFinished();
            }
        }
        else
        {
            Color initColor = btns[firstGuessIndex].GetComponent<Image>().color;
            btns[firstGuessIndex].GetComponent<Image>().color = Color.red;
            btns[secondGuessIndex].GetComponent<Image>().color = Color.red;

            yield return new WaitForSeconds(1f);

            btns[firstGuessIndex].GetComponent<Image>().color = initColor;
            btns[secondGuessIndex].GetComponent<Image>().color = initColor;
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        firstGuess = false;
        secondGuess = false;
        yield return new WaitForSeconds(.5f);
        
    }


    private void isGameFinished()
    {
        for (int i = 0; i < btns.Count; i++)
        {
            if (btns[i].GetComponent<Image>().color != Color.green)
            {
                return;
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
