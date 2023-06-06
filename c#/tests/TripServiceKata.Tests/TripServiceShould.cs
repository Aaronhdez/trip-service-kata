using System;
using FluentAssertions;
using NUnit.Framework;
using TripServiceKata.Entity;
using TripServiceKata.Exception;

namespace TripServiceKata.Tests {
    public class TripServiceShould {
        [Test]
        public void ACharacterizationTest() {
            var tripService = new TripService();
            var user = new User();

            Action result = () => tripService.GetTripsByUser(user);

            result.Should().Throw<UserNotLoggedInException>();
        }
    }
}