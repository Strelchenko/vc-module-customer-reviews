angular.module('CustomerReviews.Web')
    .controller('CustomerReviews.Web.reviewDetailController',
        [
            '$scope', 'uiGridConstants', 'virtoCommerce.catalogModule.items', 'platformWebApp.bladeNavigationService',
            function($scope, uiGridConstants, items, bladeNavigationService) {
                $scope.uiGridConstants = uiGridConstants;

                var blade = $scope.blade;

                blade.refresh = function() {
                    blade.isLoading = false;
                    return items.get({ id: blade.currentEntity.productId },
                        function(data) {
                            blade.product = data;
                            console.log(data);
                            console.log(blade.product);
                        });
                };

                blade.openProductDetails = function() {
                    var newBlade = {
                        id: 'productDetails',
                        itemId: blade.product.id,
                        controller: 'virtoCommerce.catalogModule.itemDetailController',
                        template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                    };
                    bladeNavigationService.showBlade(newBlade, blade);
                };

                blade.title = 'customerReviews.blades.review-detail.labels.title';
                blade.headIcon = 'fa-info-circle';

                blade.refresh();


            }
        ]);