using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
   [SerializeField] private int _coinsAmount;
   

   private void OnTriggerEnter2D(Collider2D other)
   {
      PlayerMover player = other.GetComponent<PlayerMover>();
      if (player != null)
      {
         player.CoinsValue += _coinsAmount;
         player.CoinsValue = _coinsAmount;
         Destroy(gameObject);
      }
   }
}
