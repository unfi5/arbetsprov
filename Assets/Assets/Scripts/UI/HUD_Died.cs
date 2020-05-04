using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Died : MonoBehaviour
{
    public Text HealthText;
    private Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (_player.Health <= 0)
        {
            HealthText.text = "You Died";
        }
    }
}
