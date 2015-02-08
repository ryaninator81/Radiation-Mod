using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace tjs_radMod
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class radManager //Global checking of radiation
    {
        //Some changes are not tested!
        
        //Make sure this on is the same as the one in "tjs_podRadiation"!!!
        private int safetyWait = 25; //Ammount of ticks to wait before the radiation stuff starts to happen (Safety first)
        private int safetyWaited = 0; //Ammount of ticks that have been already waited

        public void Start()
        {
            Debug.Log("[Radiation Manager] Start!");
            
        }

        public void Update()
        {

            if (safetyWait == safetyWaited)
            {
                //All code goes here!


            }
            else
            {
                //Safety first!
                safetyWaited = safetyWaited + 1;

            }

        }



        public double getRadiationLevel(string activeBody, double activeAltitude, double activeLatitude) //Lethal with no protection: 1.5 (WIP)
        {
            activeLatitude = Math.Abs(activeLatitude);
            
            if (activeAltitude == 0) //Just in case you are at exactly 0 meters...
            {
                activeAltitude = 1; //Set to a reasonably low number.
            }
            if (activeLatitude == 1) //Also, to not get "Infinity" as radiation dose...
            {
                activeLatitude = 1.5; //Set to 1.5 so the result is not 0, but -0.5
            }
             
            if (activeBody == "Kerbol")
            {
                double groundDose = 100000000000;  //100,000,000,000 = 1 d/s (7.3 d/s on kerbin's altitude)
                double radiationLevel = groundDose / activeAltitude;  //Simplified, needs work!!
                if (radiationLevel == 0.00) { radiationLevel = 0.01; } //Safety First!!
                return radiationLevel;
               
            }
            if (activeBody == "Moho")
            {
                //Here stuff gets tricky...
                //I know my math is WAY off, needs tweaking, though, it works :P
                double alwaysDose = 19.79;  //AVERAGE radiation dose on moho's orbit altitude. 
                double groundDose = alwaysDose + 1;  //Moho has something inside, emiting not bad ammounts of radiation, making it "deadlier"
                double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simplified, needs work!!
                if (radiationLevel == 0.00) { radiationLevel = 0.01; } //Safety First!!
                radiationLevel = radiationLevel + alwaysDose;
                return radiationLevel;
            }

            if (activeBody == "Eve")
            {
                //Do you think what you saw before was strange? This is even weirder cause Eve has an atmosphere...
                double alwaysDose = 10.06;  //AVERAGE radiation dose on Eve's orbit altitude. 
                double atmoDeep = 100000 / activeAltitude;
                double groundDose = (alwaysDose + 0.2)-(atmoDeep/20000);  //At 0 meters, you will have a radiation of 5.06! (Without any other calculation :P)
                double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simplified, needs work!!
                if (radiationLevel == 0.00) { radiationLevel = 0.01; } //Safety First!!
                radiationLevel = radiationLevel + (alwaysDose - atmoDeep/20000); //At ground level, the radiation level is about 2.
                return radiationLevel;

            }
            if(activeBody == "Kerbin")
            {
                //Kerbin calculation almost negates radiation at ground due to protective gases
                double alwaysDose = 7.35;  //AVERAGE radiation dose on Kerbin's orbit altitude. 
                double atmoDeep = 70000 / activeAltitude;
                double groundDose = alwaysDose - (atmoDeep / 10000);  //At 0 meters, you will have a radiation of 0.35! (Without any other calculation :P)
                double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simplified, needs work!!
                if (radiationLevel == 0.00) { radiationLevel = 0.01; } //Safety First!!
                radiationLevel = radiationLevel + (alwaysDose - atmoDeep / 10000); //At ground level, the radiation level is about 0,35.
                return radiationLevel;
            }
            //From here everything is WIP!
            if (activeBody == "Dres")
            {
                double alwaysDose = 19.79;  //AVERAGE radiation dose on moho's orbit altitude. 
                double groundDose = alwaysDose + 1;  //Moho has something inside, emiting not bad ammounts of radiation, making it "deadlier"
                double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simplified, needs work!!
                if (radiationLevel == 0.00) { radiationLevel = 0.01; } //Safety First!!
                radiationLevel = radiationLevel + alwaysDose;
                return radiationLevel;
            }else{
                return 2.00; //WIP Return
            }
            
            

        }

        }
    }
