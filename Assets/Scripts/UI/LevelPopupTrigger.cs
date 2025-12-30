using UnityEngine;

public class LevelPopupTrigger : MonoBehaviour
{
    [SerializeField] private GameObject levelsPopupGO;
    [SerializeField] private Collider2D trigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != trigger)
        {
            levelsPopupGO.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != trigger)
        {
            levelsPopupGO.SetActive(false);
        }
    }
}
