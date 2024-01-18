const UyariBilgilendirme = (Baslik, Icerik, Sonuc) => {
    $(document).ready(function () {
        if (Sonuc === undefined) {
            $('#UyariHead').css('background-color', 'transparent');
            $('#UyariBaslik').css('color', '#000');
            $('#btn_UyariKapat').css('display', 'none');
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
            $('#btn_UyariKapat').css('display', 'inline-block');
        }


        $('#UyariBaslik').html(Baslik);
        $('#UyariIcerik').html(Icerik);
        $('#Uyari').modal('show');
    });
}

const AltSayfaUyariBilgilendirme = (Icerik, Sonuc) =>  {
    $.notify(
        {
            message: Icerik
        },
        {
            type: Sonuc ? 'alert-success' : 'alert-danger',
            allow_dismiss: true,
            newest_on_top: true,
            timer: 2000,
            placement: {
                from: 'bottom',
                align: 'center'
            },
            animate: {
                enter: 'animated fadeInDown',
                exit: 'animated fadeOutUp'
            },
            template: '<div data-notify="container" class="bootstrap-notify-container alert alert-dismissible {0}" role="alert" style="z-index:9999">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<span data-notify="icon"></span> ' +
                '<span data-notify="title">{1}</span> ' +
                '<span data-notify="message">{2}</span>' +
                '<div class="progress" data-notify="progressbar">' +
                '<div class="progress-bar progress-bar-{0}" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 0%;"></div>' +
                '</div>' +
                '<a href="{3}" target="{4}" data-notify="url"></a>' +
                '</div>'
        }
    );
}

const DataTableKurulum = (Kume, Reset) => {
    var tbl = $(Kume).DataTable({
        lengthChange: false,
        stateSave: true,
        language: dataTableLangPack.InitLangPack('tr')
    });

    if (Reset) {
        tbl.state.clear();
        tbl.page(0).draw('page');
    }
}