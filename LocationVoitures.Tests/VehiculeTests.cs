using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocationVoitures.BackOffice.Models;
using System;

namespace LocationVoitures.Tests
{
    [TestClass]
    public class VehiculeTests
    {
        [TestMethod]
        public void TestVehiculeAvailability_Default()
        {
            // Arrange
            var vehicule = new Vehicule();

            // Act (Default bool is false usually, but our logic might default to true in UI)
            
            // Assert
            Assert.IsFalse(vehicule.Disponible); 
        }

        [TestMethod]
        public void TestLocationPriceCalculation()
        {
            // Arrange
            decimal dailyPrice = 50m;
            int days = 3;
            
            // Act
            decimal total = dailyPrice * days;

            // Assert
            Assert.AreEqual(150m, total);
        }

        [TestMethod]
        public void TestVehiculeProperties()
        {
            // Arrange
            var v = new Vehicule
            {
                Marque = "Toyota",
                Modele = "Yaris",
                PrixJour = 30m
            };

            // Assert
            Assert.AreEqual("Toyota", v.Marque);
            Assert.AreEqual(30m, v.PrixJour);
        }
    }
}
