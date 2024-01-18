let cropper, Olculer = {
    x: 1,
    y: 1,
    width: 1,
    height: 1,

    IsValid: function () {
        return !isNaN(this.x) && !isNaN(this.y) && !isNaN(this.width) && !isNaN(this.height);
    },

    Reset: function (Tip) {

        switch (Tip) {
            case 'YakaKarti':
                this.x = parseInt($('.txt-ykix').val());
                this.y = parseInt($('.txt-ykiy').val());
                this.width = parseInt($('.txt-ykiwidth').val());
                this.height = parseInt($('.txt-ykiheight').val());

                break;

            default:
                this.x = parseInt($('.txt-x').val());
                this.y = parseInt($('.txt-y').val());
                this.width = parseInt($('.txt-width').val());
                this.height = parseInt($('.txt-height').val());

                break;
        }
    }
};

const toUpper = (Kume) => {
    const startIndex = Kume.selectionStart;
    const endIndex = Kume.selectionEnd;
    Kume.value = Kume.value.replace("ı", "I").replace("i", "İ").toUpperCase();
    Kume.selectionStart = startIndex;
    Kume.selectionEnd = endIndex;
}

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
    const tbl = $(Kume).DataTable({
        lengthChange: false,
        stateSave: false,
        language: dataTableLangPack.InitLangPack('tr'),
        ordering: false,
        autoWidth: false
    });

    if (Reset) {
        tbl.state.clear();
        tbl.page(0).draw('page');
    }
}

const GorselKontrol = (Kume) => {
    var img, _URL = window.URL || window.webkitURL;
    if ((file = Kume.files[0])) {
        switch (file.type) {
            case "image/png":
            case "image/jpeg":
            case "image/jpg":
                if (file.size < 3072000) {
                    img = new Image();
                    img.onload = function () {
                        if (this.width === 2000 && this.height === 2828) {
                            var reader = new FileReader();
                            reader.onload = function () {
                                $('#hfGorsel').val(reader.result);
                            };
                            reader.onerror = function () {
                                Kume.value = '';
                                $('#hfGorsel').val('');
                                AltSayfaUyariBilgilendirme('<p>Görsel kontrolü sırasında hata meydana geldi</p><p>Hata mesajı : {0}</p>'.replace("{0}", hata), false);
                            };
                            reader.readAsDataURL(file);
                        }
                        else {
                            Kume.value = '';
                            $('#hfGorsel').val('');
                            AltSayfaUyariBilgilendirme('<p>Görseliniz 2000px x 2828px olamalıdır.</p><p>Yüklemeye çalıştığınız görseliniz {0}px x {1}px ölçülerindedir.</p>'.replace("{0}", this.width).replace("{1}", this.height), false);
                        }
                    };
                    img.src = _URL.createObjectURL(file);
                }
                else {
                    Kume.value = '';
                    $('#hfGorsel').val('');
                    AltSayfaUyariBilgilendirme('<p>Görseliniz en fazla 3mb olmalıdır.</p>', false);
                }

                break;

            default:
                Kume.value = '';
                $('#hfGorsel').val('');
                AltSayfaUyariBilgilendirme('<p>Dosyanızın uzantısı png yada jpg olmalıdır.</p>', false);
                break;
        }
    }
}

const KioskGorselKontrol = (Kume) => {
    var img, _URL = window.URL || window.webkitURL;
    if ((file = Kume.files[0])) {
        switch (file.type) {
            case "image/png":
            case "image/jpeg":
            case "image/jpg":
                if (file.size < 3072000) {
                    img = new Image();
                    img.onload = function () {
                        var reader = new FileReader();
                        reader.onload = function () {
                            $('#hfKioskGorsel').val(reader.result);
                        };
                        reader.onerror = function () {
                            Kume.value = '';
                            $('#hfKioskGorsel').val('');
                            AltSayfaUyariBilgilendirme('<p>Görsel kontrolü sırasında hata meydana geldi</p><p>Hata mesajı : {0}</p>'.replace("{0}", hata), false);
                        };
                        reader.readAsDataURL(file);
                    };
                    img.src = _URL.createObjectURL(file);
                }
                else {
                    Kume.value = '';
                    $('#hfKioskGorsel').val('');
                    AltSayfaUyariBilgilendirme('<p>Görseliniz en fazla 3mb olmalıdır.</p>', false);
                }

                break;

            default:
                Kume.value = '';
                $('#hfKioskGorsel').val('');
                AltSayfaUyariBilgilendirme('<p>Dosyanızın uzantısı png yada jpg olmalıdır.</p>', false);
                break;
        }
    }
}

const KarsilamaEkraniGorselKontrol = (Kume) => {
    var img, _URL = window.URL || window.webkitURL;
    if ((file = Kume.files[0])) {
        switch (file.type) {
            case "image/png":
            case "image/jpeg":
            case "image/jpg":
                if (file.size < 2048000) {
                    img = new Image();
                    img.onload = function () {
                        var reader = new FileReader();
                        reader.onload = function () {
                            $('#hfKarsilamaEkraniGorsel').val(reader.result);
                        };
                        reader.onerror = function () {
                            Kume.value = '';
                            $('#hfKarsilamaEkraniGorsel').val('');
                            AltSayfaUyariBilgilendirme('<p>Görsel kontrolü sırasında hata meydana geldi</p><p>Hata mesajı : {0}</p>'.replace("{0}", hata), false);
                        };
                        reader.readAsDataURL(file);
                    };
                    img.src = _URL.createObjectURL(file);
                }
                else {
                    Kume.value = '';
                    $('#hfKarsilamaEkraniGorsel').val('');
                    AltSayfaUyariBilgilendirme('<p>Görseliniz en fazla 2mb olmalıdır.</p>', false);
                }

                break;

            default:
                Kume.value = '';
                $('#hfKarsilamaEkraniGorsel').val('');
                AltSayfaUyariBilgilendirme('<p>Dosyanızın uzantısı png yada jpg olmalıdır.</p>', false);
                break;
        }
    }
}

