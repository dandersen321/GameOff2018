using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurrentActivator : Item
{
    


    public override void use()
    {
        References.GetTurrent().activateTurrentMode();
    }

}

