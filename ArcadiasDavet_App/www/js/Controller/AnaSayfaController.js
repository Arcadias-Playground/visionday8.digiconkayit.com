app.controller('AnaSayfa', function($scope, $http, $timeout){
    let keypressQRData = '';

    $scope.Yukleniyor = true;
    $scope.GirisListesi = [];
    $scope.moment = moment;

    $scope.ApiURL = 'https://davet.arcadiastech.com/Api/KatilimciGiris';
    //$scope.ApiURL = 'https://localhost:44327/Api/KatilimciGiris'
    

    $scope.ScannerOptions = {
        preferFrontCamera : false, // iOS and Android
        showFlipCameraButton : false, // iOS and Android
        showTorchButton : true, // iOS and Android
        torchOn: false, // Android, launch with the torch switched on (if available)
        saveHistory: false, // Android, save scan history (default false)
        prompt : "QR kodu okutunuz", // Android
        resultDisplayDuration: 250, // Android, display scanned text for X ms. 0 suppresses it entirely, default 1500
        formats : "QR_CODE", // default: all but PDF_417 and RSS_EXPANDED
        orientation : "portrait", // Android only (portrait|landscape), default unset so it rotates with the device
        disableAnimations : true, // iOS
        disableSuccessBeep: false // iOS and Android
    };

    $scope.Scan = () => {
        $scope.Yukleniyor = true;
        cordova.plugins.barcodeScanner.scan(
            (result) => {
                const QRData = {
                    KatilimciID: result.text,
                    KullaniciID: DeviceInfo.UUID
                };
                $http({
                    method: "POST",
                    url: $scope.ApiURL,
                    data: QRData,
                }).then((response) => {
                    if(response.data.Sonuc === 3){
                        UyariBilgilendirme('', '<p class="text-center"><img src="img/tick.png" style="width:40%" /></p><p class="text-center">Kullanıcı giriş onaylandı</p><p class="text-center" style="font-size: 18px;">' + response.data.Veriler.KatilimciBilgisi.AdSoyad + '</p><p class="text-center">' + response.data.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipi  + '</p>', true);
                        
                        if($scope.GirisListesi.length === 5)
                            $scope.GirisListesi.splice(4, 1);

                        $scope.GirisListesi.unshift(response.data.Veriler);
                    }
                    else{
                        UyariBilgilendirme('', '<p class="text-center"><img src="img/Failed.png" style="width:40%;" /></p><div class="text-center">' + response.data.HataBilgi.HataMesaji + '</div>', false);
                    }

                    $scope.Yukleniyor = false;
                }, (error) => {
                    $scope.Yukleniyor = false;
                    UyariBilgilendirme('', '<p class="text-center"><img src="img/Failed.png" class="w-100" /></p><p class="text-center">Bağlantı hatası meydana geldi</p><div class="text-center">Hata mesajı : ' + error.statusText + '</div>', false);
                });
            },
            (error) => {   
                UyariBilgilendirme('', '<p class="text-center"><img src="img/Failed.png" style="width:40%;" /></p><p class="text-center">Barkod okuma hatası</p><div class="text-center">Hata mesajı : ' + error  + '</div>', false);
                $scope.Yukleniyor = false;
            },
            $scope.ScannerOptions
        );
    }

    
    $timeout(() => { 
        $http({
            method: "GET",
            url: $scope.ApiURL,
            params:{
                UUID: DeviceInfo.UUID
            }
        }).then((response) => {
            if(response.data.Sonuc === 3){
                $scope.GirisListesi = response.data.Veriler;
            }
            else{
                alert(response.data.HataBilgi.HataMesaji);
            }
            $scope.Yukleniyor = false;
        }, (error) => {
            alert(error);
        });
    }, 2000);

    window.addEventListener('keypress', (event) => {
        if(!$scope.Yukleniyor)
            $scope.$apply(() => { $scope.Yukleniyor = true; });
        
        
        if(event.key === "ENTER" || event.key === "Enter"){
            $scope.$apply(() => {
                
                console.log(keypressQRData);

                while(keypressQRData.indexOf("=") !== -1 || keypressQRData.indexOf("*") !== -1){
                    keypressQRData = keypressQRData.replace("=", "-").replace("*", "-");
                }

                console.log(keypressQRData);

                const QRData = {
                    KatilimciID: keypressQRData,
                    KullaniciID: DeviceInfo.UUID
                };

                $http({
                    method: "POST",
                    url: $scope.ApiURL,
                    data: QRData,
                }).then((response) => {
                    if(response.data.Sonuc === 3){
                        UyariBilgilendirme('', '<p class="text-center"><img src="img/tick.png" style="width:40%" /></p><p class="text-center">Kullanıcı giriş onaylandı</p><p class="text-center" style="font-size: 18px;">' + response.data.Veriler.KatilimciBilgisi.AdSoyad + '</p><p class="text-center">' + response.data.Veriler.KatilimciBilgisi.KatilimciTipiBilgisi.KatilimciTipi  + '</p>', true);
                        
                        if($scope.GirisListesi.length === 5)
                            $scope.GirisListesi.splice(4, 1);

                        $scope.GirisListesi.unshift(response.data.Veriler);
                    }
                    else{
                        UyariBilgilendirme('', '<p class="text-center"><img src="img/Failed.png" style="width:40%;" /></p><div class="text-center">' + response.data.HataBilgi.HataMesaji + '</div><div style="margin-top:10px;">Okunan Kod ==> <b><u>' + keypressQRData + '</u></b></div>', false);
                    }

                    keypressQRData = '';
                    $scope.Yukleniyor = false;
                }, (error) => {
                    UyariBilgilendirme('', '<p class="text-center"><img src="img/Failed.png" class="w-100" /></p><p class="text-center">Bağlantı hatası meydana geldi</p><div class="text-center">Hata mesajı : ' + error.statusText + '</div><div style="margin-top:10px;">Okunan Kod ==> <b><u>' + keypressQRData + '</u></b></div>', false);

                    keypressQRData = '';
                    $scope.Yukleniyor = false;
                });
            });
        }
        else{
            keypressQRData += event.key;
        }
    });
});