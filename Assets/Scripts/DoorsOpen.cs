using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorsOpen : MonoBehaviour
{
    [SerializeField] private int _coinsToNextLevel;
    [SerializeField] private int _levelToLoad;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _opensDoorsSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null && player.CoinsValue >= _coinsToNextLevel)
        {
            _spriteRenderer.sprite = _opensDoorsSprite;
            Invoke(nameof(LoadNextScene), 1f);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(_levelToLoad);
    }
}
