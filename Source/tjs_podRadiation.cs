using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace tjs_radMod
{


    class tjs_podRadiation : PartModule //A class for checking the radiation of individual pods (Manned)
    {
        //Variables
        [KSPField(isPersistant = true)]
        public double radAmmount = 0;  //Internal, manages how much radiation is applied to a certin pod (Before maths)
        [KSPField(isPersistant = false)]
        public double radShield = 1;  //Configurable, is a DIVISOR of radAmmount, basically, shield ammount.
        [KSPField(isPersistant = true)]
        public double radShieldExtra = 0; //Internal, adds to radShield. Is dependant of protective fluid on the part
        private int safetyWait = 25; //Ammount of ticks to wait before the radiation stuff starts to happen (Safety first)
        private int safetyWaited = 0; //Ammount of ticks that have been already waited :P

        private string activePlanet = "NO_DATA"; //Active SOI. Set to "NO_DATA" to not damage crew on load.
        private double altitude = 0; //Used in radiation calculation.
        private double latitude = 0; //There tends to be more radiation at the poles.
        private double atmoDensity = 0;
        //onStart:
        public override void OnStart(StartState state)  //Runs when the part spawns
        {
            
            if(HighLogic.LoadedSceneIsFlight == true)  //Check whenever the part is on flight
            {
                
                Debug.Log("[Radiation Manager] Pod radiation manager started!");
                Debug.Log("[Radiation Manager] Make sure i'm not starting before physics load!");



            }

        }

        public override void OnUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight == true)
            {
                if (safetyWait == safetyWaited)
                {
                    //All code goes here!


                    activePlanet = vessel.orbit.referenceBody.bodyName;
                    altitude = vessel.altitude;
                    latitude = vessel.latitude;
                    atmoDensity = vessel.atmDensity;
                    radAmmount = radManager.getRadiationLevel(activePlanet, altitude, latitude, atmoDensity);
                    Debug.Log("[Spam] Vessel atmospheric density: " + atmoDensity);
                    Debug.Log("[More Useless Spam] Vessel radiation level is: " + radAmmount.ToString());
                    

                }
                else  //Safety first!
                {

                    safetyWaited = safetyWaited + 1;

                }
            }

        }

        //Simple function, it does some magic and converts three input values into the final radiation dose!
        //The magic behind this: radiationAmmount / (radiationShield + radiationShieldExtra)

        public float cRadiation(float radiationAmmount, float radiationShield, float radiationShieldExtra)
        {
            float finRadiation = radiationAmmount / (radiationShield + radiationShieldExtra);
            return finRadiation;  
        }




    }
}
