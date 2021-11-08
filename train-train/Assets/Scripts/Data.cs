using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public enum SymbolType
{
    SimpleTextures,
    NumberRange,
    Letters,
    ExampleMath,
    Class1Math,
    Class2Math, 
    Class3Math, 
    Class4Math,
    Class45Math,
    Class5Math,
    ExampleEnglish,
    CustomMapping
};

[Serializable]
public class SymbolMappings
{
    private List<Selectable<SymbolMapping>> mappings = new List<Selectable<SymbolMapping>>();
    private List<Symbol> allMatches = new List<Symbol>();

    public void addMatchee(Symbol symbol)
    {
        bool exists = true;

        if (symbol.text != null)
        {
            var all_strings = allMatches.FindAll(s => s.text != null && s.texture == null);
            exists = all_strings.Any(s => s.text.Equals(symbol.text));
        }
        else if (symbol.texture != null)
        {
            var all_textures = allMatches.FindAll(t => t.texture != null && t.text == null);
            exists = all_textures.Any(t => t.texture.path.Equals(symbol.texture.path));
        }

        if (!exists)
            allMatches.Add(symbol);
    }

    public void removeMatchee(Symbol symbol)
    {
        mappings.ForEach(t => t.value.deselect(symbol));
    }

    public Symbol getMatchee(SymbolMappingPickDescriptor symbol)
    {
        return allMatches.Find(i => symbol.mappsTo(i));
    }

    public List<Symbol> allMatchees()
    {
        return allMatches;
    }

    public void add(SymbolMapping t)
    {
        mappings.Add(new Selectable<SymbolMapping>(t, false));
    }

    public List<SymbolMapping> selected()
    {
        return mappings.FindAll(st => st.selected).Select(a => a.value).ToList();
    }

    public List<SymbolMapping> all()
    {
        return mappings.Select(a => a.value).ToList();
    }

    private Selectable<SymbolMapping> find(SymbolMapping mapping)
    {
        return mappings.Find(p => p.value == mapping);
    }

    public void select(SymbolMapping mapping)
    {
        var found = find(mapping);
        found.selected = true;
    }

    public void deselect(SymbolMapping mapping)
    {
        var found = find(mapping);
        found.selected = false;
    }

    public bool isSelected(SymbolMapping mapping)
    {
        var found = find(mapping);
        return found.selected;
    }

    public void remove(SymbolMapping mapping)
    {
        var found = find(mapping);
        mappings.Remove(found);
    }

    public bool IsSelectedEnough()
    {
        return selected().Count > 0;
    }

    public int NumberOfSelected()
    {
        return selected().Count;
    }
}

[Serializable]
public class Selectable<T>
{
    public bool selected = false;
    public T value;

    public Selectable(T t, bool s)
    {
        value = t;
        selected = s;
    }
}

public interface Pickable
{
    int NumberOfSelected();
    List<Texture2D> AllTextures();
    void Add(Texture2D texture);
    void Select(Texture2D texture);
    void Deselect(Texture2D texture);
    bool IsSelected(Texture2D texture);
    void Remove(Texture2D texture);
    bool IsSelectedEnough();
}

[Serializable]
public class Passengers : Pickable
{
    private List<Selectable<STexture2D>> passengers = new List<Selectable<STexture2D>>();

    public void Add(Texture2D t)
    {
        var selectable = new Selectable<STexture2D>(new STexture2D(t), false);
        passengers.Add(selectable);
    }

    public List<Texture2D> selected()
    {
        return passengers.FindAll(st => st.selected).Select(a => a.value.Texture).ToList();
    }

    public List<Texture2D> AllTextures()
    {
        return passengers.Select(a => a.value.Texture).ToList();
    }

    private Selectable<STexture2D> find(Texture2D texture)
    {
        return passengers.Find(p => p.value.Texture == texture);
    }

    public void Select(Texture2D texture)
    {
        var found = find(texture);
        found.selected = true;
    }

