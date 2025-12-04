using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Text scoreTxt;

    [SerializeField] private InputField targetPicsField;
    [SerializeField] private InputField targetColoumnField;

    private void Start()
    {
        scoreTxt.text = $"Best Score: {GameManager.Instance.HighestScore.ToString()}";

        targetPicsField.SetTextWithoutNotify($"{GameManager.Instance.TargetPicsNumber}");
        targetColoumnField.SetTextWithoutNotify($"{GameManager.Instance.TargetColuomnNumber}");
    }

    public void StartGame()
    {
        LoadingHandler.Instance.LoadScene("Game");
        Debug.Log("Load Game Scene");
    }
}
