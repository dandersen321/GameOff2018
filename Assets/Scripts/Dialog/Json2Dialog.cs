using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

//public static readonly string alienKey    = "alien";
//public static readonly string immortalKey = "immortal";
//public static readonly string farmerKey   = "farmer";

public class SpeakerDialogObj {
    public string name    { get; set; } // overrides speaker
    public string speaker { get; set; }
    public string line    { get; set; }
}

public class DayDialogObj {
    public string firstSpeaker                { get; set; }
    public int    day                         { get; set; }
    public IList<SpeakerDialogObj> dialogList { get; set; }
}

public class NamesObj {
    public string alien    { get; set; }
    public string immortal { get; set; }
    public string farmer   { get; set; }

    public string getNameOf(string speaker)
    {
        if (speaker == "alien") return alien;
        if (speaker == "immortal") return immortal;
        if (speaker == "farmer") return farmer;
        return "";
    }
}

public class DialogJsonObj {
    public NamesObj Names                { get; set; }
    public IList<DayDialogObj> DayDialog { get; set; }
}

public sealed class DialogJsonParser {

    public static DialogJsonObj getDialog()
    {
        return getInstance().m_dialogObj;
    }

    private  static DialogJsonParser getInstance()
    {
        return m_instance;
    }

    private static          string           m_dialogJsonPath = "Assets/Dialog/dialog.json";
    private static readonly DialogJsonParser m_instance       = new DialogJsonParser();
    private                 DialogJsonObj    m_dialogObj;

    private void load()
    {
        m_dialogObj = new JsonDeserializer<DialogJsonObj>().deserialize(m_dialogJsonPath);
        Debug.Log("the alien's name is set to be: '" + m_dialogObj.Names.alien + "'");
    }

    private DialogJsonParser()
    {
        load();
    }

    // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
    static DialogJsonParser()
    {
    }
}

public class Json2Dialog {

    //public List<DayDialog> getDialogForDay(string speaker, int day)
    //{
    //    var obj = DialogJsonParser.getDialog();
    //    var dayDialog = obj.DayDialog[day];

    //    if (dayDialog.firstSpeaker != speaker) {
    //        return getOtherSpeakerPrompt(speaker);
    //    }
    //}

    //private List<DayDialog> getOtherSpeakerPrompt(string speaker, int day)
    //{
    //    var dayDialogList = new List<DayDialog>();
    //    DayDialog dayDialog;
    //    dayDialog.day = day;
    //    dayDialog.Lines = Lines { {
    //}

    //private string getNameForSpeaker(DialogJsonObj obj, string speaker)
    //{
    //    return obj.Names.getNameOf(speaker);
    //}

    //private string getDefaultDialog(string speaker)
    //{
    //    return "";
    //}
}

