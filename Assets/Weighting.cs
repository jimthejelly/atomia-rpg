using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weighting : MonoBehaviour
{
    [System.Serializable]
    public class Element
    {
        public string name;
        public string symbol;
        public int charge;

        public Element(string name, string symbol, int charge)
        {
            this.name = name;
            this.symbol = symbol;
            this.charge = charge;
        }
    }

    public static Weighting instance;
    private Dictionary<string, Element> elements;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeElements();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    private void InitializeElements()
    {
        elements = new Dictionary<string, Element>
        {
            { "B", new Element("Boron", "B", 3) },
            { "C", new Element("Carbon", "C", 4) },
            { "H", new Element("Hydrogen", "H", 1) },
            { "N", new Element("Nitrogen", "N", -3) },
            { "O", new Element("Oxygen", "O", -2) }
        };
    }

    public Element GetElement(string symbol)
    {
        if (elements.ContainsKey(symbol))
        {
            return elements[symbol];
        }
        Debug.LogWarning($"Element with symbol {symbol} not found!");
        return null;
    }

    public Dictionary<string, Element> GetAllElements()
    {
        return elements;
    }
}
