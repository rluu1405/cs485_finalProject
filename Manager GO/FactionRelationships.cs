using UnityEngine;
using System.Collections;

public class FactionRelationships {

    //      Column likes Row if non-negative       C     A      N      E      P
    int[,] relationships = new int[5, 5]     { { 9999,  500,   100,  -9999,  200 },     // C
                                               { 500,  9999,   200,  -1000,  200 },     // A
                                               { 100,   100,  9999,   100,   -9999 },     // N
                                               {-9999, -500,  -500,   9999, -500 },     // E
                                               { 200,   200,   -9999,  -500,  9999 } };   // P



    // Get the reputation of how faction sees otherFaction
    // If max value is returned, then the tage of otherFaction isnt actually a faction
    public int GetReputation(string faction, string otherFaction)
    {
        uint i = 0, j = 0;
        LookUp(faction, otherFaction, ref i, ref j);
        if (i == int.MaxValue || j == int.MaxValue)
        {
            return int.MaxValue;
        }
        else
        {
            //Debug.Log("i: " + i + "and j: " + j);   
            return relationships[i, j];
        }
            
    }

    // Modify a repututation of how faction sees otherFaction
    public void IncrementReputation(string faction, string otherFaction, int change)
    {
        uint i = 0, j = 0;
        if (faction != otherFaction)
        {
            LookUp(faction, otherFaction, ref i, ref j);
            if (i != int.MaxValue && j != int.MaxValue)
                relationships[i, j] += change;
        }
    }


    // Lookup function returns indices
    private void LookUp(string faction, string otherFaction, ref uint i, ref uint j)
    {
        uint[] indices = new uint[2] { i, j };
        string[] factions = new string[2] {faction, otherFaction };

        for (uint k = 0; k < 2; k++)
        {
            switch (factions[k])
            {
                case "Coalition":
                    indices[k] = 0;
                    break;
                case "Alien":
                    indices[k] = 1;
                    break;
                case "Nanite":
                    indices[k] = 2;
                    break;
                case "Earth":
                    indices[k] = 3;
                    break;
                case "Player":
                    indices[k] = 4;
                    break;
                case "Owned":
                    indices[k] = 4;
                    break;
                default:
                    indices[k] = int.MaxValue;
                    break;
            }

            i = indices[0];
            j = indices[1];
        }
    }

}
