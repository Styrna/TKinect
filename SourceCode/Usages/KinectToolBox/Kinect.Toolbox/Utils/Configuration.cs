using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kinect.Toolbox.Utils
{
    public static class Configuration
    {
        //Activators of the analyzers
        public static bool headNavigation { get; set; }
        public static bool handNavigation { get; set; }
        public static bool swipeNavigation { get; set; }
        public static bool dualSwipeNavigation { get; set; }
        public static bool postureNavigation { get; set; }
        public static bool movementNavigation { get; set; }


 


        /// <summary>
        /// HandNavigationAnalyzer
        /// </summary>
        /// 

        //max hand length detected 
        public static double maxHandLength = 0.4;

        //minimal valute to active the hand calibrates itslef ser only initial value
        public static double activeHandDistance = 0.25;

        //CONST radius from max hand length to active
        public static double activePart = 0.6;

        //CONST analyze skipped after hand activated for a sability
        public static int framesSkipped = 20;

        //CONST analyze one out of n calls to reduce traffic
        public static int callsSkipped = 0;

        //CONST minimal disntace betteen hands to be even considered a change
        public static double minimalHandDistanceChanged = 0.07;

        //CONST minial period between gestures to allow rise event
        public static int minimalPeriodBetweenGestures = 250;
    }

    
}
