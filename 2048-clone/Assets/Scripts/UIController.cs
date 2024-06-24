using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private GameObject gameoverParent;
    [SerializeField] private Button retryBtn;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        board.gameovered += () => gameoverParent.SetActive(true);
        retryBtn.onClick.AddListener(board.RestartGame);
        board.score.changed += (v) => scoreText.text = v.ToString();
    }
}