using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.AutoComplete.Request.Enums;
using GoogleApi.Entities.Places.Common;
using NUnit.Framework;

namespace GoogleApi.Test.Places.AutoComplete;

[TestFixture]
public class AutoCompleteTests : BaseTest
{
    [Test]
    public void PlacesAutoCompleteTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            Types = new List<RestrictPlaceType> { RestrictPlaceType.Address }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);

        var results = response.Predictions.ToArray();
        Assert.IsNotNull(results);
        Assert.IsNotEmpty(results);
        Assert.AreEqual(1, results.Length);

        var result = results.FirstOrDefault();
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Terms);
        Assert.IsNotNull(result.PlaceId);
        Assert.IsNotNull(result.StructuredFormatting);

        var description = result.Description.ToLower();
        Assert.IsTrue(description.Contains("2200"), "1");
        Assert.IsTrue(description.Contains("jagtvej"), "2");

        var matchedSubstrings = result.MatchedSubstrings.ToArray();
        Assert.IsNotNull(matchedSubstrings);
        Assert.AreEqual(3, matchedSubstrings.Length);

        var types = result.Types.ToArray();
        Assert.IsNotNull(types);
        Assert.Contains(PlaceLocationType.Route, types);
        Assert.Contains(PlaceLocationType.Geocode, types);
    }

    [Test]
    public void PlacesAutoCompleteWhenAsyncTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn"
        };

        var result = GooglePlaces.AutoComplete.QueryAsync(request).Result;
        Assert.IsNotNull(result);
        Assert.AreEqual(Status.Ok, result.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenAsyncAndCancelledTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn"
        };

        var cancellationTokenSource = new CancellationTokenSource();
        var task = GooglePlaces.AutoComplete.QueryAsync(request, cancellationTokenSource.Token);
        cancellationTokenSource.Cancel();

        var exception = Assert.Throws<OperationCanceledException>(() => task.Wait(cancellationTokenSource.Token));
        Assert.IsNotNull(exception);
        Assert.AreEqual(exception.Message, "The operation was canceled.");
    }

    [Test]
    public void PlacesAutoCompleteWhenLanguageTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            Language = Language.Danish
        };

        var response = GooglePlaces.AutoComplete.Query(request);
        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);

        var results = response.Predictions.ToArray();
        Assert.IsNotNull(results);
        Assert.IsNotEmpty(results);
        Assert.GreaterOrEqual(results.Length, 2);

        var result = results.FirstOrDefault();
        Assert.IsNotNull(result);

        var description = result.Description.ToLower();
        Assert.IsTrue(description.Contains("2200"), "1");
        Assert.IsTrue(description.Contains("jagtvej"), "2");
    }

    [Test]
    public void PlacesAutoCompleteWhenOffsetTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            Offset = "offset"
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            Location = new Coordinate(1, 1)
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationAndRadiusTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            Radius = 100
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationAndRadiusAndRegionTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            Radius = 100,
            Region = "KÝbenhavn"
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationBiasAndIpBiasTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            LocationBias = new LocationBias
            {
                IpBias = true
            }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationBiasAndPointTest()
    {
        Assert.Inconclusive("Documentation states that 'point' bias is possible, but Google returns invalid request");

        //var request = new PlacesAutoCompleteRequest
        //{
        //    Key = this.Settings.ApiKey,
        //    Input = "jagtvej 2200 KÝbenhavn",
        //    LocationBias = new LocationBias
        //    {
        //        Location = new Coordinate(55.69987296762697, 12.552359427579363)
        //    }
        //};

        //var response = GooglePlaces.AutoComplete.Query(request);

        //Assert.IsNotNull(response);
        //Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationBiasAndCircleTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            LocationBias = new LocationBias
            {
                Location = new Coordinate(1, 1),
                Radius = 1000
            }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationBiasAndRectangularTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            LocationBias = new LocationBias
            {
                Bounds = new ViewPort(new Coordinate(1, 1), new Coordinate(2, 2))
            }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationRestrictionAndCircleTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            LocationRestriction = new LocationRestriction
            {
                Location = new Coordinate(55.69987296762697, 12.552359427579363),
                Radius = 50000
            }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenLocationRestrictionAndRectangularTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            LocationRestriction = new LocationRestriction
            {
                Bounds = new ViewPort(new Coordinate(54.69987296762697, 11.552359427579363), new Coordinate(56.69987296762697, 13.552359427579363))
            }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenTypesTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "jagtvej 2200 KÝbenhavn",
            Types = new List<RestrictPlaceType> { RestrictPlaceType.Address }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenTypesCitiesTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "KÝbenhavn",
            Types = new List<RestrictPlaceType> { RestrictPlaceType.Cities }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }

    [Test]
    public void PlacesAutoCompleteWhenTypesRegionsTest()
    {
        var request = new PlacesAutoCompleteRequest
        {
            Key = this.Settings.ApiKey,
            Input = "Denmark",
            Types = new List<RestrictPlaceType> { RestrictPlaceType.Regions }
        };

        var response = GooglePlaces.AutoComplete.Query(request);

        Assert.IsNotNull(response);
        Assert.AreEqual(Status.Ok, response.Status);
    }
}