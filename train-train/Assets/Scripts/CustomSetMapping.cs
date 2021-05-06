using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSetMapping : SymbolRepresentation {

    public Image selection;
    private SymbolMapping mapping = null;
    private bool drawedMask = false;
    private bool _selected = false;
    private bool Selected { get { return _selected; } set {
            _selected = value;
            Color color = selection.color;
            color.a = _selected ? 0.4f : 0.0f;
            selection.color = color;
        } }

    public void setMapping(SymbolMapping mapping){
        this.mapping = mapping;
        setSymbol(mapping.stationSymbol());
    }

    public void onClick() {
        GetComponentInParent<CustomSetPicker>().HandleSelectRequest(mapping, gameObject);
        Start();
    }

    public void onEditMappingClick() {
        CustomPicker.Modify(mapping);
    }

    public void Update()
    {
        if(!drawedMask)
            Start();
    }

    public void Start() {
        if( mapping != null )
            DrawSelected( GetComponentInParent<CustomSetPicker>().isSelected( mapping ) );
    }

    public void DrawSelected(bool selectedState) {
        Selected = selectedState;
        drawedMask = true;
    }
}
