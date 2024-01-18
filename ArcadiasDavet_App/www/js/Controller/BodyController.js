app.controller('BodyController', function($scope) {
    $('#TarihSaat').html(moment().format('DD.MM.YYYY HH:mm:ss'));

    setInterval(function() {
        $('#TarihSaat').html(moment().format('DD.MM.YYYY HH:mm:ss'));
    }, 1000);
})