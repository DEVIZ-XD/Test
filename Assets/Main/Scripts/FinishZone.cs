using UnityEngine;

public class FinishZone : MonoBehaviour
{
    [SerializeField] private GameObject winMenuUI;
    private string playerTag = "Player";

    private bool finished;

    private void Start()
    {
        if (winMenuUI) winMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (finished) return;
        if (!other.CompareTag(playerTag)) return;

        finished = true;

        Time.timeScale = 0f;
        if (winMenuUI) winMenuUI.SetActive(true);
    }
}
