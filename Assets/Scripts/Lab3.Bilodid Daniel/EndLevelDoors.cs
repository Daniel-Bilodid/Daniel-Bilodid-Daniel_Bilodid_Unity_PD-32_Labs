using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelDoors : MonoBehaviour
{
    [SerializeField] private int _coinsToNextLevel;
    [SerializeField] private int _levelToLoad;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _openDoorsSprite;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null && player.CoinsAmount >= _coinsToNextLevel)
        {
            Debug.Log("Doors openned");
            _spriteRenderer.sprite = _openDoorsSprite;
            Invoke(nameof(loadNextScene), 1f);  
        }
    }
    private void loadNextScene()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
