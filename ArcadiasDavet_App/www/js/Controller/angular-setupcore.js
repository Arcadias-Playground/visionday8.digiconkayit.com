const app = angular.module('ArcadiasDavet', ['ngRoute', 'ngSanitize']);

app.config(($routeProvider) => {
    $routeProvider
        .when('/', {
            controller: 'AnaSayfa',
            templateUrl: 'Views/AnaSayfa.html'
        });
});