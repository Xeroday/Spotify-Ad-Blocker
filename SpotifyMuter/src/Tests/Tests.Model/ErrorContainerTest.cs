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
    public class ErrorContainerTest
    {
        [TestMethod]
        public void HasError_Returns_False_If_Error_Is_Null()
        {
            // Arrange
            var status = new ErrorContainer()
            {
                Error = null,
            };

            // Act & Assert
            Assert.IsFalse(status.HasError);
        }

        [TestMethod]
        public void HasError_Returns_True_If_Error_Is_Not_Null()
        {
            // Arrange
            var status = new ErrorContainer()
            {
                Error = new Error(),
            };

            // Act & Assert
            Assert.IsTrue(status.HasError);
        }
    }
}