    public void Deselect(Texture2D texture)
    {
        var found = find(texture);
        found.selected = false;
    }

    public bool IsSelected(Texture2D texture)
    {
        var found = find(texture);
        return found.selected;
    }

    public void Remove(Texture2D texture)
    {
        var found = find(texture);
        found.value.delete();
        passengers.Remove(found);
    }

    public bool IsSelectedEnough()
    {
        return selected().Count > 0;
    }

    public int NumberOfSelected()
    {
        return selected().Count;
    }
}

[Serializable]
public class TextureSymbols : Pickable
{
    private List<Selectable<STexture2D>> textureSymbols = new List<Selectable<STexture2D>>();

    public void Add(Texture2D t)
    {
        var selectable = new Selectable<STexture2D>(new STexture2D(t), false);
        textureSymbols.Add(selectable);
    }

    public List<Texture2D> selected()
    {
        return textureSymbols.FindAll(st => st.selected).Select(a => a.value.Texture).ToList();
    }

    public List<Texture2D> AllTextures()
    {
        return textureSymbols.Select(a => a.value.Texture).ToList();
    }

    private Selectable<STexture2D> find(Texture2D texture)
    {
        return textureSymbols.Find(p => p.value.Texture == texture);
    }

    public void Select(Texture2D texture)
    {
        var found = find(texture);
        found.selected = true;
    }

    public void Deselect(Texture2D texture)
    {
        var found = find(texture);
        found.selected = false;
    }

    public bool IsSelected(Texture2D texture)
    {
        var found = find(texture);
        return found.selected;
    }

    public void Remove(Texture2D texture)
    {
        var found = find(texture);
        found.value.delete();
        textureSymbols.Remove(found);
    }

    public bool IsSelectedEnough()
    {
        return selected().Count > 0;
    }

    public int NumberOfSelected()
    {
        return selected().Count;
    }
}

[Serializable]
public class Drivers : Pickable
{
    private List<Selectable<STexture2D>> drivers = new List<Selectable<STexture2D>>();

    public void Add(Texture2D t)
    {
        var selectable = new Selectable<STexture2D>(new STexture2D(t), false);
        drivers.Add(selectable);
    }

    private Selectable<STexture2D> find(Texture2D texture)
    {
        return drivers.Find(p => p.value.Texture == texture);
    }

    public void Select(Texture2D t)
    {
        foreach (var driver in drivers)
        {
            driver.selected = false;
        }

        var found = find(t);
        found.selected = true;
    }

    public bool IsSelected(Texture2D texture)
    {
        var found = find(texture);
        return found.selected;
    }

    public void Remove(Texture2D t)
    {
        var found = find(t);
        found.value.delete();
        drivers.Remove(found);
    }

    public List<Texture2D> AllTextures()
    {
        return drivers.Select(a => a.value.Texture).ToList();
    }

    public Texture2D selected()
    {
        var l = drivers.Find(st => st.selected);
        if (l == null) { return null; }
        return l.value;
    }

    public void Deselect(Texture2D texture)
    {
        var found = find(texture);
        found.selected = false;
    }

    public bool IsSelectedEnough()
    {
        return selected() != null;
    }

    public int NumberOfSelected()
    {
        return selected() == null ? 0 : 1;
    }
}

public static class ExtensionMethod
{
    public static Texture2D DeCompress(this Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}

[Serializable]
public class STexture2D
{
    [NonSerialized]
    private Texture2D _texture = null;

    public string path = null;

    public void delete()
    {
        File.Delete(path);
    }

    public static string generateID()
    {
        return Guid.NewGuid().ToString("N");
    }

    public Texture2D Texture
    {
        get
        {
            if (_texture == null)
            {
                var file = File.OpenRead(path);
                _texture = new Texture2D(0, 0);
                _texture.LoadImage((byte[])new BinaryFormatter().Deserialize(file));
                file.Close();
            }

            return _texture;
        }

        set
        {
            _texture = value;
            if (path == null)
            {
                path = Application.persistentDataPath + "/" + generateID() + ".png";
            }

            var file = File.Open(path, FileMode.Create);
            new BinaryFormatter().Serialize(file, _texture.DeCompress().EncodeToPNG());
            file.Close();
        }
    }

