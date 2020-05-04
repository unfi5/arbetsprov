using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Health : MonoBehaviour
{    
    public Text HealthText;
    private Player _player;
    private int _Health;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (_Health != _player.Health)
        {
            _Health = _player.Health;
            HealthText.text = _player.Health.ToString();
        }
    }
}
