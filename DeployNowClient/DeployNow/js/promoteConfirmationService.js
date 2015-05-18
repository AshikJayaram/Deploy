/// <reference path="../../Scripts/ui-bootstrap-0.5.0.js" />
/// <reference path="../../Scripts/CommonLibrary.js" />
angular
	.module('DeployNow')
    .service('ConfirmationService', confirmationService);


function confirmationService() {
    this.Region = "";
    this.dialog = {};

    this.CallTask = function (taskToPerform) {
        this.dialog.close(taskToPerform);
    };

    this.Close = function () {
        this.dialog.close();
    };
    return this;
};

angular
    .module('DeployNow')
    .controller('ConfirmationController', ConfirmationCtrl);

function ConfirmationCtrl(scope, dialog, confirmationService) {
    scope.ConfirmationServiceModel = confirmationService;
    scope.ConfirmationServiceModel.dialog = dialog;
}
ConfirmationCtrl.$inject = ['$scope', 'dialog', 'ConfirmationService'];