    public STexture2D(Texture2D texture)
    {
        Texture = texture;
    }

    public static implicit operator Texture2D(STexture2D st)
    {
        return st.Texture;
    }

}

[Serializable]
public class NumberRange
{
    public int begin;
    public int end;
}

[Serializable]
public class Letters
{
    public List<char> list = new List<char>();
}

[Serializable]
public class Profile
{
    public float trainSpeed = 25;
    public float contrast = 25;
    public float sounds = 10.0f;
    public float music = 10.0f;
    public float lightColor;
    public bool doesEnd = true;
    public bool limitPassengers = true;
    public bool allowScore = true;
    public bool allowLabels = true;
    public bool calmBackground = false;
    public bool leftHand = false;
    public int colorScheme = 0;
    public bool end = false;
    public bool left = false;
    public bool seatActive = false;

    public Drivers drivers = new Drivers();
    public Passengers passengers = new Passengers();

    public SymbolType symbolType = SymbolType.SimpleTextures;
    public TextureSymbols textureSymbols;
    public NumberRange numberRange;
    public Letters letters;
    public SymbolMappings customMappings = new SymbolMappings();
    public List<SymbolMapping> exampleMath;
    public List<SymbolMapping> class1Math;
    public List<SymbolMapping> class2Math;
    public List<SymbolMapping> class3Math;
    public List<SymbolMapping> class4Math;
    public List<SymbolMapping> class45Math;
    public List<SymbolMapping> class5Math;
    public List<SymbolMapping> exampleEnglish;

    public double avgScoreOnCurrentGameMode = 0.0f;
    public int numberOfGamesOnCurrentGameMode = 0;

