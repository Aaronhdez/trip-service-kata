using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using TripServiceKata.Entity;

namespace TripServiceKata.Tests {
    public class TripServiceShould {
        [Test]
        public void RetrieveEmptyListIfUserIsNotAFriend() {
            TripService tripService = new TripService();
            var user = new User();

            List<Trip> tripList = tripService.GetTripsByUser(user);

            tripList.Should().BeEmpty();
        }
    }
}