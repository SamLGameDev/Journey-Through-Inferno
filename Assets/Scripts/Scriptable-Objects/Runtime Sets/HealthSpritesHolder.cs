using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSpritesHolder : MonoBehaviour
{
    private Image _highHealth;
    public Image HighHealth { get { return _highHealth; } }

    private Image _lowHealth;
    public Image LowHealth { get { return _lowHealth; } }

    private Image _midHealth;
    public Image MidHealth { get { return _midHealth; } }

    private Image _healthBar;
    public Image HealthBar { get { return _healthBar; } }

    private Image _healthRenderer;
    public Image currentSprite { get { return _healthRenderer; } }

    private Image _petrified;
    public Image Petrified { get { return _petrified; } }


    public HealthSpritesHolder(Image highHealth, Image lowHealth, Image midHealth, Image petrified, Image heathBar, ref Image currentSprite)
    {
        _highHealth = highHealth;
        _lowHealth = lowHealth;
        _midHealth = midHealth;
        _healthBar = heathBar;
        _petrified = petrified;
        _healthRenderer = currentSprite;

    }

}