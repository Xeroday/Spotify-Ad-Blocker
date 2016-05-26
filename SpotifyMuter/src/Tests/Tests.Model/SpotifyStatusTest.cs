/* SpotifyMuter - A simple Spotify Ad Muter for Windows
 * Copyright(C) 2016 Maschmi
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see<http://www.gnu.org/licenses/>.*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;

namespace Tests.Model
{
    [TestClass]
    public class SpotifyStatusTest
    {
        [TestMethod]
        public void CanReturnFalseIfSpotifyIsNotInPrivateSession()
        {
            // Arrange
            var status = new SpotifyStatus
            {
                OpenGraphState = new OpenGraphState
                {
                    PrivateSession = false,
                },
            };

            // Act
            var isPrivateSession = status.SpotifyIsInPrivateSession;

            // Assert
            Assert.IsFalse(isPrivateSession);
        }

        [TestMethod]
        public void CanReturnTrueIfSpotifyIsInPrivateSession()
        {
            // Arrange
            var status = new SpotifyStatus
            {
                OpenGraphState = new OpenGraphState
                {
                    PrivateSession = true,
                },
            };

            // Act
            var isPrivateSession = status.SpotifyIsInPrivateSession;

            // Assert
            Assert.IsTrue(isPrivateSession);
        }
    }
}