using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Image Player1Health;
    public Image Player2Health;
    public GameObject PlayerUI;
    public GameObject Player1HealthBehaviour;
    public GameObject Player2HealthBehaviour;
    public IntReference currentScene;
    [SerializeField] private HealthSprites _player1Sprites;
    [SerializeField] private HealthSprites _player2Sprites;
    [SerializeField] private HealthSpriteObject _player1Holder;
    [SerializeField] private HealthSpriteObject _player2Holder;


    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnEnable()
    {
        _player1Holder.sprites = new HealthSpritesHolder(_player1Sprites.highHealth, _player1Sprites.lowHeath, _player1Sprites.midHealth,
            _player1Sprites.healthBar, ref _player1Sprites.currentSprite);
        _player2Holder.sprites = new HealthSpritesHolder(_player2Sprites.highHealth, _player2Sprites.lowHeath, _player2Sprites.midHealth, _player2Sprites.healthBar, ref _player2Sprites.currentSprite);
    }
    // Update is called once per frame
    void Update()
    {
        if (!EntityHealthBehaviour.player1.IsAlive && !EntityHealthBehaviour.player2.IsAlive)
        {
            currentScene.value = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene("LevelFailed");
        }

    }
    [System.Serializable]
    private struct HealthSprites
    {
        public Image lowHeath;
        public Image highHealth;
        public Image midHealth;
        public Image healthBar;
        public Image currentSprite;
    }
}
