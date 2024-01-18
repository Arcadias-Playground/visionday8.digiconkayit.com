let ConState = false;
const HubCon = $.connection.yazici_Hub;

HubCon.client.yakaKartiBasmaTalebi = (YazdirmaBilgileri) => {
    window.open('YakaKarti/' + YazdirmaBilgileri.KatilimciID + '/' + YazdirmaBilgileri.ePosta, '_blank', 'width = 10, height = 10, top = 2000, left = 2000');
}

$.connection.hub.stateChanged((State) => {
    $('#span_ConnectionStatus').css('display', 'inline');
    switch (State.newState) {
        // connecting
        case 0:
            $('#span_ConnectionStatus').css('background-color', 'white');
            break;

        // connected
        case 1:
            $('#span_ConnectionStatus').css('background-color', 'darkseagreen');
            break;

        // reconnecting
        case 2:
            $('#span_ConnectionStatus').css('background-color', '#DB5C5C');
            break;

        // disconnected
        case 4:
            $('#span_ConnectionStatus').css('background-color', '#FF0000');
            break;

        default: break;
    }
});
$.connection.hub.connectionSlow(() => {
    $('#span_ConnectionStatus').css('background-color', 'orange');
});
$.connection.hub.reconnected(() => {
    $('#span_ConnectionStatus').css('background-color', 'darkseagreen');
});
$.connection.hub.disconnected(function () {
    if (!ConState) {
        if ($.connection.hub.lastError) { alert("Bağlantı beklenmeyen şekilde sonlandı. Hata mesajı : " + $.connection.hub.lastError.message); }
        setTimeout(() => {
            $.connection.hub.start({ transport: ['webSockets', 'longPolling'] }).done(() => {
                console.log('{0} - Tekrar bağlanıldı'.replace('{0}', moment().format('DD.MM.yyyy HH:mm:ss')));
            }).fail((err) => { console.log(err) });
        }, 2000);
    }
    else {
        console.log("{0} - Bağlantı sonlandırıldı".replace('{0}', moment().format('DD.MM.yyyy HH:mm:ss')));
    }
});
$.connection.hub.start({ transport: ['webSockets', 'longPolling'] }).done(() => {
    console.log('Bağlanıldı');
}).fail((err) => { console.log(err) });

window.addEventListener('beforeunload', () => {
    $.connection.hub.stop();
})