angular.module('CustomerReviews.Web')
    .controller('CustomerReviews.Web.reviewsListController',
        [
            '$scope', 'CustomerReviews.WebApi', 'platformWebApp.bladeUtils', 'uiGridConstants',
            'platformWebApp.uiGridHelper', 'platformWebApp.bladeNavigationService',
            function($scope, reviewsApi, bladeUtils, uiGridConstants, uiGridHelper, bladeNavigationService) {
                $scope.uiGridConstants = uiGridConstants;

                var blade = $scope.blade;

                blade.refresh = function() {
                    blade.isLoading = true;
                    reviewsApi.search(angular.extend(filter,
                            {
                                searchPhrase: filter.keyword ? filter.keyword : undefined,
                                sort: uiGridHelper.getSortExpression($scope),
                                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                                take: $scope.pageSettings.itemsPerPageCount
                            }),
                        function(data) {
                            blade.isLoading = false;
                            $scope.pageSettings.totalItems = data.totalCount;
                            blade.currentEntities = data.results;
                        });
                };

                blade.selectNode = function(data) {
                    var newBlade = {
                        id: 'reviewDetails',
                        currentEntity: data,
                        controller: 'CustomerReviews.Web.reviewDetailController',
                        template: 'Modules/$(CustomerReviews.Web)/Scripts/blades/review-detail.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade);
                };

                blade.delete = function(data) {
                    var ids = [data.id];
                    reviewsApi.remove({ ids: ids }, blade.refresh);
                };

                blade.headIcon = 'fa-comments';

                blade.toolbarCommands = [
                    {
                        name: "platform.commands.refresh",
                        icon: 'fa fa-refresh',
                        executeMethod: blade.refresh,
                        canExecuteMethod: function() {
                            return true;
                        }
                    }
                ];

                // simple and advanced filtering
                var filter = $scope.filter = blade.filter || {};

                filter.criteriaChanged = function() {
                    if ($scope.pageSettings.currentPage > 1) {
                        $scope.pageSettings.currentPage = 1;
                    } else {
                        blade.refresh();
                    }
                };

                // ui-grid
                $scope.setGridOptions = function(gridOptions) {
                    uiGridHelper.initialize($scope,
                        gridOptions,
                        function(gridApi) {
                            uiGridHelper.bindRefreshOnSortChanged($scope);
                        });
                    bladeUtils.initializePagination($scope);
                };

            }
        ]);