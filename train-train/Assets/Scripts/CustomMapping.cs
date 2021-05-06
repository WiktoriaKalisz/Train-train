using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMapping : SymbolRepresentation {

    public Image selection;
    private bool drawedMask = false;
    private bool _selected = false;
    private bool Selected {
        get { return _selected; }
        set {
            _selected = value;
            Color color = selection.color;
            color.a = _selected ? 0.4f : 0.0f;
            selection.color = color;
        }
    }

    public void onClick() {
        GetComponentInParent<CustomPicker>().HandleSelectRequest(symbol, gameObject);
        Start();
    }

    public void Update(){
        if(!drawedMask)
            Start();
    }

    public void Start() {
        if( symbol != null ) 
            DrawSelected(GetComponentInParent<CustomPicker>().isSelected(symbol));
    }

    public void DrawSelected(bool selectedState) {
        Selected = selectedState;
        drawedMask = true;
    }
}
