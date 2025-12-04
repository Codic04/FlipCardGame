using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private Transform gridContainer;

    [SerializeField] private GameObject cardPrefab;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private float checkMatchDelay;
    [SerializeField] private float closeDelay;
    [SerializeField] private int picsNumber;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip cardsMatchSound;
    [SerializeField] private AudioClip cardsMisMatchSound;
    [SerializeField] private AudioClip endlevelSound;

    private List<CardObject> selectedPics = new List<CardObject>();

    private int trueMatches = 0;

    private void Start()
    {
        picsNumber = GameManager.Instance.TargetPicsNumber;
        gridContainer.GetComponent<GridLayoutGroup>().constraintCount = GameManager.Instance.TargetColuomnNumber;

        InitalizePicsGrid();
    }

    private void InitalizePicsGrid()
    {
        picsNumber = picsNumber > sprites.Length ? sprites.Length : picsNumber;
        List<Sprite> targetPics = new List<Sprite>();
        List<Sprite> tempPics = sprites.ToList();

        for (int i = 0; i < picsNumber; i++)
        {
            int randPicIndex = Random.Range(0, tempPics.Count);
            targetPics.Add(tempPics[randPicIndex]);
            tempPics.RemoveAt(randPicIndex);
        }

        targetPics.AddRange(targetPics);

        tempPics.Clear();
        tempPics.AddRange(targetPics);

        for (int i = 0; i < targetPics.Count; i++)
        {
            var imgObj = Instantiate(cardPrefab, gridContainer);

            int randPic = Random.Range(0, tempPics.Count);

            imgObj.GetComponent<CardObject>().Initialize(tempPics[randPic], closeDelay);
            imgObj.GetComponent<CardObject>().OnCardClicked += OnCardClicked;

            tempPics.RemoveAt(randPic);
        }

        tempPics = null;
        targetPics = null;

        ValidateGridContent();
    }

    private void ValidateGridContent()
    {
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForEndOfFrame();
            Debug.Log($"{gridContainer.GetComponent<RectTransform>().rect.height},{Screen.height}");
            if (gridContainer.GetComponent<RectTransform>().rect.height > Screen.height)
            {
                Debug.Log("Larger");
                //Height
                float gridHeight = gridContainer.GetComponent<RectTransform>().rect.height;
                float heightDifference = gridHeight - Screen.height;

                float rowsNum = gridContainer.childCount / gridContainer.GetComponent<GridLayoutGroup>().constraintCount;
                int rowsCount = Mathf.CeilToInt(rowsNum);

                int heightToModify = Mathf.CeilToInt(heightDifference / rowsCount);


                gridContainer.GetComponent<GridLayoutGroup>().cellSize -= new Vector2(0, heightToModify);

            }
        }
    }

    private void OnCardClicked(CardObject targetPic)
    {
        selectedPics.Add(targetPic);

        if (selectedPics.Count % 2 == 0)
        {
            LevelManager.Instance.IncreaseTurn();
            StartCoroutine(CheckCardMatch());
        }
    }

    IEnumerator CheckCardMatch()
    {
        yield return new WaitForSeconds(checkMatchDelay);

        if (selectedPics[0].TargetPic == selectedPics[1].TargetPic)
        {
            selectedPics[0].OnCardMatch();
            selectedPics[1].OnCardMatch();

            audioSource.PlayOneShot(cardsMatchSound);
            LevelManager.Instance.IncreaseScore();
            trueMatches++;

            if (trueMatches == picsNumber)
            {
                audioSource.PlayOneShot(endlevelSound);
                LevelManager.Instance.EndLevel();
            }
        }
        else
        {
            selectedPics[0].CloseCard();
            selectedPics[1].CloseCard();

            audioSource.PlayOneShot(cardsMisMatchSound);
            LevelManager.Instance.ResetCombo();
        }

        selectedPics.RemoveAt(0);
        selectedPics.RemoveAt(0);
    }
}
