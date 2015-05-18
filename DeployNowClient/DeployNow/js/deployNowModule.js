angular.module('HubProxy', []);

angular.module('CommonModule', []);

angular
	.module('DeployNow', ['HubProxy', 'ui.bootstrap','CommonModule']);

