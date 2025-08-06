using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Home : MonoBehaviour
{
    public GameObject frog;

    private BoxCollider2D boxCollider;
    [field:SerializeField]public bool IsOccuped { get; private set; } = false;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Occupy()
    {
        if (IsOccuped) return;
        IsOccuped = true;
        frog.SetActive(true);
        boxCollider.enabled = false;
        MiniGameManager.Instance.HomeOccupied();
    }

    public void ResetHome()
    {
        IsOccuped = false;
        frog.SetActive(false);
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Occupy();
        }
    }

}
