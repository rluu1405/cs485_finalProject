///*
//    Status: No compilation errors - waiting for SpaceEntities to have a cargo.
//    WIP's (to implement) marked - use CTRL+F for '[WIP]'
//*/

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//// Entity for a static building in-game.*/
//public class Station : ShieldedEntity {
//    public int cargoCapacity = 3; //How many different items in total.*/
//    List<Item> dealings;          //Items the station will exclusively deal with.*/
//    Dictionary<Item, int>   stock;//Container for items.*/
//    Dictionary<Item, int>   cap_table; //How much a station can carry a specific item.*/
//    Dictionary<Item, float> cost_table;//How much a station would pay for an item.*/
//    Dictionary<Item, int>   p;//Production costs. <0 values = resource, >0, products.*/
//    float time_threshold;     //Time for a single production cycle*/ 
//    float time_progress;      //Current progress to a production cycle.*/

//    /*Will not vary between instantiations and types of stations.
//    ----------------------------------------------------------*/
//    List<Ship> docking_bay;   //Ships that are docked at a station.*/

//    //--------------------------------------------------------
//    void Start() {
//        /*Destroyed station -> nothing.*/
//        stock       = new Dictionary<Item, int>();
//        cost_table  = new Dictionary<Item, float>();
//        cap_table   = new Dictionary<Item, int>();
//        docking_bay = new List<Ship>();
//        //placeholder value- bound to vary for balancing or different kinds of Stations.*/
//        time_threshold = 1f;
//        time_progress  = 0f;
//        /*Preset values, existing only for testing purposes.
//        -----------------------------------------------------*/
//        //[WIP] - Add in placeholders OR instantiations by type of Station
//    }

//    /*Prone to change - cheap workaround from adding code in Traders
//      Factory facilitates the transactions, instead of - 
//      the Trader having a goal and facilitating the transaction.*/
//    private void fill_order(/*inventory entity*/) {
//        foreach (var ware in dealings) {
//            //[WIP] - SpaceEntity needs a cargo or inventory entity.*/
//        }
//    }

//    //-----------------------------------------------------+
//    /*NOTE: Called by produce.*/
//    private bool invencheck() {
//        foreach (var ware in dealings) {
//        /*1: Distinguish between a product and a reagent.
//          2: If does not have enough for a production cycle, then do not produce.*/
//            if (p[ware] < 0 && stock[ware] > -p[ware]) {
//                return false;
//            }
//        }
//        return true;
//    }

//    /*NOTE: Greater than 0 check is done by Update(). 
//     * However it is okay to have products be at 0.*/
//    private void Produce() {
//        if (!invencheck())
//            return;
//        //Increment until time threshold*/
//        time_progress += Time.deltaTime * 0.01f;
//        if (time_progress >= time_threshold) {
//            //At time threshold:
//            //Subtract from stock reagent items.*/
//            //Add to stock of produced items.*/
//            foreach (var ware in dealings) {
//                stock[ware] += p[ware];
//            }
//        }
//    }
//    //-----------------------------------------------------+

//    void Update() {
//        /*1. Check for trade action only when there is a docked ship on station.
//             Trades can be initiated between NPCs for this entity 
//             - only draw GUI on player trade.
//             Reject unwanted items (not in dealings).*/
//        foreach(var ship in docking_bay) {
//            if (ship.gameObject.tag != "Player") {
//                fill_order(/*Ship's inventory entity*/);
//                //[WIP] - Related to fill_order's WIP
//            }
//        }
//        //Wait for player to initiate trade, by event, w/e.
//        /*2. Produce items*/
//        Produce();
//        /*3. Re-adjust prices based on stock.*/
//        foreach (var ware in dealings) {
//            //Simple equation- no thought put into balancing.
//            float st_ratio    = 1.0f - (stock[ware]/cap_table[ware]);
//            cost_table[ware] = ware.ItemValue * st_ratio;
//        }
//    }
//}