const LogoKontrol = (Kume) => {
    var img, _URL = window.URL || window.webkitURL;
    if ((file = Kume.files[0])) {
        switch (file.type) {
            case "image/png":
            case "image/jpeg":
            case "image/jpg":
                if (file.size < 1024000) {
                    img = new Image();
                    img.onload = function () {
                        var reader = new FileReader();
                        reader.onload = function () {
                            $('#hfLogo').val(reader.result);
                        };
                        reader.onerror = function () {
                            Kume.value = '';
                            $('#hfLogo').val('');
                            AltSayfaUyariBilgilendirme('<p>Görsel kontrolü sırasında hata meydana geldi</p><p>Hata mesajı : {0}</p>'.replace("{0}", hata), false);
                        };
                        reader.readAsDataURL(file);
                    };
                    img.src = _URL.createObjectURL(file);
                }
                else {
                    Kume.value = '';
                    $('#hfLogo').val('');
                    AltSayfaUyariBilgilendirme('<p>Görseliniz en fazla 2mb olmalıdır.</p>', false);
                }

                break;

            default:
                Kume.value = '';
                $('#hfLogo').val('');
                AltSayfaUyariBilgilendirme('<p>Dosyanızın uzantısı png yada jpg olmalıdır.</p>', false);
                break;
        }
    }
}

const InsertContentToTextArea = (Kume, TextArea) => {
    const dataContent = Kume.getAttribute('data-content');
    const startIndex = TextArea.selectionStart;
    const endIndex = TextArea.selectionEnd;

    TextArea.value = TextArea.value.slice(0, startIndex) + dataContent + TextArea.value.slice(endIndex);
    TextArea.selectionStart = startIndex + dataContent.length;
    TextArea.selectionEnd = TextArea.selectionStart;
    TextArea.focus();
}

const InsertContentToEditor = (Kume, Editor) => {
    CKEDITOR.instances[Editor.id].insertText(Kume.getAttribute('data-content'));
}

const YakaKartiGorselKontrol = (Kume) => {
    var img, _URL = window.URL || window.webkitURL;
    if ((file = Kume.files[0])) {
        switch (file.type) {
            case "image/png":
            case "image/jpeg":
            case "image/jpg":
                if (file.size < 2048000) {
                    img = new Image();
                    img.onload = function () {
                        var reader = new FileReader();
                        reader.onload = function () {
                            $('#hfYakaKartiGorsel').val(reader.result);
                        };
                        reader.onerror = function () {
                            Kume.value = '';
                            $('#hfYakaKartiGorsel').val('');
                            AltSayfaUyariBilgilendirme('<p>Görsel kontrolü sırasında hata meydana geldi</p><p>Hata mesajı : {0}</p>'.replace("{0}", hata), false);
                        };
                        reader.readAsDataURL(file);
                    };
                    img.src = _URL.createObjectURL(file);
                }
                else {
                    Kume.value = '';
                    $('#hfYakaKartiGorsel').val('');
                    AltSayfaUyariBilgilendirme('<p>Görseliniz en fazla 2mb olmalıdır.</p>', false);
                }

                break;

            default:
                Kume.value = '';
                $('#hfYakaKartiGorsel').val('');
                AltSayfaUyariBilgilendirme('<p>Dosyanızın uzantısı png yada jpg olmalıdır.</p>', false);
                break;
        }
    }

}

const cropperSetUp = () => {
    let image = document.getElementById('ImgAntetliKagit');
    cropper = new Cropper(image, {
        ready() {
            OlcuAyarla('');

            image.addEventListener('crop', (event) => {
                $('.txt-x').val(parseInt(event.detail.x, 0));
                $('.txt-y').val(parseInt(event.detail.y, 0));
                $('.txt-width').val(parseInt(event.detail.width, 0));
                $('.txt-height').val(parseInt(event.detail.height, 0));
            });
        },
        zoomable: false,
        viewMode: 1,
        aspectRatio: parseFloat($('.txt-oran').val().replace(",", "."))
    });
}

const cropperBadgeSetUp = () => {
    let image = document.getElementById('ImgYakaKarti');
    cropper = new Cropper(image, {
        ready() {
            OlcuAyarla('YakaKarti');

            image.addEventListener('crop', (event) => {
                $('.txt-ykix').val(parseInt(event.detail.x, 0));
                $('.txt-ykiy').val(parseInt(event.detail.y, 0));
                $('.txt-ykiwidth').val(parseInt(event.detail.width, 0));
                $('.txt-ykiheight').val(parseInt(event.detail.height, 0));
            });
        },
        zoomable: false,
        viewMode: 1,
        aspectRatio: parseFloat($('.txt-ykioran').val().replace(",", "."))
    });
}

const OlcuAyarla = (Tip) => {
    Olculer.Reset(Tip);

    if (Olculer.IsValid()) {
        cropper.setData(Olculer);
    }
    else {
        AltSayfaUyariBilgilendirme('<p>Geçersiz sayı girildi</p>', false);
    }
}