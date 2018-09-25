using System;
using System.Collections.Generic;
using System.Text;

namespace ItLabs.MBox.Contracts
{
    public static class MBoxConstants
    {
        public static int HomeItemsToDisplay = 5;
        public static int initialSkip = 0;
        public static int initialTakeTabel = 20;
        public static int initialTakeHomeLists= 25;
        public static int MaximumArtistsAllowed = 50;
        public static string DefaultArtistImage = "DefaultArtist.png";
        public static string DefaultRecordLabelImage = "DefaultRecordLabel.png";
        public static string DefaultSongImage = "DefaultSong.jpg";
        public static double MaximumImageSizeAllowed = Math.Pow(1024, 2) * 3;
    }
}
