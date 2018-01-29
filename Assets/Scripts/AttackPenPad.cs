﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPenPad : Item
{
    public float EffectDuration = 2f;
    public float Force = 50f;
    private Rigidbody2D myRig;
    public byte whoUse;
    public bool isShoot;
    private void Awake()
    {
        UseType = ItemUsedType.AttackAim;
        objItem = gameItem.penpad;
        myRig = GetComponent<Rigidbody2D>();
    }
    public override void UseItem(Vector2 direction, GameObject whoUse)
    {
        this.whoUse = whoUse.GetComponent<Player>().PlayerOrder;
        transform.position = whoUse.transform.position;
        Vector2 moveForce = new Vector2(direction.x, -direction.y) * Force;
        myRig.velocity = moveForce;
        isShoot = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isShoot)
            {
                Player player = collision.GetComponent<Player>();
                if (player.PlayerOrder != whoUse)
                {
                    player.setStatus(true, -2, EffectDuration);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Player player = collision.GetComponent<Player>();
                player.pickupObj = this.gameObject;
            }
        }
        else if (collision.CompareTag("Obtacle") && isShoot)
        {
            Destroy(this.gameObject);
        }
    }
}
