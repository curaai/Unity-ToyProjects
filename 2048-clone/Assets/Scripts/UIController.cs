using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private GameObject gameoverParent;
    [SerializeField] private Button retryBtn;

    private void Start()
    {
        board.gameovered += () => gameoverParent.SetActive(true);
        retryBtn.onClick.AddListener(board.RestartGame);

        // TODO: Score UI
    }
}