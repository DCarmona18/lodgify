﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetVacationsRentalCalendarTests
    {
        private readonly HttpClient _client;

        public GetVacationsRentalCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModelV2
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", postRentalRequest))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postResult.Id,
                 Nights = 2,
                 Start = new DateTime(2021, 07, 18)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 2,
                Start = new DateTime(2021, 07, 20)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getCalendarResponse = await _client.GetAsync($"/api/v1/vacationrental/calendar?rentalId={postResult.Id}&start=2021-07-18&nights=5"))
            {
                Assert.True(getCalendarResponse.IsSuccessStatusCode);

                var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<VacationsRentalCalendarViewModel>();
                
                Assert.Equal(postResult.Id, getCalendarResult.RentalId);
                Assert.Equal(5, getCalendarResult.Dates.Count);
                
                Assert.Equal(new DateTime(2021, 07, 18), getCalendarResult.Dates[0].Date);
                Assert.Single(getCalendarResult.Dates[0].Bookings);
                Assert.Empty(getCalendarResult.Dates[0].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);

                Assert.Equal(new DateTime(2021, 07, 19), getCalendarResult.Dates[1].Date);
                Assert.Single(getCalendarResult.Dates[1].Bookings);
                Assert.Empty(getCalendarResult.Dates[1].PreparationTimes);
                Assert.Contains(getCalendarResult.Dates[1].Bookings, x => x.Id == postBooking1Result.Id);
                
                Assert.Equal(new DateTime(2021, 07, 20), getCalendarResult.Dates[2].Date);
                Assert.Single(getCalendarResult.Dates[2].Bookings);
                Assert.Contains(getCalendarResult.Dates[2].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Single(getCalendarResult.Dates[2].PreparationTimes);

                Assert.Equal(new DateTime(2021, 07, 21), getCalendarResult.Dates[3].Date);
                Assert.Single(getCalendarResult.Dates[3].Bookings);
                Assert.Contains(getCalendarResult.Dates[3].Bookings, x => x.Id == postBooking2Result.Id);
                Assert.Single(getCalendarResult.Dates[3].PreparationTimes);

                Assert.Equal(new DateTime(2021, 07, 22), getCalendarResult.Dates[4].Date);
                Assert.Empty(getCalendarResult.Dates[4].Bookings);
                Assert.Single(getCalendarResult.Dates[4].PreparationTimes);
            }
        }
    }
}
