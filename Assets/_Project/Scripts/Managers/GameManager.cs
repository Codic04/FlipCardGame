using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => instance;
    private static GameManager instance;

    public int TargetPicsNumber => targetPicsNumber;
    private int targetPicsNumber = 3;

    public int TargetColuomnNumber => targetColuomnNumber;
    private int targetColuomnNumber = 3;

    public int TargetRowsNumber => targetRowsNumber;
    private int targetRowsNumber;

    public int HighestScore => hightestScore;
    private int hightestScore;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);


        var data = SaveSystem.LoadData();
        hightestScore = data.highScore;
    }

    public void SetTargetPicsNumber(string targetPicsNumber)
    {
        this.targetPicsNumber = int.Parse(targetPicsNumber);
    }
    public void SetTargetColumn(string targetColumn)
    {
        targetColuomnNumber = int.Parse(targetColumn);
    }
    public void SetTargetRows(string targetRows)
    {
        targetRowsNumber = int.Parse(targetRows);
    }
}
