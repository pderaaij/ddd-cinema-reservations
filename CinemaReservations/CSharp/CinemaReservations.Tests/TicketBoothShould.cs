﻿using CinemaReservations.Tests.StubMovieScreening;
using CinemaReservations.Domain;
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

        [Test]
        public void Reserve_first_possibility_for_multiple_seats_when_available()
        {
            const string showId = "3";
            const int partyRequested = 2;

            IMovieScreeningRepository repository =  new StubMovieScreeningRepository(new StubAuditoriumRepository());
            TicketBooth ticketBooth = new TicketBooth(repository);

            var seatsAllocated = ticketBooth.AllocateSeats(new AllocateSeats(showId, partyRequested));

            Check.That(seatsAllocated.ReservedSeats).HasSize(2);
            Check.That(seatsAllocated.ReservedSeats[0].ToString()).IsEqualTo("A6");
            Check.That(seatsAllocated.ReservedSeats[1].ToString()).IsEqualTo("A7");
        }

        [Test]
        public void Return_TooManyTicketsRequested_when_9_tickets_are_requested()
        {
            const string showId = "5";
            const int partyRequested = 9;

            IMovieScreeningRepository repository =  new StubMovieScreeningRepository(new StubAuditoriumRepository());
            TicketBooth ticketBooth = new TicketBooth(repository);

            var seatsAllocated = ticketBooth.AllocateSeats(new AllocateSeats(showId, partyRequested));

            Check.That(seatsAllocated.ReservedSeats).HasSize(0);
            Check.That(seatsAllocated).IsInstanceOf<TooManyTicketsRequested>();
        }

    }
}
