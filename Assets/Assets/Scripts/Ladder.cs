using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{
    private Player _player;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if(_player == null)
            {
                _player = collision.transform.GetComponent<Player>();
            }

            _player.isClimbing = true;
            _player.rb.useGravity = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            _player.isClimbing = false;
            _player.rb.useGravity = true;
        }
    }
}
