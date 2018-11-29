using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System.Text.RegularExpressions;

public class SpeakerObj {
    public string name                  { get; set; }
    public string otherSpeakerPrompt    { get; set; }
    public string defaultDialog         { get; set; }
}

public class Speakers {
    public SpeakerObj Alien    { get; set; }
    public SpeakerObj Immortal { get; set; }
    public SpeakerObj Farmer   { get; set; }
}

public class SpeakerDialogObj {
    public string name    { get; set; } // overrides speaker
    public string speaker { get; set; }
    public string text    { get; set; }
}

public class DayDialogObj {
    public string firstSpeaker                    { get; set; }
    public int    day                             { get; set; }
    public IList<SpeakerDialogObj> alienDialog    { get; set; }
    public IList<SpeakerDialogObj> immortalDialog { get; set; }
}

public class DialogJsonObj {
    public Speakers            Speakers  { get; set; }
    public IList<DayDialogObj> DayDialog { get; set; }
}

// Singleton, parses dialog.json file into structures defined above
// and provides a static getDialog() to access the parsed data.
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
        // Debug.Log("the alien's name is set to be: '" + m_dialogObj.Speakers.Alien.name + "'");
        // Debug.Log("the text: '" + m_dialogObj.DayDialog[0].alienDialog[0].text + "'");
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

// translate the parsed JSON into data structures needed to display the text in-game.
public class Json2Dialog {

    public static List<DayDialog> getDialogListForDay(Speaker speaker, int day)
    {
	int maxChars = 76;
        var obj               = DialogJsonParser.getDialog();
        var objDayDialog      = obj.DayDialog[day]; // current day counter should be zero based.

        DayDialog dayDialog = new DayDialog();
        dayDialog.day       = day;

        var dialogList = getDialogList(objDayDialog, speaker);
        if (dialogList == null) {
            throw new System.Exception("dialog null! probably syntax error in dialog.json!");
        }
	// split the text into an array of lines to be displayed on screen.
	// if the next word is > than the size we allow in the text area,
	// add it to the next line.
        foreach (var dialog in dialogList) {
            string text    = dialog.text;
	    string[] words = text.Split(new char [] {' '});
	    int lineLen    = 0;
            Line line      = new Line();
	    line.name      = getNameOfSpeaker(obj.Speakers, dialog.speaker);
	    foreach (var word in words) {
		if (word.Length + lineLen >= maxChars) {
		    dayDialog.lines.Add(line);
		    line      = new Line();
		    line.name = getNameOfSpeaker(obj.Speakers, dialog.speaker);
		    lineLen   = 0;
		}
		line.line += word + " ";
		lineLen += word.Length +1;
	    }
	    dayDialog.lines.Add(line);
        }
        var dayDialogList = new List<DayDialog>();
        dayDialogList.Add(dayDialog);
        return dayDialogList;
    }

    private static IList<SpeakerDialogObj> getDialogList(DayDialogObj obj, Speaker speaker)
    {
        switch (speaker) {
            case Speaker.alien:
                return obj.alienDialog;
            case Speaker.immortal:
                return obj.immortalDialog;
            default:
                throw new System.Exception("undefined speaker");
        }
    }

    private static string getNameOfSpeaker(Speakers speakerObj, string speaker)
    {
	if (speaker == "alien") {
	    return speakerObj.Alien.name;
	}
	if (speaker == "immortal") {
	    return speakerObj.Immortal.name;
	}
	if (speaker == "farmer") {
	    return speakerObj.Farmer.name;
	}
	throw new System.Exception("undefined speaker");
    }

    private static SpeakerObj getSpeaker(DialogJsonObj obj, Speaker speaker)
    {
        switch (speaker) {
            case Speaker.alien:
                return obj.Speakers.Alien;
            case Speaker.immortal:
                return obj.Speakers.Immortal;
            default:
                throw new System.Exception("undefined speaker");
        }
    }
}
