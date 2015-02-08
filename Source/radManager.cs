using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace tjs_radMod
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class radManager : MonoBehaviour //Global checking of radiation
    {
        
        //Make sure this on is the same as the one in "tjs_podRadiation"!!!
        private int safetyWait = 25; //Ammount of ticks to wait before the radiation stuff starts to happen (Safety first)
        private int safetyWaited = 0; //Ammount of ticks that have been already waited

        public void Start()
        {
            Debug.Log("[Radiation Manager] Start!");
            Debug.Log("[Radiation Manger Debugger] Remember kids, never exceed 32 bit limit!");
            
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


        // Beware, this function is the hearth of the mod, but is very messy :P
        public static double getRadiationLevel(CelestialBody activeBody, double activeAltitude, double activeLatitude, double atmoActualDeep) //Lethal with no protection: 1.5 (WIP)
        {
            activeLatitude = Math.Abs(activeLatitude);  
            bool hasAtmosphere = false;
          
            double groundDose = 1;
            
            double activeAtmoAltitude = 1; //How deep are you in the atmosphere? 0 when there is no atmosphere over you.
            
            if (activeAltitude == 0) //Just in case you are at exactly 0 meters...
            {
                activeAltitude = 1; //Set to a reasonably low number.
            }
            if (activeLatitude == 1) //Also, to not get "Infinity" as radiation dose...
            {
                activeLatitude = 1.5; //Set to 1.5 so the result is not 0, but -0.5
            }
            if(activeAltitude == activeLatitude)  //Another safety check! (Due to code limitations :P)
            {
                activeLatitude = activeLatitude + 1;
            }

            //Creating variable table for active planet (Compatibility with planet packs)
            if (activeBody.atmosphere == true) 
            { 
                hasAtmosphere = true; 
            }
            double seaLevelAtm = activeBody.atmosphereMultiplier; //The pressure at sea level (ATM)      
            double atmScaleHeight = activeBody.atmosphereScaleHeight; //On kms, this is a very confusing thingy...
            double orbAltitude = activeBody.orbit.altitude; //The current orbit altitude.
            double radiationAvrDose = 100000000000 / orbAltitude; //That first number can be changed, may it be changed as a difficulty slider?

            if (activeBody.orbit.referenceBody.name == "Sun")  //Make sure this is a planet, and not a moon!
            {
                //Calculation starts here:
                if (activeBody.name == "Sun")  //This one is required, as there is no way to do it otherway (The sun is not orbiting anything)
                {
                    groundDose = 100000000000;  //100,000,000,000 = 1 d/s (7.3 d/s on kerbin's altitude)
                    double radiationLevel = groundDose / activeAltitude;  //Simplified, needs work!!
                    if (radiationLevel == 0.00) { radiationLevel = 0.01; } //Safety First!!
                    return radiationLevel;
                }
                else
                {
                    if (hasAtmosphere == true)
                    {

                        if (atmoActualDeep <= 0)
                        {
                            activeAtmoAltitude = 0; //Uh, cause KSP may start doing unwanted stuff!
                        }
                        else
                        {
                            activeAtmoAltitude = atmoActualDeep / seaLevelAtm; //Easy, calculating how deep are you in the atmosphere
                            //(Decimal Percentage) (1 if you are at sea level)


                        }
                        groundDose = radiationAvrDose - activeAtmoAltitude; //Shall do its job (Untested)

                    }
                    else
                    {
                        groundDose = radiationAvrDose; //Expendable, for adding radiation emiting planets!
                    }


                    double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simple! May need to multiply this for more impact!
                    //Once again more atmospheric dependant calculation :P
                    if (hasAtmosphere == true)
                    {
                        radiationLevel = radiationLevel + (radiationAvrDose - (activeAtmoAltitude * radiationAvrDose));
                    }
                    else
                    {
                        radiationLevel = radiationLevel + radiationAvrDose;  //Simpler way to calculate this
                    }

                    return radiationLevel;  //And finally, get that radiation :D
                }
            }
            else //If it's a moon, do these calculations!
            {
                double hostPlanetRadiation = 100000000000 / activeBody.orbit.referenceBody.orbit.altitude; //Duh, that call 
                double myRadiation = hostPlanetRadiation; //Needs expansion!
                if (hasAtmosphere == true)
                {

                    if (atmoActualDeep <= 0)
                    {
                        activeAtmoAltitude = 0; //Uh, cause KSP may start doing unwanted stuff!
                    }
                    else
                    {
                        activeAtmoAltitude = atmoActualDeep / seaLevelAtm; //Easy, calculating how deep are you in the atmosphere
                        //(Decimal Percentage) (1 if you are at sea level)


                    }
                    groundDose = myRadiation - activeAtmoAltitude; //Shall do its job (Untested)

                }
                else
                {
                    groundDose = myRadiation; //Expandable, for adding radiation emiting planets!
                }


                double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simple! May need to multiply this for more impact!
                //Once again more atmospheric dependant calculation :P
                if (hasAtmosphere == true)
                {
                    radiationLevel = radiationLevel + (radiationAvrDose - (activeAtmoAltitude * radiationAvrDose));
                }
                else
                {
                    radiationLevel = radiationLevel + radiationAvrDose;  //Simpler way to calculate this
                }

                return radiationLevel;  //And finally, get that radiation :D

            }


            /* Sadly, all this code becomes slighty useless with the CelestialBody class D:
            if (activeBody == "Sun")  //Aparently, Kerbol is named Sun in code! (That's why we were getting bugged returns)
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
                double atmoDeep = atmoDen * alwaysDose;  //KSP gives density starting at: 1.0
                double groundDose = alwaysDose - atmoDeep;
                double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simplified, needs work!!
                radiationLevel = radiationLevel + (alwaysDose - (atmoDen * alwaysDose));
                if (radiationLevel <= 0)
                {
                    radiationLevel = 1.0;

                }
                return radiationLevel;  
                
            }
            if(activeBody == "Kerbin")
            {
                
                //Kerbin calculation almost negates radiation at ground due to protective gases
                double alwaysDose = 7.35;  //AVERAGE radiation dose on Kerbin's orbit altitude.
                double atmoDeep = atmoDen * alwaysDose;  //KSP gives density starting at: 1.0
                double groundDose = alwaysDose - atmoDeep;
                double radiationLevel = groundDose / Math.Abs(activeAltitude - activeLatitude); //Simplified, needs work!!
                radiationLevel = radiationLevel + (alwaysDose - (atmoDen * alwaysDose));
                if(radiationLevel <= 0)
                {
                    radiationLevel = 0.1;
                 
                }
                
                return radiationLevel;  
            }
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
            */
            

        }


        
    }
    
    }

    

