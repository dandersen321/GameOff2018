using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JsonDeserializer<ObjectType> {

    public ObjectType deserialize(string filePath)
    {
        var fileContent = File.ReadAllText(filePath);
        var obj         = JsonConvert.DeserializeObject<ObjectType>(fileContent);
        return obj;
    }
}
