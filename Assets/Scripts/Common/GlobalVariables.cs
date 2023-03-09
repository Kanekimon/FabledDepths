using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GlobalVariables
{
    public static float UnitSize = 1f;

    public static float HaltUnit = UnitSize / 2;

    public static string RoamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    public static string PlayerConfig = Path.Combine(RoamingDirectory, "FabledDepths\\player.json");
    public static string DeckConfig = Path.Combine(RoamingDirectory, "FabledDepths\\decks.json");

}