    public static bool PassengersAnimals = true;
    public bool isAnimalsOn;
    public void ResetScore()
    {
        avgScoreOnCurrentGameMode = 0.0f;
        numberOfGamesOnCurrentGameMode = 0;
    }
    public bool canMathDifficultyBeIncreased()
    {
        return ((this.symbolType >= SymbolType.Class1Math && this.symbolType < SymbolType.Class3Math )||(this.symbolType >= SymbolType.Class4Math && this.symbolType < SymbolType.Class5Math));
    }
    public void IncreaseMathDifficulty()
    {
            this.symbolType += 1;
    }
    public List<SymbolMapping> Symbols
    {
        get
        {
            switch (symbolType)
            {
                case SymbolType.SimpleTextures:
                    return textureSymbols.selected().Select(t => new SymbolMapping(t)).ToList();
                case SymbolType.NumberRange:
                    var numberMappings = new List<SymbolMapping>();
                    for (var i = numberRange.begin; i <= numberRange.end; i++)
                    {
                        numberMappings.Add(new SymbolMapping(i));
                    }
                    return numberMappings;
                case SymbolType.Letters:
                    var letterMappings = new List<SymbolMapping>();
                    foreach (var letter in letters.list)
                    {
                        letterMappings.Add(new SymbolMapping(letter.ToString()));
                    }
                    return letterMappings;
                case SymbolType.ExampleMath:
                    return exampleMath.ToList();
                case SymbolType.Class1Math:
                    return class1Math.ToList();
                case SymbolType.Class2Math:
                    return class2Math.ToList();
                case SymbolType.Class3Math:
                    return class3Math.ToList();
                case SymbolType.Class4Math:
                    return class4Math.ToList();
                case SymbolType.Class45Math:
                    return class45Math.ToList();
                case SymbolType.Class5Math:
                    return class5Math.ToList();
                case SymbolType.ExampleEnglish:
                    return exampleEnglish.ToList();
                case SymbolType.CustomMapping:
                    return customMappings.selected();
            }
            return null;
        }
    }
    /* TODO:
       * THIS WILL NEED TO BE CHANGED WHILE ADDING NEW LEVELS
       *  ADD MORE OF MATH MAPPING, LIKE ELEMENTARY, ADVANCED...OR DIFFERENT NAMING FOR MATH
       *  IN EACH TRY TO ADD MORE POSSIBILITIES, TO NOT HAVE ALL OF GAME PREDICTIBLE
      */
    public static List<SymbolMapping> exampleMathMappings()
    {
        var exampleMath = new List<SymbolMapping>();

        {
            var a = new Symbol("12");
            var l = new List<Symbol>() { new Symbol("2*6"), new Symbol("12"), new Symbol("3*4") };
            var map = new SymbolMapping(a, l);
            exampleMath.Add(map);
        }

        {
            var a = new Symbol("5");
            var l = new List<Symbol>() { new Symbol("5*1"), new Symbol("3+2"), new Symbol("8-3") };
            var map = new SymbolMapping(a, l);
            exampleMath.Add(map);
        }

        {
            var a = new Symbol("15");
            var l = new List<Symbol>() { new Symbol("20-5"), new Symbol("3*5"), new Symbol("60/4") };
            var map = new SymbolMapping(a, l);
            exampleMath.Add(map);
        }

        {
            var a = new Symbol("23");
            var l = new List<Symbol>() { new Symbol("20+3"), new Symbol("4*5+3"), new Symbol("26-3") };
            var map = new SymbolMapping(a, l);
            exampleMath.Add(map);
        }

        {
            var a = new Symbol("100");
            var l = new List<Symbol>() { new Symbol("50+50"), new Symbol("10^2"), new Symbol("99+1") };
            var map = new SymbolMapping(a, l);
            exampleMath.Add(map);
        }

        return exampleMath;
    }
    /* ONLY +- 1-10 */
    public static List<SymbolMapping> class1MathMappings()
    {
        var elementaryMath = new List<SymbolMapping>();

        {
            var a = new Symbol("2");
            var l = new List<Symbol>() { new Symbol("1+1"), new Symbol("2-0"), new Symbol("2+0"), new Symbol("3-1"), new Symbol("4-2"), new Symbol("5-3"), new Symbol("6-4"), new Symbol("7-5") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("5");
            var l = new List<Symbol>() { new Symbol("2+3"), new Symbol("1+4"), new Symbol("5+0"), new Symbol("6-1"), new Symbol("7-2"), new Symbol("8-3"), new Symbol("9-4"), new Symbol("10-5") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("7");
            var l = new List<Symbol>() { new Symbol("0+7"), new Symbol("2+5"), new Symbol("10-3"), new Symbol("9-2"), new Symbol("3+4"), new Symbol("1+6"), new Symbol("8-1"), new Symbol("7-0") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("9");
            var l = new List<Symbol>() { new Symbol("4+5"), new Symbol("3+6"), new Symbol("2+7"), new Symbol("1+8"), new Symbol("0+9"), new Symbol("10-1"), new Symbol("9-0"), new Symbol("6+3") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("10");
            var l = new List<Symbol>() { new Symbol("5+5"), new Symbol("4+6"), new Symbol("3+7"), new Symbol("2+8"), new Symbol("1+9"), new Symbol("0+10"), new Symbol("10-0"), new Symbol("10+0") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }
        return elementaryMath;
    }
    /*dodawanie i odejmowanie w zakresie do 100
    mnożenie i dzielenie w zakresie do 50 
    */
    public static List<SymbolMapping> class2MathMappings()
    {

        var beginnerMath = new List<SymbolMapping>();
        {
            var a = new Symbol("10");
            var l = new List<Symbol>() { new Symbol("2*5"), new Symbol("1*10"), new Symbol("20/2"), new Symbol("40/4"), new Symbol("50/5"), new Symbol("30/3"), new Symbol("99-89"), new Symbol("45-35") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }
        {
            var a = new Symbol("20");
            var l = new List<Symbol>() { new Symbol("15+5"), new Symbol("20+0"), new Symbol("28-8"), new Symbol("22-2"), new Symbol("2*10"), new Symbol("4*5"), new Symbol("40/2"), new Symbol("1*20") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }
        {
            var a = new Symbol("40");
            var l = new List<Symbol>() { new Symbol("15+25"), new Symbol("20+20"), new Symbol("48-8"), new Symbol("42-2"), new Symbol("2*20"), new Symbol("4*10"), new Symbol("8*5"), new Symbol("1*40") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }
        {
            var a = new Symbol("80");
            var l = new List<Symbol>() { new Symbol("40+40"), new Symbol("28+52"), new Symbol("35+25"), new Symbol("9+71"), new Symbol("91-11"), new Symbol("100-20"), new Symbol("94-14"), new Symbol("76+4") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }
        {
            var a = new Symbol("100");
            var l = new List<Symbol>() { new Symbol("50+50"), new Symbol("11+89"), new Symbol("23+77"), new Symbol("13+87"), new Symbol("43+57"), new Symbol("34+66"), new Symbol("96+4"), new Symbol("18+82") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }

        return beginnerMath;
    }
    /*dodawanie, odejmowanie, mnożenie i dzielenie  w zakresie 100. */
    public static List<SymbolMapping> class3MathMappings()
    {
        var advancedMath = new List<SymbolMapping>();

        {
            var a = new Symbol("20");
            var l = new List<Symbol>() { new Symbol("80/4"), new Symbol("100/5"), new Symbol("40/2"), new Symbol("2*10"), new Symbol("100-80") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }
        {
            var a = new Symbol("50");
            var l = new List<Symbol>() { new Symbol("5*10"), new Symbol("100/2"), new Symbol("2*25"), new Symbol("50/1"), new Symbol("50*1") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }
        {
            var a = new Symbol("60");
            var l = new List<Symbol>() { new Symbol("3*20"), new Symbol("30*2"), new Symbol("36+24"), new Symbol("6*10"), new Symbol("99-39") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }
        
        {
            var a = new Symbol("100");
            var l = new List<Symbol>() { new Symbol("2*50"), new Symbol("4*25"), new Symbol("100/1"), new Symbol("64+36"), new Symbol("20*5") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }


        return advancedMath;
    }
    /*dodawanie, odejmowanie, mnożenie i dzielenie  w zakresie 100 + zapisywanie liczb w postaci potęg*/
    public static List<SymbolMapping> class4MathMappings()
    {
        var expertMath = new List<SymbolMapping>();


        {
            var a = new Symbol("25");
            var l = new List<Symbol>() { new Symbol("5^2"), new Symbol("100/4"), new Symbol("25^1"), new Symbol("75/3"), new Symbol("50/2") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }
        {
            var a = new Symbol("32");
            var l = new List<Symbol>() { new Symbol("2^5"), new Symbol("4*8"), new Symbol("2*16"), new Symbol("28+4"), new Symbol("64/2") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }
        {
            var a = new Symbol("81");
            var l = new List<Symbol>() { new Symbol("9^2"), new Symbol("3^4"), new Symbol("9*9"), new Symbol("81^1"), new Symbol("27*3") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("100");
            var l = new List<Symbol>() { new Symbol("10^2"), new Symbol("4*25"), new Symbol("2*50"), new Symbol("1*100"), new Symbol("100/1") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        return expertMath;
    }

    public static List<SymbolMapping> class45MathMappings()
    {
        var expertMath = new List<SymbolMapping>();

        {
            var a = new Symbol("120");
            var l = new List<Symbol>() { new Symbol("2*60"), new Symbol("4*30"), new Symbol("240/2"), new Symbol("12*10")};
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("150");
            var l = new List<Symbol>() { new Symbol("75*2"), new Symbol("5*30"), new Symbol("15*10"), new Symbol("300/2")};
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("400");
            var l = new List<Symbol>() { new Symbol("4*100"), new Symbol("2*200"), new Symbol("40*10"), new Symbol("20^2"), new Symbol("20*20") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("500");
            var l = new List<Symbol>() { new Symbol("2*250"), new Symbol("4*125"), new Symbol("50*10"), new Symbol("20*25") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        return expertMath;
    }
    /*dodawanie, odejmowanie, mnożenie i dzielenie  w zakresie >100
    mnożenie liczb:
    dwucyfrowych przez jednocyfrowe powyżej 100
    trzycyfrowych przez jednocyfrowe w zakresie 1000
    */
    public static List<SymbolMapping> class5MathMappings()
    {
        var expertMath = new List<SymbolMapping>();

        {
            var a = new Symbol("200");
            var l = new List<Symbol>() { new Symbol("2*100"), new Symbol("4*50"), new Symbol("8*25"), new Symbol("40*5"), new Symbol("1000/5") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("600");
            var l = new List<Symbol>() { new Symbol("6*100"), new Symbol("3*200"), new Symbol("4*150"), new Symbol("8*75"), new Symbol("40*15") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("800");
            var l = new List<Symbol>() { new Symbol("8*100"), new Symbol("4*200"), new Symbol("2*400"), new Symbol("20*40"), new Symbol("10*80") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("1000");
            var l = new List<Symbol>() { new Symbol("2*500"), new Symbol("4*250"), new Symbol("8*125") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        return expertMath;
    }
    public static List<SymbolMapping> exampleEnglishMappings()
    {
        var exampleEnglish = new List<SymbolMapping>();

        {
            var a = new Symbol("carrot");
            var l = new List<Symbol>() { new Symbol(Resources.Load<Texture2D>("Images/carrot2")) };
            var map = new SymbolMapping(a, l);
            exampleEnglish.Add(map);
        }

        {
            var a = new Symbol(Resources.Load<Texture2D>("Images/cherries2"));
            var l = new List<Symbol>() { new Symbol("cherries") };
            var map = new SymbolMapping(a, l);
            exampleEnglish.Add(map);
        }

        {
            var a = new Symbol("watermelon");
            var l = new List<Symbol>() { new Symbol(Resources.Load<Texture2D>("Images/watermelon2")) };
            var map = new SymbolMapping(a, l);
            exampleEnglish.Add(map);
        }

        {
            var a = new Symbol("grapes");
            var l = new List<Symbol>() { new Symbol(Resources.Load<Texture2D>("Images/grapes2")) };
            var map = new SymbolMapping(a, l);
            exampleEnglish.Add(map);
        }

        return exampleEnglish;
    }
    /*TODO:
         * IF THERE WILL BE MORE MODELS AND THEY WILL BE BASED ON USERS AGE,
         * THIS PART WILL NEED TO BE CHANGED
         */
    public static Passengers defaultPassengers()
    {
        var passengers = new Passengers();
        foreach (var path in new List<string>() { "Images/Bee2", "Images/Monkey2", "Images/Mouse2", "Images/cat", "Images/Sloth" })
        {
            var texture = Resources.Load<Texture2D>(path);
            passengers.Add(texture);
            if(PassengersAnimals == true) passengers.Select(texture);
        }

        foreach (var path in new List<string>() { "Images/businessman2", "Images/doctor2", "Images/girl22", "Images/man22", "Images/man_2", "Images/student2", "Images/woman2", })
        {
            var texture = Resources.Load<Texture2D>(path);
            passengers.Add(texture);
            if (PassengersAnimals == false) passengers.Select(texture);
        }

        return passengers;
    }

    public SymbolMappings defaultCustomMappings()
    {
        defaultTextureSymbols().AllTextures().ForEach(t => customMappings.addMatchee(new Symbol(t)));

        var mapping = new SymbolMapping(Resources.Load<Texture2D>("Images/businessman2"));
        customMappings.add(new SymbolMapping(Resources.Load<Texture2D>("Images/doctor2")));
        customMappings.add(mapping);
        customMappings.select(mapping);

        return customMappings;
    }

    public void defaultProfile()
    {
        drivers = defaultDrivers();
        passengers = defaultPassengers();
        symbolType = SymbolType.ExampleMath;
        textureSymbols = defaultTextureSymbols();
        numberRange = defaultNumbers();
        letters = defaultLetters();
        customMappings = defaultCustomMappings();
        exampleMath = exampleMathMappings();
        class1Math = class1MathMappings();
        class2Math = class2MathMappings();
        class3Math = class3MathMappings();
        class4Math = class4MathMappings();
        class45Math = class45MathMappings();
        class5Math = class5MathMappings();
        exampleEnglish = exampleEnglishMappings();
        
    }
    private static Drivers defaultDrivers()
    {
        var drivers = new Drivers();
        var driver1 = Resources.Load<Texture2D>("Images/girl22");
        var driver2 = Resources.Load<Texture2D>("Images/driver2");
        drivers.Add(driver1);
        drivers.Add(driver2);
        drivers.Select(driver2);
        return drivers;
    }

    private static Letters defaultLetters()
    {
        return new Letters
        {
            list = new List<char>() { 'a', 'b', 'c' }
        };
    }

    private static NumberRange defaultNumbers()
    {
        return new NumberRange
        {
            begin = 1,
            end = 10
        };
    }

    private static TextureSymbols defaultTextureSymbols()
    {
        var paths = new List<string>() {"Images/carrot2", "Images/cherries2", "Images/grapes2", "Images/watermelon2", "Images/raspberry2",
                                         "Images/gamepad2", "Images/pyramid2", "Images/rocket2", "Images/skateboard2", "Images/spinner",
                                         "Images/gift2" };

        var textureSymbols = new TextureSymbols();
        paths.ForEach(t => textureSymbols.Add(Resources.Load<Texture2D>(t)));
        textureSymbols.AllTextures().ForEach(t => textureSymbols.Select(t));
        return textureSymbols;
    }

    public string GetGameMode()
    {
        switch (symbolType)
        {
            case SymbolType.SimpleTextures:
                return "Simple Textures";
            case SymbolType.NumberRange:
                return "Number Range";
            case SymbolType.Letters:
                return "Letters";
            case SymbolType.ExampleMath:
                return "Math Mixer";
            case SymbolType.Class1Math:
                return "Math 1-3 Easy";
            case SymbolType.Class2Math:
                return "Math 1-3 Medium";
            case SymbolType.Class3Math:
                return "Math 1-3 Hard";
            case SymbolType.Class4Math:
                return "Math 4-5 Easy";
            case SymbolType.Class45Math:
                return "Math 4-5 Medium";
            case SymbolType.Class5Math:
                return "Math 4-5 Hard";
            case SymbolType.ExampleEnglish:
                return "English";
            case SymbolType.CustomMapping:
                return "Custom";
        }
        return null;
    }
}


public static class Data
{


    static Data()
    {
        destination = Application.persistentDataPath + "/profiles51.bin";
        load();
    }

    public static void load()
    {

        if (File.Exists(destination))
        {
            var file = File.OpenRead(destination);
            Profile = (Profile)new BinaryFormatter().Deserialize(file);
            file.Close();
        }
        else
        {
            reset();
        }

        Profile.drivers.AllTextures();
        Profile.passengers.AllTextures();
        Profile.textureSymbols.AllTextures();
        Profile.PassengersAnimals = Profile.isAnimalsOn;

        Debug.Log("Profile file was loaded.");
    }

    public static void reset()
    {
        Profile = new Profile();
        Profile.defaultProfile();
        File.Delete(destination);
    }

    public static void save()
    {
        Profile.isAnimalsOn = Profile.PassengersAnimals;
        var file = File.Open(destination, FileMode.Create);
        new BinaryFormatter().Serialize(file, Profile);
        file.Close();
        Debug.Log("Profile file was saved.");
    }


    public static string destination;
    public static Profile Profile;

}