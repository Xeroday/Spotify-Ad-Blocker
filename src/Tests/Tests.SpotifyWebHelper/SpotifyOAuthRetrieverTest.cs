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
using Moq;
using SpotifyWebHelper;
using SpotifyWebHelper.Exceptions;
using Utilities;

namespace Tests.SpotifyWebHelper
{
    [TestClass]
    public class SpotifyOAuthRetrieverTest
    {
        private SpotifyOAuthRetriever _spotifyOAuthRetriever;
        private Mock<IJsonPageLoader> _stubJsonPageLoader;

        [TestInitialize]
        public void TestInitialize()
        {
            _stubJsonPageLoader = new Mock<IJsonPageLoader>();
            _spotifyOAuthRetriever = new SpotifyOAuthRetriever(_stubJsonPageLoader.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(RetrieveOAuthException))]
        public void RetrieveOAuthThrowsRetrieveOAuthExceptionIfPageLoaderThrowsJsonPageLoadingFailedException()
        {
            // Arrange
            _stubJsonPageLoader.SetupJsonPageLoaderToThrowException();

            // Act
            _spotifyOAuthRetriever.RetrieveSpotifyOAuth();
        }

        [TestMethod]
        public void RetrieveOAuthReturnsOAuth()
        {
            // Arrange
            const string token = "randomTokenText";
            _stubJsonPageLoader.SetupJsonPageLoaderToReturnJson("{\"t\": \"" + token + "\"}");

            // Act
            var result = _spotifyOAuthRetriever.RetrieveSpotifyOAuth();

            // Assert
            Assert.AreEqual(result.Token, token);
        }
    }
}