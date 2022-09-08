using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public int DNAPoints = 0;
    
    private int absorbedCells = 0;
    private int infectedCells = 0;
    private int factoryAmount = 0;

    public void UpdateTextMesh() {
        textMesh.text = "DNA Points: " + DNAPoints + "\nAbsorbed Cells: " + absorbedCells + "\nInfected Cells: " + infectedCells + "\nFactories: " + factoryAmount;
    }

    public void IncreaseDNAPoints(int amount) {
        DNAPoints += amount;
        UpdateTextMesh();
    }

    public void IncreaseAbsorbedCells() {
        absorbedCells++;
        UpdateTextMesh();
    }

    public void IncreaseInfectedCells() {
        infectedCells++;
        UpdateTextMesh();
    }

    public void IncreaseFactoryAmount() {
        factoryAmount++;
        UpdateTextMesh();
    }
}
