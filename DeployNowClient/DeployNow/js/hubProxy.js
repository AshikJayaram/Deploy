angular
	.module('HubProxy')
    .service('HubProxyService', hubProxyService);

function hubProxyService(rootScope) {
    return {
        Initialize: initialize,
    };

    function initialize() {
        
        var connection = $.hubConnection();
        var proxy = connection.createHubProxy('DeployNowClient.DeployNowClientHub');
        connection.logging = true;

        proxy.on('Deployment', function (message) {
            console.log('Deployment Hub proxy invoked');
            rootScope.$broadcast('DeploymentCompleted', message);
        });

        connection.disconnected(function () {
            console.log('Connection closed. Retrying...');
            setTimeout(function () {
                connection.start();
            }, 2000);
        });
        connection.start({ transport: ['serverSentEvents'] }).done(function () {
            console.log('Yes, I am connected');
        }).fail(function () {
            console.log('Could not connect');
        });
    }
}
hubProxyService.$inject = ['$rootScope'];

