using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] private int _armorPoints;

 

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.gameObject.GetComponent<PlayerMover>();

        if (player != null)
        {
            player.AddArmor(_armorPoints);
            Destroy(gameObject);
        }
    }
}
