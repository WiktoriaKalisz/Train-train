using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Symbol
{
    string _text = null;
    STexture2D _texture = null;
    public string text { get { return _text; } set { _text = value; _texture = null; } }
    public STexture2D texture { get { return _texture; } set { _texture = value; _text = null; } }

    public Symbol(string t)
    {
        text = t;
    }

    public Symbol(Texture2D t)
    {
        texture = new STexture2D(t);
    }
}

[Serializable]
public class SymbolMappingPickDescriptor{
    string texturePath = null,
            symbolText = null;

    public SymbolMappingPickDescriptor(Symbol s) {
        if( s.texture != null )
            texturePath = s.texture.path;
        else
            symbolText = s.text;
    }

    public bool mappsTo(Symbol symbol)
    {
        if( symbol.texture != null && texturePath != null )
            return symbol.texture.path.Equals( texturePath );
        else if( symbol.text != null && symbolText != null )
            return symbol.text.Equals( symbolText );
        else
            return false;
    }
}

[Serializable]
public class SymbolMapping
{
    Symbol matcher;
    List<SymbolMappingPickDescriptor> matches = new List<SymbolMappingPickDescriptor>();
    
    public SymbolMapping(Texture2D t) {
        var symbol = new Symbol(t);
        matcher = symbol;
        Data.Profile.customMappings.addMatchee(symbol);
        matches.Add( new SymbolMappingPickDescriptor(symbol) );
    }

    public SymbolMapping(Symbol symbol, List<SymbolMappingPickDescriptor> matche ) {
        matcher = symbol;
        matches = matche;
    }

    public SymbolMapping( Symbol symbol, List<Symbol> symbols ) {
        symbols.ForEach( i => {
            Data.Profile.customMappings.addMatchee( i );
            matches.Add( new SymbolMappingPickDescriptor(i) );
        } );
        matcher = symbol;
    }

    public SymbolMapping(int i) {
        var symbol = new Symbol(i.ToString());
        matcher = symbol;
        Data.Profile.customMappings.addMatchee( symbol );
        matches.Add( new SymbolMappingPickDescriptor( symbol ) );
    }

    public SymbolMapping(string s) {
        var symbol = new Symbol(s);
        matcher = symbol;
        Data.Profile.customMappings.addMatchee( symbol );
        matches.Add( new SymbolMappingPickDescriptor( symbol ) );
    }

    public Symbol stationSymbol() {
        return matcher;
    }
    
    public Symbol randomMatching() {
        var randSymb = matches[UnityEngine.Random.Range(0, matches.Count)];
        return Data.Profile.customMappings.getMatchee(randSymb);
    }

    public bool doesMatch(Symbol symbol) {
        return isSelected( symbol );
    }

    public bool IsSelectedEnough()
    {
        return matches.Count > 0;
    }

    public int NumberOfSelected()
    {
        return matches.Count;
    }

    public void select(Symbol symbol) {
        matches.Add( new SymbolMappingPickDescriptor( symbol ) );
    }

    public void deselect(Symbol symbol) {
        matches.RemoveAll( t => t.mappsTo( symbol ) );
    }

    public bool isSelected(Symbol symbol) {
        if(matches == null) { return false; }
        return matches.Any( t => t.mappsTo(symbol) );
    }
}
