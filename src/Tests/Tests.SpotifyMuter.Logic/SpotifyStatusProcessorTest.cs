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
using Moq;
using SpotifyMuter.Logic;

namespace Tests.SpotifyMuter.Logic
{
    [TestClass]
    public class SpotifyStatusProcessorTest
    {
        private Mock<ISpotifyMuter> _mockSpotifyMuter;
        private SpotifyStatusProcessor _spotifyStatusProcessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockSpotifyMuter = new Mock<ISpotifyMuter>();
            _spotifyStatusProcessor = new SpotifyStatusProcessor(_mockSpotifyMuter.Object);
        }

        [TestMethod]
        public void DoesNotMuteOrUnmuteIfAdIsPaused()
        {
            // Arrange
            var status = new Mock<ISpotifyStatus>();

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            _mockSpotifyMuter.Verify(muter => muter.Mute(), Times.Never);
            _mockSpotifyMuter.Verify(muter => muter.Unmute(), Times.Never);
        }

        [TestMethod]
        public void DoesNotMuteOrUnmuteIfSongIsPaused()
        {
            // Arrange
            var status = new Mock<ISpotifyStatus>();
            status.Setup(s => s.NextEnabled)
                  .Returns(true);

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            _mockSpotifyMuter.Verify(muter => muter.Mute(), Times.Never);
            _mockSpotifyMuter.Verify(muter => muter.Unmute(), Times.Never);
        }

        [TestMethod]
        public void DoesNotMuteOrUnmuteIfError()
        {
            // Arrange
            var status = new Mock<ISpotifyStatus>();
            status.Setup(s => s.HasError)
                  .Returns(true);

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            _mockSpotifyMuter.Verify(muter => muter.Mute(), Times.Never);
            _mockSpotifyMuter.Verify(muter => muter.Unmute(), Times.Never);
        }

        [TestMethod]
        public void DoesNotMuteOrUnmuteIfPrivateSession()
        {
            // Arrange
            var status = new Mock<ISpotifyStatus>();
            status.Setup(s => s.SpotifyIsInPrivateSession)
                  .Returns(true);

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            _mockSpotifyMuter.Verify(muter => muter.Mute(), Times.Never);
            _mockSpotifyMuter.Verify(muter => muter.Unmute(), Times.Never);
        }

        [TestMethod]
        public void CanMuteIfAdIsPlaying()
        {
            // Arrange
            var status = GetStatusWhichWillBeMuted();

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            _mockSpotifyMuter.Verify(muter => muter.Mute(), Times.Once);
            _mockSpotifyMuter.Verify(muter => muter.Unmute(), Times.Never);
        }

        [TestMethod]
        public void CanUnmuteIfSongIsPlaying()
        {
            // Arrange
            var status = GetStatusWhichWillBeUnmuted();

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            _mockSpotifyMuter.Verify(muter => muter.Mute(), Times.Never);
            _mockSpotifyMuter.Verify(muter => muter.Unmute(), Times.Once);
        }

        [TestMethod]
        public void CanRaiseSpotifyMutedIfSpotifyWasMuted()
        {
            // Arrange
            var spotifyMutedWasCalled = false;
            _spotifyStatusProcessor.SpotifyMuted += (sender, e) => spotifyMutedWasCalled = true;

            var status = GetStatusWhichWillBeMuted();

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            Assert.IsTrue(spotifyMutedWasCalled);
        }

        [TestMethod]
        public void CanRaiseSpotifyUnmutedIfSpotifyWasUnmuted()
        {
            // Arrange
            var spotifyUnmutedWasCalled = false;
            _spotifyStatusProcessor.SpotifyUnmuted += (sender, e) => spotifyUnmutedWasCalled = true;

            var status = GetStatusWhichWillBeUnmuted();

            // Act
            _spotifyStatusProcessor.ProcessSpotifyStatus(status.Object);

            // Assert
            Assert.IsTrue(spotifyUnmutedWasCalled);
        }

        private static Mock<ISpotifyStatus> GetStatusWhichWillBeMuted()
        {
            var status = new Mock<ISpotifyStatus>();
            status.Setup(s => s.Playing)
                  .Returns(true);

            return status;
        }

        private static Mock<ISpotifyStatus> GetStatusWhichWillBeUnmuted()
        {
            var status = new Mock<ISpotifyStatus>();
            status.Setup(s => s.Playing)
                  .Returns(true);
            status.Setup(s => s.NextEnabled)
                  .Returns(true);
            status.Setup(s => s.Track)
                  .Returns(new Track());

            return status;
        }
    }
}