using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{   
    public enum Faction { BACTERIA, IMMUNESYSTEM, NEUTRAL };
    public Transform healthBar;
    public UnitMovement unitMovement;
    public List<GameObject> inRange;
    public UnitSquad squad;
    public UnitStats stats;
    public UnitType unitType;
    public UnitProducer producer;

    public Faction owner;
    public List<Task> currentTasks = new List<Task>();
    public Task proximityHostile;
    public GameObject hitEffect;
    public bool isSelected;
    public Vector2 hostileDetectionPos;
    public float maxHPBar;

    public List<AudioClip> hitSoundEffects;

    GameObject healthBarBackground = null;

    protected void Start(){
        maxHPBar = healthBar.localScale.x;
        if(unitMovement != null){
            for(int i = 0; i < healthBar.parent.childCount; i++) {
                if(healthBar.parent.GetChild(i).gameObject.name == "healthBarBackground")
                    healthBarBackground = healthBar.parent.GetChild(i).gameObject;
            }

            if(healthBarBackground){
                healthBar.gameObject.SetActive(false);
                healthBarBackground.SetActive(false);
            }
        }
    }

    public void SetUnitStats(int hp, int currentHp, int dmg, int speed, float time, float r, bool state) {
        stats = new UnitStats(hp, currentHp, dmg, speed, time, r, state);
    }

    protected void Update(){
        if(PauseManager.Instance.CurrPauseState == PauseManager.PauseState.NONE) {
            if (stats.IsAggressive()){
                if(!CanAttack())
                    stats.DecreaseAttackTimer(Time.deltaTime);
                FindHostileInProximity();
            }
            UpdateCurrentTask();
        }
    }

    public TaskType GetTaskType(){
        TaskType currentTaskType = TaskType.IDLE;
        if(currentTasks.Count > 0){
            currentTaskType = currentTasks[0].taskType;
        }
        return currentTaskType;
    }

    public void CancelTasks(){
        currentTasks.Clear();
    }

    public void FindHostileInProximity(){
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject closeObject in inRange){
            if(IsHostile(closeObject.GetComponent<Unit>())){
                float distance = Vector2.Distance(transform.position, closeObject.transform.position);
                if(distance + 0.2f <= closestDistance) {
                    closest = closeObject;
                    closestDistance = distance;
                }
            }
        }

        if (closest != null && (proximityHostile == null || closest != proximityHostile.GetTarget())){
            proximityHostile = new AttackTask(this, closest);
            hostileDetectionPos = transform.position;
            //if(squad != null){
              //  squad.HostileDetected(closest);
            //}
        }       
    }

    public void UpdateCurrentTask(){
        Task currentTask = null;
        if(proximityHostile != null){
            //Temp fix for not chasing units forever
            if(Vector2.Distance(transform.position, hostileDetectionPos) > 10){
                proximityHostile = null;
                GiveTask(new MoveTask(this, hostileDetectionPos), true);
            }
            else {
                currentTask = proximityHostile;
            }
        }
        else if(currentTasks.Count > 0){
            currentTask = currentTasks[0];
        }
        if(currentTask != null){
            if(currentTask.finished){
                if(currentTask == proximityHostile){
                    proximityHostile = null;
                }
                else {
                    currentTasks.Remove(currentTask);
                }
            } else {
                currentTask.Update();
            }
        }
    }

    public void GiveTask(Task task, bool priority){
        if(priority){
            currentTasks.Insert(0, task);
        }
        else {
            currentTasks.Add(task);
        }
    }

    public bool InRange(Vector2 position){
        return Vector2.Distance(transform.position, position) <= stats.GetRange();
    }

    public bool IsHostile(Unit unit){
        return unit.owner != Faction.NEUTRAL && owner != unit.owner;
    }

    public bool CanAttack(){
        return stats.GetAttackTimer() <= 0;
    }

    public virtual bool AttackTarget(GameObject attackTarget){
        if(CanAttack()){
            stats.SetAttackTimer(stats.GetAttackSpeed());
            Unit unit = attackTarget.GetComponent<Unit>();
            return unit.TakeDamage(stats.GetDamage());
        }
        return false;
    }

    public void Heal(int amount){
        int maxHP = stats.GetHealth();
        int currentHP = stats.GetCurrentHealth();
        if(currentHP < maxHP){
            int newHP = currentHP + amount;
            stats.SetCurrentHealth(newHP);
            healthBar.localScale = new Vector2(((float)newHP/maxHP) * maxHPBar, healthBar.localScale.y);
            if(newHP == maxHP && healthBarBackground){
                healthBar.gameObject.SetActive(false);
                healthBarBackground.SetActive(false);
            }
        }
    }

    public bool TakeDamage(int takenDamage){
        stats.DecreaseCurrentHealth(takenDamage);
        OnTakeDamage();

        if(!healthBar.gameObject.activeSelf) { // Hide healthbar before taken hit
            healthBar.gameObject.SetActive(true);
            healthBarBackground.SetActive(true);
        }

        healthBar.localScale = new Vector2(((float)stats.GetCurrentHealth()/stats.GetHealth()) * maxHPBar, healthBar.localScale.y);
        if (stats.GetCurrentHealth() <= 0){
            OnDeath();
            Destroy(gameObject.transform.root.gameObject);
            return true;
        } 
        return false;
    }

    public virtual void OnTakeDamage(){
        AudioSource audioSrc = GameObject.Find("Sound Effect Player").GetComponent<AudioSource>();
        audioSrc.clip = hitSoundEffects[Random.Range(0, hitSoundEffects.Count)];
        audioSrc.Play();
        GetComponent<DamageEffect>().Activate();
    }

    public virtual void OnDeath(){
        if(producer != null){
            producer.OnDeath(this);
        }
        if(squad != null){
            squad.RemoveUnit(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.GetComponent<Unit>() != null){
            inRange.Add(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.GetComponent<Unit>() != null){
            inRange.Remove(col.gameObject);
        }
    }    

    public void ToggleSelection(bool toggle) {
        isSelected = toggle;
        GameObject outlineObject = transform.Find("outline").gameObject;
        outlineObject.SetActive(toggle);
    }  

    public void MoveToPosition(Vector2 targetPosition) {
        unitMovement.MoveToPosition(targetPosition);
    }

    public bool IsMoving(){
        return unitMovement.moving;
    }

    public bool AtPosition(Vector2 position){
        return Vector2.Distance(transform.position, position) <= 0.2f;
    }

    public bool CanMove(){
        return unitMovement != null;
    }

    public void StopMoving(){
        unitMovement.StopMoving();
    }
 
    public void FollowTarget(GameObject targetObject) {
        unitMovement.FollowTarget(targetObject);
    }   

    public void FollowTarget(GameObject targetObject, Vector2 offset) {
        unitMovement.FollowTarget(targetObject, offset);
    }     

    public virtual void OnReachedDestination(GameObject target){

    }
    
}
