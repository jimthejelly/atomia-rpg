using System.Collections.Generic;
using UnityEngine;

/* This script organizes the goal compounds for the user to create 
   Of course, the compunds are all made of the current available elements.
   Over time, more complicated compounds and more elements can be added.

*/

public class Compound : MonoBehaviour
{
    [System.Serializable]
    public class CompoundData
    {
        public string name;
        public string formula;
        public int targetCharge;

        public CompoundData(string name, string formula, int targetCharge)
        {
            this.name = name;
            this.formula = formula;
            this.targetCharge = targetCharge;
        }
    }

    public static Compound instance;
    private List<CompoundData> compounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeCompounds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCompounds()
    {
        compounds = new List<CompoundData>
        {
            new CompoundData("Methane", "CH₄", 0),
            new CompoundData("Water", "H₂O", 0),
            new CompoundData("Ammonia", "NH₃", 0),
            new CompoundData("Carbon Dioxide", "CO₂", 0),
            new CompoundData("Hydrogen Cyanide", "HCN", 0),
            new CompoundData("Boron Trihydride", "BH₃", 0)
        };
    }

    public CompoundData GetCompound(string compoundName)
    {
        foreach (CompoundData compound in compounds)
        {
            if (compound.name == compoundName)
            {
                return compound;
            }
        }
        Debug.LogWarning($"Compound {compoundName} not found!");
        return null;
    }

    public CompoundData GetRandomCompound()
    {
        return compounds[Random.Range(0, compounds.Count)];
    }

    public List<CompoundData> GetAllCompounds()
    {
        return compounds;
    }
}
