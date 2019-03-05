angular.module('CustomerReviews.Web')
    .controller('CustomerReviews.Web.averageProductRateWidgetController',
        [
            '$scope', 'CustomerReviews.WebApi',
            function($scope, reviewsApi) {
                var blade = $scope.blade;
                var filter = { take: 1 };

                function refresh() {
                    $scope.loading = true;
                    reviewsApi.searchAverageRate(filter,
                        function(data) {
                            $scope.loading = false;

                            $scope.rate = data.results[0].averageRate;
                        });
                }

                $scope.$watch("blade.itemId",
                    function(id) {
                        filter.productIds = [id];

                        if (id) refresh();
                    });
            }
        ]);