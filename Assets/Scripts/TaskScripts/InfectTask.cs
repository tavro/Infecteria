using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectTask : Task
{
    new InfectUnit unit;

    public InfectTask(InfectUnit owner, GameObject targetObject) : base((Unit)owner, targetObject) {
        taskType = TaskType.INFECT;
        unit = owner;
        target = targetObject;
    }    

    public override void Update(){
        if(target != null){
            if (!unit.IsMoving())
                unit.FollowTarget(target);
            
            if(unit.AtPosition(target.transform.position)){
                Infectable infectTarget = target.GetComponent<Infectable>();
                if(infectTarget != null && unit.CanInfect(infectTarget)){
                    unit.InfectTarget(infectTarget);
                }
                FinishTask();
            }
        }
        else {
            FinishTask();
        }
    } 
}
