angular
	.module('CommonModule')
    .directive('displayName', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                scope.$watch(attr.displayName, function () {
                    var value = scope.$eval(attr.displayName);
                    if (value && value.length > 9) {
                        var version = value.substr(value.indexOf(value.match(/\d+/)));
                        var fileName = value.substring(value.indexOf('.') + 1).substr(0, value.indexOf('.') - 1);
                        return element.text(fileName.concat(version));
                    }
                    return element.text(value.substring(0,indexOf('.')));
                });
            }
        };
    });
