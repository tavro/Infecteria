using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTask : Task
{
    Vector2 offset;

    public FollowTask(Unit owner, GameObject targetObject, Vector2 targetOffset) : base(owner, targetObject) {
        taskType = TaskType.FOLLOW;
        offset = targetOffset;
    }

    public override void Update(){
        if(target != null){
            unit.FollowTarget(target, offset);
        }
        else {
            FinishTask();
        }
    }    
}
