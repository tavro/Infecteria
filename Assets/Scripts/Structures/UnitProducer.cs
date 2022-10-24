using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitProducer : MonoBehaviour
{
    public Dictionary<UnitType, UnitProductionData> productionData = new Dictionary<UnitType, UnitProductionData>();

    void Update(){
        if(PauseManager.Instance.CurrPauseState == PauseManager.PauseState.NONE)
            foreach(KeyValuePair<UnitType, UnitProductionData> entry in productionData)
                entry.Value.Update();
    }

    public void IncreaseMaximumUnit(UnitType unitType, int amount){
        productionData[unitType].IncreaseMaximum(amount);
    }

    public void AddProduction(UnitType unitType, UnitProductionData newProductionData){
        productionData.Add(unitType, newProductionData);
    }

    public float GetProductionProgress(UnitType unitType){
        return productionData[unitType].GetProductionProgress();
    }

    public int GetAvailableAmount(UnitType unitType){
        return productionData[unitType].GetAvailableAmount();
    }

     public int GetMaximumAmount(UnitType unitType){
        return productionData[unitType].GetMaximumAmount();
    }   

    public int GetSpawnedAmount(UnitType unitType){
        return productionData[unitType].GetSpawnedAmount();
    }

    public int WithdrawAll(UnitType unitType){
        if(productionData.ContainsKey(unitType))
            return productionData[unitType].WithdrawAll();
        return -1;
    }

    public int WithdrawAmount(UnitType unitType, int amount){
        return productionData[unitType].WithdrawAmount(amount);
    }    

    public void OnReturn(Unit returnedUnit){
        productionData[returnedUnit.unitType].OnReturn();
        Destroy(returnedUnit.transform.root.gameObject);
    }

    public void OnDeath(Unit deadUnit){
        productionData[deadUnit.unitType].OnDeath();
    }    
}