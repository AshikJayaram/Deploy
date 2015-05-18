angular
	.module('DeployNow')
    .service('DashboardService', dashboardService);

dashboardService.$inject = ['$http', '$filter', '$dialog', '$rootScope', '$location'];

function dashboardService(http, filter, $dialog, $rootScope, location) {
    var dashboardModel = DashboardModel;
    $rootScope.ActiveTemplate = "html/Dashboard.html";
    return {
        DashboardModel: dashboardModel,
        GetAllDeploymentVersions: getAllDeploymentVersions,
        GetVersionForRegion: getVersionForRegion,
        PromoteForDeployment: promoteForDeployment,
        GetFiles: getFiles,
        ConfirmToPromote: confirmToPromote
    };

    function getAllDeploymentVersions() {
        $rootScope.ActiveTemplate = "html/Dashboard.html";
        $rootScope.TasksToComplete = [];
        location.path('/dashboard');
        http({ method: 'GET', url: '/api/GetFileDetails' })
            .success(function (data) {
                _.each(data, function (file) {
                    file.FileName = file.FileName.substr(0, file.FileName.lastIndexOf('.'));
                });
                dashboardModel.BuildVersions = getFiles(data, "BUILDS");
                dashboardModel.DevVersions = getFiles(data, "DEV");
                dashboardModel.STVersions = getFiles(data, "ST");
                dashboardModel.UATVersions = getFiles(data, "UAT");
                dashboardModel.PreProdVersions = getFiles(data, "PREPROD");
                dashboardModel.ProdVersions = getFiles(data, "PROD");

            });
    }

    function getVersionForRegion(region, files) {
        http({ method: 'GET', url: '/api/GetFileDetails?Region=' + region })
                .success(function (data) {
                    files = data;
                });
    }

    function promoteForDeployment(fileName, region) {
        $rootScope.RegionForDeploy = region;
        $rootScope.FileToDeploy = fileName;
        confirmToPromote();
    }

    function getFiles(files, reg) {
        _.each(files, function (file) {
            file.CreationTime = filter('date')(new Date(file.CreationTime), 'dd/MMM/yyyy HH:mm:ss');
        });
        return _.where(files, { Region: reg });
    }

    function confirmToPromote() {
        var dialogService = $dialog.dialog({
            dialogFade: false,
            backdrop: true,
            keyboard: false,
            backdropClick: false,
            templateUrl: 'html/PromoteConfirmation.html',
            controller: 'ConfirmationController'
        });
        dialogService.open().then(function (promoteCode) {
            callTask(promoteCode);
        });

    }
    function callTask(promoteCode) {
        var executeTask = {
            'Promote': function () {
                var codeForDeployment = { FileName: $rootScope.FileToDeploy + ".zip", Region: $rootScope.RegionForDeploy };
                navigate();
                http({ method: 'GET', url: '/api/ServerStop?region=' + $rootScope.RegionForDeploy })
                    .success(function () {
                        $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "IIS Stopped successfully." });
                        $rootScope.CompletionStatus = 'width-25';
                        http({ method: 'POST', url: '/api/PromoteCode', data: codeForDeployment })
                            .success(function (data) {
                                $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "Code deployed successfully." });
                                $rootScope.CompletionStatus = 'width-50';
                                http({ method: 'GET', url: '/api/ClearCache?region=' + $rootScope.RegionForDeploy })
                                    .success(function () {
                                        $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "Cleared cache successfully." });
                                        $rootScope.CompletionStatus = 'width-75';
                                        http({ method: 'GET', url: '/api/ServerStart?region=' + $rootScope.RegionForDeploy })
                                            .success(function () {
                                                $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "Started IIS successfully." });
                                                $rootScope.CompletionStatus = 'width-100';
                                            }).error(function () {
                                                $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "IIS restart unsuccessful." });
                                            });
                                    }).error(function () {
                                        $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "Cache clear unsuccessful." });
                                    });
                            }).error(function () {
                                $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "Code deployment unsuccessful." });
                            });
                    }).error(function () {
                        $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "IIS stop unsuccessful." });
                    });
                //http({ method: 'GET', url: '/api/GetFileDetails'})
                //    .success(function () {
                //        $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "IIS Stopped successfully." });
                //        $rootScope.CompletionStatus = 'width-25';
                //        http({ method: 'GET', url: '/api/GetFileDetails' })
                //            .success(function (data) {
                //                $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "Code deployed successfully." });
                //                $rootScope.CompletionStatus = 'width-50';
                //                http({ method: 'GET', url: '/api/GetFileDetails' })
                //                    .success(function () {
                //                        $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "Cleared cache successfully." });
                //                        $rootScope.CompletionStatus = 'width-75';
                //                        http({ method: 'GET', url: '/api/GetFileDetails' })
                //                            .success(function () {
                //                                $rootScope.TasksToComplete.push({ TaskCss: 'checkmark', TaskName: "Started IIS successfully." });
                //                                $rootScope.CompletionStatus = 'width-100';
                //                            }).error(function () {
                //                                $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "IIS restart unsuccessful." });
                //                            });
                //                    }).error(function () {
                //                        $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "Cache clear unsuccessful." });
                //                    });
                //            }).error(function () {
                //                $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "Code deployment unsuccessful." });
                //            });
                //    }).error(function () {
                //        $rootScope.TasksToComplete.push({ TaskCss: 'abort-icon', TaskName: "IIS stop unsuccessful." });
                //    });
            },
            'StopIIS': function () {
                http({ method: 'GET', url: '/api/ServerStop?region=' + $rootScope.RegionForDeploy })
                    .success(function () {
                        navigate();
                    });
            },
            'ClearCache': function () {
                http({ method: 'GET', url: '/api/ClearCache?region=' + $rootScope.RegionForDeploy })
                    .success(function () {
                        navigate();
                    });
            },
            'StartIIS': function () {
                http({ method: 'GET', url: '/api/ServerStart?region=' + $rootScope.RegionForDeploy })
                    .success(function () {
                        navigate();
                    });
            }
        };
        executeTask[promoteCode]();
    }
    function navigate() {
        $rootScope.ActiveTemplate = "html/DeploymentProgress.html";
        location.path('/deployment');
        launchIutTests();
    }
    
    function launchIutTests() {
        var reg = "http://10.157.84.4";
        http({ method: 'GET', url: '/api/Iut?region=' + reg })
                    .success(function () {
                        console.log("Tests completed");
                    });
    }

}
