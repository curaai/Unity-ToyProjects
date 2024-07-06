using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameoverPanel;
    [SerializeField] private Field field;
    [SerializeField] private Button retryBtn;

    private void Start()
    {
        retryBtn.onClick.AddListener(field.RestartGame);
        field.gameovered += () => gameoverPanel.SetActive(true);
    }
}