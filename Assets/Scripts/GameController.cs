using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Sprite buttonImg = null;
    [SerializeField]
    private Sprite pressedBtnImg = null;
    private Info info;

    public List<Dictionary<string, int>> puzzleValues;

    public List<Button> btns = new List<Button>();
    [SerializeField]
    private LineRenderer lineRenderer;
    private Vector3 leftPos;
    private Vector3 rightPos;

    private bool firstGuess, secondGuess;
    private int firstGuessIndex, secondGuessIndex;
    private int firstSelectionID, secondSelectionID;
    void Awake()
    {
        info = Resources.Load<Info>("Infos/" + SceneManager.GetActiveScene().name);
    }
    void Start()
    {
        lineRenderer = FindObjectOfType<Canvas>().GetComponent<LineRenderer>();
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
        for (int i = 0; i < info.right_side.Count; i++)
        {
            int rnd = Random.Range(0, info.right_side.Count);
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
        if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
        {
            string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            if (!firstGuess)
            {
                firstGuess = true;
                firstGuessIndex = int.Parse(name);
                firstSelectionID = info.infoContents[btns[firstGuessIndex].GetComponent<Button>().GetComponentInChildren<Text>().text];
                //origin = btns[firstGuessIndex].transform;
                leftPos = new Vector3(btns[firstGuessIndex].transform.position.x, btns[firstGuessIndex].transform.position.y, btns[firstGuessIndex].transform.position.z);

            }
            else if (!secondGuess)
            {
                secondGuess = true;
                secondGuessIndex = int.Parse(name);
                secondSelectionID = info.infoContents[btns[secondGuessIndex].GetComponent<Button>().GetComponentInChildren<Text>().text];
                StartCoroutine(CheckMatches());
                //destination = btns[secondGuessIndex].transform;
                rightPos = new Vector3(btns[secondGuessIndex].transform.position.x, btns[secondGuessIndex].transform.position.y, btns[secondGuessIndex].transform.position.z);
            }
        }
    }

    IEnumerator CheckMatches()
    {
        yield return new WaitForSeconds(0f);
        if (firstSelectionID == secondSelectionID)
        {
            if(btns[firstGuessIndex].name != btns[secondGuessIndex].name)
            {
                SpriteState firstSpriteState = new SpriteState();
                SpriteState secondSpriteState = new SpriteState();
                btns[firstGuessIndex].interactable = false;
                firstSpriteState = btns[firstGuessIndex].spriteState;
                firstSpriteState.pressedSprite = pressedBtnImg;
                btns[secondGuessIndex].interactable = false;
                secondSpriteState = btns[firstGuessIndex].spriteState;
                secondSpriteState.pressedSprite = pressedBtnImg;

                lineRenderer.SetPosition(0, leftPos);
                lineRenderer.SetPosition(1, rightPos);
                lineRenderer.startWidth = 0.2f;

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
            if (btns[i].interactable == true)
            {
                return;
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
