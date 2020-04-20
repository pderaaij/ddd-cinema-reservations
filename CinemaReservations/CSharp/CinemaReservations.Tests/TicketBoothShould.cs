﻿using CinemaReservations.Tests.StubMovieScreening;
using External.AuditoriumLayout;
using NFluent;
using NUnit.Framework;

namespace CinemaReservations.Tests
{
    [TestFixture]
    public class TicketBoothShould
    {
        [Test]
        public void Reserve_one_seat_when_available()
        {
            const string showId = "1";
            const int partyRequested = 1;

            IMovieScreeningRepository repository =  new StubMovieScreeningRepository(new StubAuditoriumRepository());
            TicketBooth ticketBooth = new TicketBooth(repository);

            var seatsAllocated = ticketBooth.AllocateSeats(new AllocateSeats(showId, partyRequested));

            Check.That(seatsAllocated.ReservedSeats).HasSize(1);
            Check.That(seatsAllocated.ReservedSeats[0].ToString()).IsEqualTo("A3");
        }

        [Test]
        public void Return_SeatsNotAvailable_when_all_seats_are_unavailable()
        {
            const string showId = "5";
            const int partyRequested = 1;

            IMovieScreeningRepository repository =  new StubMovieScreeningRepository(new StubAuditoriumRepository());
            TicketBooth ticketBooth = new TicketBooth(repository);

            var seatsAllocated = ticketBooth.AllocateSeats(new AllocateSeats(showId, partyRequested));

            Check.That(seatsAllocated.ReservedSeats).HasSize(0);
            Check.That(seatsAllocated).IsInstanceOf<NoPossibleAllocationsFound>();
        }

    }
}