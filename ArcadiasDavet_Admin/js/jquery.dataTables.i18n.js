const dataTableLangPack = {
	tr: {
		"sDecimal": ",",
		"sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
		"sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
		"sInfoEmpty": "Kayıt yok",
		"sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
		"sInfoPostFix": "",
		"sInfoThousands": ".",
		"sLengthMenu": "Sayfada _MENU_ kayıt göster",
		"sLoadingRecords": "Yükleniyor...",
		"sProcessing": "İşleniyor...",
		"sSearch": "Ara:",
		"sZeroRecords": "Eşleşen kayıt bulunamadı",
		"oPaginate": {
			"sFirst": "İlk",
			"sLast": "Son",
			"sNext": "Sonraki",
			"sPrevious": "Önceki"
		},
		"oAria": {
			"sSortAscending": ": artan sütun sıralamasını aktifleştir",
			"sSortDescending": ": azalan sütun sıralamasını aktifleştir"
		},
		"select": {
			"rows": {
				"_": "%d kayıt seçildi",
				"0": "",
				"1": "1 kayıt seçildi"
			}
		}
	},
	en: {
		"sEmptyTable": "No data available in table",
		"sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
		"sInfoEmpty": "Showing 0 to 0 of 0 entries",
		"sInfoFiltered": "(filtered from _MAX_ total entries)",
		"sInfoPostFix": "",
		"sInfoThousands": ",",
		"sLengthMenu": "Show _MENU_ entries",
		"sLoadingRecords": "Loading...",
		"sProcessing": "Processing...",
		"sSearch": "Search:",
		"sZeroRecords": "No matching records found",
		"oPaginate": {
			"sFirst": "First",
			"sLast": "Last",
			"sNext": "Next",
			"sPrevious": "Previous"
		},
		"oAria": {
			"sSortAscending": ": activate to sort column ascending",
			"sSortDescending": ": activate to sort column descending"
		}
	},
	fr: {
		"sEmptyTable": "Aucune donnée disponible dans le tableau",
		"sInfo": "Affichage de l'élément _START_ à _END_ sur _TOTAL_ éléments",
		"sInfoEmpty": "Affichage de l'élément 0 à 0 sur 0 élément",
		"sInfoFiltered": "(filtré à partir de _MAX_ éléments au total)",
		"sInfoPostFix": "",
		"sInfoThousands": ",",
		"sLengthMenu": "Afficher _MENU_ éléments",
		"sLoadingRecords": "Chargement...",
		"sProcessing": "Traitement...",
		"sSearch": "Rechercher :",
		"sZeroRecords": "Aucun élément correspondant trouvé",
		"oPaginate": {
			"sFirst": "Premier",
			"sLast": "Dernier",
			"sNext": "Suivant",
			"sPrevious": "Précédent"
		},
		"oAria": {
			"sSortAscending": ": activer pour trier la colonne par ordre croissant",
			"sSortDescending": ": activer pour trier la colonne par ordre décroissant"
		},
		"select": {
			"rows": {
				"_": "%d lignes sélectionnées",
				"0": "Aucune ligne sélectionnée",
				"1": "1 ligne sélectionnée"
			}
		}
	},
	InitLangPack: function (langCode) {
		return this[langCode];
	}
}