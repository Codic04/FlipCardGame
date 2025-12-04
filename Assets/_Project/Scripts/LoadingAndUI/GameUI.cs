using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject endLevelPanel;

    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text comboTxt;
    [SerializeField] private Text turnTxt;

    private void Start()
    {
        endLevelPanel.SetActive(false);

        UpdateCombo("1");
        UpdateScore("0");
        UpdateTurn("0");
    }

    public void EndLevel()
    {
        endLevelPanel.SetActive(true);
    }

    public void StartNewLevel()
    {
        LoadingHandler.Instance.LoadScene("Game");
    }

    public void UpdateTurn(string value)
    {
        turnTxt.text = $"Turn: {value}";
    }
    public void UpdateScore(string value)
    {
        scoreTxt.text = $"Score: {value}";
    }
    public void UpdateCombo(string value)
    {
        comboTxt.text = $"Combo: {value}";
    }
}
