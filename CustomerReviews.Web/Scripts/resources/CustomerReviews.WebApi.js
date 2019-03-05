angular.module('CustomerReviews.Web')
.factory('CustomerReviews.WebApi', ['$resource', function ($resource) {
    return $resource('api/customerReviews', {}, {
        search: { method: 'POST', url: 'api/customerReviews/search' },
        update: { method: 'PUT' },
        searchAverageRate: { method: 'POST', url: 'api/customerReviews/searchAverageRate' }
    });
}]);
