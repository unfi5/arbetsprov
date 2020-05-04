using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderFolded : MonoBehaviour
{
    [SerializeField] GameObject _lowerLadder;

    private Player _player;
    private Text _helpText;
    private bool _folded = true;
    private bool _playerInside;
    private bool _eButtonDown;

    private void Update()
    {
        if (!_folded) return;
        if (_playerInside)
        {
            _helpText.text = "[E] to lower ladder";
            GetInput();
        }
    }

    void GetInput()
    {
        _eButtonDown = Input.GetKeyDown(KeyCode.E);
        if (_eButtonDown) StartCoroutine(LowerLaddder());
    }


    IEnumerator LowerLaddder()
    {
        _folded = false;
        _helpText.text = "";
        float y = 0;
        while (y < 2.75f)
        {
            yield return new WaitForSeconds(0.01f);
            _lowerLadder.transform.position = new Vector3(_lowerLadder.transform.position.x, _lowerLadder.transform.position.y - 0.125f, _lowerLadder.transform.position.z);
            y += 0.125f;
        }       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_folded && other.gameObject.layer == 9)
        {
            if (_player == null || _helpText == null)
            {
                _player = other.transform.GetComponent<Player>();
                _helpText = GameObject.Find("HelpSubtitles").GetComponent<Text>();
            }

            _playerInside = true;
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_folded && other.gameObject.layer == 9)
        {
            _playerInside = false;
            _helpText.text = "";
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 && !_folded)
        {
            if (_player == null)
            {
                _player = collision.transform.GetComponent<Player>();
            }

            _player.isClimbing = true;
            _player.rb.useGravity = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9 && !_folded)
        {
            _player.isClimbing = false;
            _player.rb.useGravity = true;
        }
    }
}
