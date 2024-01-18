const DeviceInfo = {
    manufacturer:'',
    platform: '',
    model: '',
    UUID: '',
    serial: ''
};

const UyariBilgilendirme = (Baslik, Icerik, Sonuc) => {
    if (Sonuc === undefined) {
        $('#UyariHead').css('background-color', 'transparent');
        $('#UyariBaslik').css('color', '#000');
        $('#UyariKapatButon').css('display', 'none');
    }
    else {
        if (Sonuc) {
            $('#UyariHead').css('background-color', 'darkseagreen');
            $('#UyariBaslik').css('color', '#fff');
        }
        else {
            $('#UyariHead').css('background-color', '#f00');
            $('#UyariBaslik').css('color', '#fff');
        }
        $('#UyariKapatButon').css('display', 'inline-block');
    }


    $('#UyariBaslik').html(Baslik);
    $('#UyariIcerik').html(Icerik);
    $('#Uyari').modal('show');
}

document.addEventListener('deviceready', onDeviceReady, false);

function onDeviceReady() {
    DeviceInfo.manufacturer = device.manufacturer;
    DeviceInfo.platform = device.platform;
    DeviceInfo.model = device.model;
    DeviceInfo.UUID = device.uuid;
    DeviceInfo.serial = device.serial;

    $('#UUID').html('Seri No : ' + DeviceInfo.UUID);
}