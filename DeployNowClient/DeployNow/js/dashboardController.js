angular
	.module('DeployNow')
	.config(routeProvider);

routeProvider.$inject = ['$routeProvider'];

function routeProvider($routeProvider) {
    $routeProvider.when('/dashboard', {
        controller: 'MainController',
        templateUrl: ''
    }).otherwise({
        redirectTo: '/dashboard'
    });
};

angular
    .module('DeployNow')
    .controller('MainController', MainCtrl);

function MainCtrl(scope, dashboardService, hubProxyService) {
    scope.DashboardService = dashboardService;
    initialize();

    function initialize() {
        //hubProxyService.Initialize();
        return dashboardService.GetAllDeploymentVersions();
    }
}
MainCtrl.$inject = ['$scope', 'DashboardService', 'HubProxyService'];
