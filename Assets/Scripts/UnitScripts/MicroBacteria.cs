using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroBacteria : Unit
{
    public Factory parent;

    public bool followPlayer;
    private GameObject player;

    public bool isSelected;
    public Cell targetCell;
    public Vector2 startPosition;

    void Start() {
        player = GameObject.Find("Player");
    }

    new void Update()
    {
        base.Update();
        if(followPlayer && !targetCell) {
            FollowTarget(player);
        }
        else if(isSelected) {
            if(targetCell && !unitMovement.moving) {
                GiveTask(new MoveTask(this, targetCell.gameObject));
            }
        }
    }

    public override void OnReachedDestination(GameObject target){
        if(targetCell != null){
            if (target != null && target == targetCell.gameObject){
                targetCell.Infect(0.2f);
                parent.RemoveMicrobacteria(this);
                Destroy(gameObject);
            }
        }
    }

    public void ToggleSelection() {
        isSelected = !isSelected;
        GameObject child = transform.GetChild(0).gameObject;
        child.SetActive(!child.activeSelf);
    }

    public override void OnDeath() {
        parent.RemoveMicrobacteria(this);
    }
}
