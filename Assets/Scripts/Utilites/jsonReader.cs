using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class Json2DialogObject {

    private static string dialogJsonPath = "Assets/Dialog/dialog.json";

    public class DialogObject {
        public string Speaker { get; set; }
        public string Text    { get; set; }
    }

    public class DayDialogObject {
        public string Day                     { get; set; }
        public IList<DialogObject> dialogList { get; set; }
    }

    public class NamesObject {
        public string Alien    { get; set; }
        public string Immortal { get; set; }
        public string Farmer   { get; set; }
    }

    public class Root {
        public NamesObject Names         { get; set; }
        public IList<DayDialogObject> DayDialog { get; set; }
    }

    public void load()
    {
        var fileContent = File.ReadAllText(dialogJsonPath);
        var root = JsonConvert.DeserializeObject<Root>(fileContent);
        Debug.Log("the alien's name is set to be: '" + root.Names.Alien + "'");
    }
}
