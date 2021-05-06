using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class SymbolRepresentation : MonoBehaviour
{
    public Text text;
    public RawImage image;
    public Symbol symbol;
   
    public void setSymbol(Symbol symbol)
    {
        Debug.Assert(symbol.text != null ^ symbol.texture != null, "Bad sybol state!" );

        this.symbol = symbol;
        if (symbol.text != null) {
            text.text = symbol.text;
            image.gameObject.SetActive(false);
            text.gameObject.SetActive(true);
        } else if (symbol.texture != null) {
            image.texture = symbol.texture;
            image.gameObject.SetActive(true);
            text.gameObject.SetActive(false);
        }
    }


}