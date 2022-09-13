using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : Infectable
{
    private Vector2 originPos;
    private Vector2 target;
    
    public GameObject factoryPrefab;
    public Transform infectionBarPivot;

    public float offset;
    public float speed;

    public SpriteRenderer insideRenderer;

    void Start()
    {
        originPos = transform.position;
        target = GetNewTarget();
    }

    public void GetAbsorbed() {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.IncreaseDNAPoints(100);
        gm.IncreaseAbsorbedCells();
        gm.RemoveCell(gameObject);
        Destroy(gameObject);
    }

    public void TurnIntoFactory() {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.IncreaseFactoryAmount();
        GameObject factory = Instantiate(factoryPrefab, transform.position, Quaternion.identity);
        gm.ReplaceCell(gameObject, factory);
        Destroy(gameObject);
    }

    Vector2 GetNewTarget() {
        return new Vector2(Random.Range(originPos.x-offset, originPos.x+offset), Random.Range(originPos.y-offset, originPos.y+offset));
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        
        float diff = Vector2.Distance(transform.position, target);
        if(diff <= 0.01f) {
            target = GetNewTarget();
        }
        transform.position = Vector2.MoveTowards(transform.position, target, step);
    }

    public override void OnInfect(){
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.IncreaseInfectedCells();
    }

    public override void OnInfectUpdate(float amount){
        insideRenderer.color = new Color(insideRenderer.color.r, insideRenderer.color.g + amount/10.0f, insideRenderer.color.b, insideRenderer.color.a);
        infectionBarPivot.localScale = new Vector2(infectionAmount/10.0f, 1.0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().SetSpeed(2.5f);
            col.gameObject.GetComponent<Player>().currentCell = this;
        }
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerMovement>().SetSpeed(5.0f);
            col.gameObject.GetComponent<Player>().currentCell = null;
        }
    }
}
