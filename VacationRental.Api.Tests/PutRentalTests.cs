using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Models;
using Xunit;

namespace VacationRental.Api.Tests
{

    [Collection("Integration")]
    public class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRentalAndNoBooking_ThenAGetReturnsTheUpdatedRental()
        {
            var request = new RentalBindingModelV2
            {
                Units = 25,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var request2 = new RentalBindingModelV2
            {
                Units = 20,
                PreparationTimeInDays = 3
            };

            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/vacationrental/rentals/{postResult.Id}", request2))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/vacationrental/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModelV2>();
                Assert.Equal(request2.Units, getResult.Units);
                Assert.Equal(request2.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRentalAndBookingCheckDays_ThenAGetReturnsTheUpdatedRental()
        {
            var request = new RentalBindingModelV2
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", request))
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

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 2,
                Start = new DateTime(2021, 07, 24)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }


            var request2 = new RentalBindingModelV2
            {
                Units = 1,
                PreparationTimeInDays = 3
            };

            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/vacationrental/rentals/{postResult.Id}", request2))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
            }

            using (var getResponse = await _client.GetAsync($"/api/v1/vacationrental/rentals/{postResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModelV2>();
                Assert.Equal(request2.Units, getResult.Units);
                Assert.Equal(request2.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRentalAndBookingCheckUnit_ThenAPutReturnsErrorWhenThereIsOverlapping()
        {
            var request = new RentalBindingModelV2
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync($"/api/v1/vacationrental/rentals", request))
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

            using (var postBooking1Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 2,
                Start = new DateTime(2021, 07, 18)
            };

            using (var postBooking2Response = await _client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }

            var request2 = new RentalBindingModelV2
            {
                Units = 1,
                PreparationTimeInDays = 3
            };

            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/vacationrental/rentals/{postResult.Id}", request2))
                {
                }
            });
        }
    }
}
