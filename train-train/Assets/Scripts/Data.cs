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
    ElementaryMath,
    BeginnerMath, 
    AdvancedMath, 
    ExpertMath,
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
            new BinaryFormatter().Serialize(file, _texture.EncodeToPNG());
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
    public int colorScheme = 1;
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
    public List<SymbolMapping> elementaryMath;
    public List<SymbolMapping> beginnerMath;
    public List<SymbolMapping> advancedMath;
    public List<SymbolMapping> expertMath;
    public List<SymbolMapping> exampleEnglish;


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
                case SymbolType.ElementaryMath:
                    return elementaryMath.ToList();
                case SymbolType.BeginnerMath:
                    return beginnerMath.ToList();
                case SymbolType.AdvancedMath:
                    return advancedMath.ToList();
                case SymbolType.ExpertMath:
                    return expertMath.ToList();
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
    /* ONLY +- */
    public static List<SymbolMapping> elementaryMathMappings()
    {
        var elementaryMath = new List<SymbolMapping>();

        {
            var a = new Symbol("5");
            var l = new List<Symbol>() { new Symbol("1+4"), new Symbol("10-5"), new Symbol("2+3"), new Symbol("6-1"), new Symbol("7-2"), new Symbol("8-3"), new Symbol("9+4"), new Symbol("5+0") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("12");
            var l = new List<Symbol>() { new Symbol("6+6"), new Symbol("10+2"), new Symbol("20-8"), new Symbol("1+11"), new Symbol("5+7"), new Symbol("4+8"), new Symbol("3+9"), new Symbol("15-3") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("15");
            var l = new List<Symbol>() { new Symbol("8+7"), new Symbol("10+5"), new Symbol("20-5"), new Symbol("4+11"), new Symbol("13+2"), new Symbol("10+5"), new Symbol("6+9"), new Symbol("15-0") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("20");
            var l = new List<Symbol>() { new Symbol("10+10"), new Symbol("18+2"), new Symbol("20-0"), new Symbol("1+19"), new Symbol("5+15"), new Symbol("21-1"), new Symbol("30-9"), new Symbol("26-6") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }

        {
            var a = new Symbol("30");
            var l = new List<Symbol>() { new Symbol("15+15"), new Symbol("10+20"), new Symbol("38-8"), new Symbol("42-12"), new Symbol("5+7+18"), new Symbol("40-15+5"), new Symbol("11+11+8"), new Symbol("10+10+10") };
            var map = new SymbolMapping(a, l);
            elementaryMath.Add(map);
        }
        return elementaryMath;
    }
    public static List<SymbolMapping> beginnerMathMappings()
    {
        var beginnerMath = new List<SymbolMapping>();
        {
            var a = new Symbol("40");
            var l = new List<Symbol>() { new Symbol("15+25"), new Symbol("20+20"), new Symbol("48-8"), new Symbol("42-2"), new Symbol("15+7+18"), new Symbol("21+37-18"), new Symbol("11+21+8"), new Symbol("10+10+10+10") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }
        {
            var a = new Symbol("60");
            var l = new List<Symbol>() { new Symbol("35+25"), new Symbol("10+20+30"), new Symbol("35-8+33"), new Symbol("42+12+6"), new Symbol("21+37+2"), new Symbol("40+15+5"), new Symbol("77-11-6"), new Symbol("20+20+20") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }
        {
            var a = new Symbol("80");
            var l = new List<Symbol>() { new Symbol("40+40"), new Symbol("28+52"), new Symbol("13+9+21+37"), new Symbol("47-12+59-14"), new Symbol("16+24+40"), new Symbol("47+13+36-16"), new Symbol("19-13+68+6"), new Symbol("91-23+5+7") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }
        {
            var a = new Symbol("100");
            var l = new List<Symbol>() { new Symbol("50+50"), new Symbol("23+47+30"), new Symbol("13+71+16"), new Symbol("82-61+79"), new Symbol("21+37+42"), new Symbol("73-8+43-8"), new Symbol("24+19-17+74"), new Symbol("121-23+2") };
            var map = new SymbolMapping(a, l);
            beginnerMath.Add(map);
        }

        return beginnerMath;
    }
    /*  +-*: */
    public static List<SymbolMapping> advancedMathMappings()
    {
        var advancedMath = new List<SymbolMapping>();

        {
            var a = new Symbol("5");
            var l = new List<Symbol>() { new Symbol("5*1"), new Symbol("2*5-5"), new Symbol("15/3"), new Symbol("2*3-1") , new Symbol("2*2+1") , new Symbol("20/4") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }

        {
            var a = new Symbol("12");
            var l = new List<Symbol>() { new Symbol("2*6"), new Symbol("1*10+2"), new Symbol("3*4"), new Symbol("21/7 + 3*3"), new Symbol("3*5 - 3") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }

        {
            var a = new Symbol("40");
            var l = new List<Symbol>() { new Symbol("4*5*2"), new Symbol("8*5"), new Symbol("160/4"), new Symbol("4*10"), new Symbol("2*5 + 3*9 + 3") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }

        {
            var a = new Symbol("100");
            var l = new List<Symbol>() { new Symbol("10*10"), new Symbol("4*5*5"), new Symbol("11*11 - 3*7"), new Symbol("2*4*5 + 3*4*5"), new Symbol("7*8 - 3*7 + 13*5") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }

        {
            var a = new Symbol("200");
            var l = new List<Symbol>() { new Symbol("10*20"), new Symbol("8*5*5"), new Symbol("11*22 - 6*7"), new Symbol("4*4*5 + 6*4*5"), new Symbol("14*8 - 6*7 + 26*5") };
            var map = new SymbolMapping(a, l);
            advancedMath.Add(map);
        }

        return advancedMath;
    }
    /*+-*:^*/
    public static List<SymbolMapping> expertMathMappings()
    {
        var expertMath = new List<SymbolMapping>();

        {
            var a = new Symbol("8");
            var l = new List<Symbol>() { new Symbol("2^3"), new Symbol("2^2 + 2^2"), new Symbol("3^2 - 1"), new Symbol("2* 2^2"), new Symbol("4^2 / 2") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("32");
            var l = new List<Symbol>() { new Symbol("2^5"), new Symbol("2^4 + 4 * 2^2"), new Symbol("3^3 + 5^1"), new Symbol("1000^0 + 5^2 + 7^1"), new Symbol("8^2 / 2") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("25");
            var l = new List<Symbol>() { new Symbol("5^2"), new Symbol("3 * 2^3 + 7^0"), new Symbol("3^2 - 1"), new Symbol("2* 2^2"), new Symbol("4^2 / 2") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("81");
            var l = new List<Symbol>() { new Symbol("9^2"), new Symbol("3^4"), new Symbol("10^2 - 4^2 - 3^1"), new Symbol("3* 3^3"), new Symbol("3^5 / 3") };
            var map = new SymbolMapping(a, l);
            expertMath.Add(map);
        }

        {
            var a = new Symbol("100");
            var l = new List<Symbol>() { new Symbol("10^2"), new Symbol("2^2 * 5^2"), new Symbol("11^2 - 5^2 + 2^2 "), new Symbol("4* 5^2"), new Symbol("5^3 - 5^2") };
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
            var l = new List<Symbol>() { new Symbol(Resources.Load<Texture2D>("Images/carrot")) };
            var map = new SymbolMapping(a, l);
            exampleEnglish.Add(map);
        }

        {
            var a = new Symbol(Resources.Load<Texture2D>("Images/cherries"));
            var l = new List<Symbol>() { new Symbol("cherries") };
            var map = new SymbolMapping(a, l);
            exampleEnglish.Add(map);
        }

        {
            var a = new Symbol("watermelon");
            var l = new List<Symbol>() { new Symbol(Resources.Load<Texture2D>("Images/watermelon")) };
            var map = new SymbolMapping(a, l);
            exampleEnglish.Add(map);
        }

        {
            var a = new Symbol("grapes");
            var l = new List<Symbol>() { new Symbol(Resources.Load<Texture2D>("Images/grapes")) };
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
        foreach (var path in new List<string>() { "Images/Bee", "Images/Monkey", "Images/Mouse" })
        {
            var texture = Resources.Load<Texture2D>(path);
            passengers.Add(texture);
            passengers.Select(texture);
        }

        foreach (var path in new List<string>() { "Images/businessman", "Images/doctor", "Images/girl", "Images/girl2", "Images/girl3", "Images/man2", "Images/student", "Images/woman", })
        {
            var texture = Resources.Load<Texture2D>(path);
            passengers.Add(texture);
        }

        return passengers;
    }

    public SymbolMappings defaultCustomMappings()
    {
        defaultTextureSymbols().AllTextures().ForEach(t => customMappings.addMatchee(new Symbol(t)));

        var mapping = new SymbolMapping(Resources.Load<Texture2D>("Images/businessman"));
        customMappings.add(new SymbolMapping(Resources.Load<Texture2D>("Images/doctor")));
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
        elementaryMath = elementaryMathMappings();
        beginnerMath = beginnerMathMappings();
        advancedMath = advancedMathMappings();
        expertMath = expertMathMappings();
        exampleEnglish = exampleEnglishMappings();
    }

    private static Drivers defaultDrivers()
    {
        var drivers = new Drivers();
        var driver1 = Resources.Load<Texture2D>("Images/girl3");
        var driver2 = Resources.Load<Texture2D>("Images/driver");
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
        var paths = new List<string>() {"Images/carrot", "Images/cherries", "Images/grapes", "Images/watermelon", "Images/raspberry",
                                         "Images/gamepad", "Images/pyramid", "Images/rocket", "Images/skateboard", "Images/spinner",
                                         "Images/gift" };

        var textureSymbols = new TextureSymbols();
        paths.ForEach(t => textureSymbols.Add(Resources.Load<Texture2D>(t)));
        textureSymbols.AllTextures().ForEach(t => textureSymbols.Select(t));
        return textureSymbols;
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
        var file = File.Open(destination, FileMode.Create);
        new BinaryFormatter().Serialize(file, Profile);
        file.Close();
        Debug.Log("Profile file was saved.");
    }


    public static string destination;
    public static Profile Profile;

}