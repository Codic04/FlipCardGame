using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance => instance;
    private static LevelManager instance;

    [SerializeField] private GameUI gameUI;

    private int continuesCardMatch;
    private int curCombo = 1;
    private int curScore;
    private int curTurn;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void OnDisable()
    {
        int targetScore = curScore > GameManager.Instance.HighestScore ? curScore : GameManager.Instance.HighestScore;
        SaveData data = new SaveData(targetScore);
        SaveSystem.SaveData(data);
    }

    public void EndLevel()
    {
        gameUI.EndLevel();
    }

    public void IncreaseScore()
    {
        continuesCardMatch++;
        if (continuesCardMatch % 2 == 0) // Every Two right Match increase combo
            curCombo++;

        curScore += curCombo;

        gameUI.UpdateCombo(curCombo.ToString());
        gameUI.UpdateScore(curScore.ToString());
    }
    public void IncreaseTurn()
    {
        curTurn++;

        gameUI.UpdateTurn(curTurn.ToString());
    }
    public void ResetCombo()
    {
        continuesCardMatch = 0;
        curCombo = 1;

        gameUI.UpdateCombo(curCombo.ToString());
    }
}
