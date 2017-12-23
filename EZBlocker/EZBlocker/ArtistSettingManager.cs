using System.Collections.Generic;

namespace EZBlocker
{
    internal class ArtistSettingManager
    {
        public List<string> blockList; 

        public ArtistSettingManager()
        {
            blockList = Properties.Settings.Default.BlockedArtists ?? new List<string>();
        }

        /// <summary>
        /// Checks if the setting contains the specified artist
        /// </summary>
        public bool ContainsArtist(string artistName)
        {
            return blockList.Contains(artistName);
        }

        /// <summary>
        /// Adds artists tot the list, if the artist is already added it will return false
        /// </summary>
        /// <returns></returns>
        public bool TryAddArtist(string artistName)
        {
            if (ContainsArtist(artistName))
            {
                return false;
            }

            blockList.Add(artistName);
            UpdateSettings();
            return true;
        }

        /// <summary>
        /// Removes artists from the list, if the artist is not in the list it will return false
        /// </summary>
        /// <returns></returns>
        public bool TryRemoveArtist(string artistName)
        {
            if (!ContainsArtist(artistName)) {
                return false;
            }

            blockList.Remove(artistName);
            UpdateSettings();
            return true;
        }


        /// <summary>
        /// Update the setting to the C# settings
        /// </summary>
        private void UpdateSettings()
        {
            Properties.Settings.Default.BlockedArtists = blockList;
            Properties.Settings.Default.Save();
        }
    }
}
