$(function () {
    $('.sidebar').hide();
    $('#page-wrapper').css({ 'margin-left': '0px' });
    
    //const urlAPI = "http://localhost:60718/api/railroadport";
    const urlAPI = "http://172.23.191.14/IntegraPsTaCockpit/Tu_RailRoadReport_O_RailRoadReportList";
    var DataAPI;
    var LastDataAPI;
    var fieldsUpdated = [];
    var screenExpanded = true;
    var i = 1;

    function getDataAPI() {
        var jqxhr =
        $.ajax({
            url: urlAPI,
            dataType: "json",
            async: false,
        })
        .success(function (response, data, xhr) {          
            LastDataAPI = DataAPI;
            DataAPI = response;
        })
        .fail(function (error) {
            console.log(error);
        });
    }

    function checkZeros(id, content) {
        //Verifica se o horário está zerado
        if ($(id).html() == content) {
            $(id).html("");
        }
    }

    function checkEntreLotes(status,id) {
        if (status == "Entre Lotes") {
            $(id).html("");
        }
    }

    function blink(id, time) {
        while(time > 0) {
            setTimeout(function () {
                $(id).fadeTo(300, 0.1).fadeTo(300, 1);
            }, time * 00);
            time--;
        }
    }

    function checkMudancaItem(indice, atributo) {
        //Problema de atributo faltando
        if (DataAPI[indice] != null && LastDataAPI[indice]) {
            if (DataAPI[indice][atributo] != LastDataAPI[indice][atributo]) {
                blink('#VV0' + (indice + 1) + atributo, 3);
            }
        }
    }

    function checkMudancas() {
        DataAPI.forEach(function (e, i) {
            checkMudancaItem(i, "Status");
            checkMudancaItem(i, "hPreparar");
            checkMudancaItem(i, "hDuracaoRolar");
            checkMudancaItem(i, "hLivrar");
            checkMudancaItem(i, "hEntreLotes");
            checkMudancaItem(i, "hPrevTermino");
            checkMudancaItem(i, "hRetira");
            checkMudancaItem(i, "NDescarregado");
            checkMudancaItem(i, "NVagao");
            checkMudancaItem(i, "NCheio");
            checkMudancaItem(i, "hDuracaoRetirar");
            checkMudancaItem(i, "hPreparar");          
            checkMudancaItem(i, "hRolar");
            checkMudancaItem(i, "hf");
            checkMudancaItem(i, "hi");
        });
    }

    function mapTable1() {
        for (var i = 1; i < 6; i++) {
            //console.log(DataAPI[i - 1].Status);

            //Troca Status
            if (DataAPI[i - 1].Status != $('#VV0' + i + 'Status').html()) {
                if (DataAPI[i - 1].Status == "Parado") {
                    $('#VV0' + i + 'Status').removeClass();
                    $('#VV0' + i + 'Status').addClass("warning-gray");
                }
                if (DataAPI[i - 1].Status == "Desconhecido") {
                    $('#VV0' + i + 'Status').removeClass();
                    $('#VV0' + i + 'Status').addClass("warning-black");
                }
                if (DataAPI[i - 1].Status == "Manutenção") {
                    $('#VV0' + i + 'Status').removeClass();
                    $('#VV0' + i + 'Status').addClass("warning-red");
                }
                if (DataAPI[i - 1].Status == "Entre Lotes") {
                    $('#VV0' + i + 'Status').removeClass();
                    $('#VV0' + i + 'Status').addClass("warning-blue");
                }
                if (DataAPI[i - 1].Status == "Operando") {
                    $('#VV0' + i + 'Status').removeClass();
                    $('#VV0' + i + 'Status').addClass("warning-green");
                }
                if (DataAPI[i - 1].Status == "Primeiro Posicionamento" || DataAPI[i - 1].Status == "Finalizando Descarga" || DataAPI[i - 1].Status == "Último Posicionamento") {
                    $('#VV0' + i + 'Status').removeClass();
                    $('#VV0' + i + 'Status').addClass("warning-yellow");
                }
                $('#VV0' + i + 'Status').addClass("text-center");
            }
            $('#VV0' + i + 'Status').html(DataAPI[i - 1].Status);

            //Tempo entre Lotes
            if (DataAPI[i - 1].CorEntreLotes == 1) {
                $('#VV0' + i + 'hEntreLotes').removeClass();
                $('#VV0' + i + 'hEntreLotes').addClass("warning-time-red");
                $('#VV0' + i + 'hEntreLotes').addClass("text-center");
            } else {
                $('#VV0' + i + 'hEntreLotes').removeClass();
                $('#VV0' + i + 'hEntreLotes').addClass("text-center");
            }
            $('#VV0' + i + 'hEntreLotes').html(DataAPI[i - 1].hEntreLotes.substring(0, 8));
            checkZeros('#VV0' + i + 'hEntreLotes', "00:00:00");

            //Tempo Prep. Cheias
            if (DataAPI[i - 1].CorPreparar == 1) {
                $('#VV0' + i + 'hPreparar').removeClass();
                $('#VV0' + i + 'hPreparar').addClass("warning-time-red");
                $('#VV0' + i + 'hPreparar').addClass("text-center");
            } else {
                $('#VV0' + i + 'hPreparar').removeClass();
                $('#VV0' + i + 'hPreparar').addClass("text-center");
            }
            $('#VV0' + i + 'hPreparar').html(DataAPI[i - 1].hPreparar.substring(0, 8));
            checkZeros('#VV0' + i + 'hPreparar', "00:00:00");

            //Horário cheias
            $('#VV0' + i + 'hRolar').html(DataAPI[i - 1].hRolar.substring(0, 5));
            checkZeros('#VV0' + i + 'hRolar', "00:00");

            $('#VV0' + i + 'hDuracaoRolar').html(DataAPI[i - 1].hDuracaoRolar.substring(0, 8));
            checkZeros('#VV0' + i + 'hDuracaoRolar', "00:00:00");

            //Início Desc.
            $('#VV0' + i + 'hi').html(DataAPI[i - 1].hi.substring(0, 5));
            checkZeros('#VV0' + i + 'hi', "00:00");

            //Qt. Vagões
            $('#VV0' + i + 'NVagao').html(DataAPI[i - 1].NVagao);
            checkEntreLotes(DataAPI[i - 1].Status, '#VV0' + i + 'NVagao');

            //Qt. Desc.
            $('#VV0' + i + 'NDescarregado').html(DataAPI[i - 1].NDescarregado);
            checkEntreLotes(DataAPI[i - 1].Status, '#VV0' + i + 'NDescarregado');

            //Qt. Rest.
            $('#VV0' + i + 'NCheio').html(DataAPI[i - 1].NCheio);
            checkEntreLotes(DataAPI[i - 1].Status, '#VV0' + i + 'NCheio');

            //Prev. término
            // Se for menor que 15 destacar de amarelo
            $('#VV0' + i + 'hPrevTermino').html(DataAPI[i - 1].hPrevTermino.substring(0, 8));
            if ($('#VV0' + i + 'hPrevTermino').html().substring(3, 5) < 15 && $('#VV0' + i + 'hPrevTermino').html().substring(0, 2) == "00" && $('#VV0' + i + 'hPrevTermino').html() != "00:00:00") {
                $('#VV0' + i + 'hPrevTermino').removeClass();
                $('#VV0' + i + 'hPrevTermino').addClass("warning-yellow");
                $('#VV0' + i + 'hPrevTermino').addClass("text-center");
            } else {
                $('#VV0' + i + 'hPrevTermino').removeClass("warning-yellow");
            }
            checkZeros('#VV0' + i + 'hPrevTermino', "00:00:00");

            //Término Desc.	
            $('#VV0' + i + 'hf').html(DataAPI[i - 1].hf.substring(0, 5));
            checkZeros('#VV0' + i + 'hf', "00:00");

            //Tempo retirar
            if (DataAPI[i - 1].CorDuracaoRetirar == 1) {
                $('#VV0' + i + 'hDuracaoRetirar').removeClass();
                $('#VV0' + i + 'hDuracaoRetirar').addClass("warning-time-red");
                $('#VV0' + i + 'hDuracaoRetirar').addClass("text-center");
            } else {
                $('#VV0' + i + 'hDuracaoRetirar').removeClass();
                $('#VV0' + i + 'hDuracaoRetirar').addClass("text-center");
            }
            $('#VV0' + i + 'hDuracaoRetirar').html(DataAPI[i - 1].hDuracaoRetirar.substring(0, 8));
            checkZeros('#VV0' + i + 'hDuracaoRetirar', "00:00:00");

                        
        }
    }

    function mapTable2() {

        for (var i = 1; i < 6; i++) {
            $('#VV0' + i + 'hLivrar').html(DataAPI[i - 1].hLivrar.substring(0, 8));
            checkZeros('#VV0' + i + 'hLivrar', "00:00:00");

            $('#VV0' + i + 'hEntreLotesAnt').html(DataAPI[i - 1].hEntreLotesAnt.substring(0, 8));
            checkZeros('#VV0' + i + 'hEntreLotesAnt', "00:00:00");

            $('#VV0' + i + 'hPrepararAnt').html(DataAPI[i - 1].hPrepararAnt.substring(0, 8));
            checkZeros('#VV0' + i + 'hPrepararAnt', "00:00:00");

            $('#VV0' + i + 'hRolarAnt').html(DataAPI[i - 1].hRolarAnt.substring(0, 5));
            checkZeros('#VV0' + i + 'hRolarAnt', "00:00");

            $('#VV0' + i + 'hDuracaoRolarAnt').html(DataAPI[i - 1].hDuracaoRolarAnt.substring(0, 8));
            checkZeros('#VV0' + i + 'hDuracaoRolarAnt', "00:00:00");

            $('#VV0' + i + 'hiAnt').html(DataAPI[i - 1].hiAnt.substring(0, 5));
            checkZeros('#VV0' + i + 'hiAnt', "00:00");

            $('#VV0' + i + 'NVagaoAnt').html(DataAPI[i - 1].NVagaoAnt);
            checkZeros('#VV0' + i + 'NVagaoAnt', "0");

            $('#VV0' + i + 'hfAnt').html(DataAPI[i - 1].hfAnt.substring(0, 5));
            checkZeros('#VV0' + i + 'hfAnt', "00:00");

            $('#VV0' + i + 'hDuracaoRetirarAnt').html(DataAPI[i - 1].hDuracaoRetirarAnt.substring(0, 8));
            checkZeros('#VV0' + i + 'hDuracaoRetirarAnt', "00:00:00");

            $('#VV0' + i + 'hRetiraAnt').html(DataAPI[i - 1].hRetiraAnt.substring(0, 5));
            checkZeros('#VV0' + i + 'hRetiraAnt', "00:00");

            //if (DataAPI[i - 1].CorDuracaoRolar == 1) {
            //    $('#VV0' + i + 'hDuracaoRolar').removeClass();
            //    $('#VV0' + i + 'hDuracaoRolar').addClass("warning-time-red");
            //    $('#VV0' + i + 'hDuracaoRolar').addClass("text-center");
            //} else {
            //    $('#VV0' + i + 'hDuracaoRolar').removeClass();
            //    $('#VV0' + i + 'hDuracaoRolar').addClass("text-center");
            //}
            //$('#VV0' + i + 'hDuracaoRolar').html(DataAPI[i - 1].hDuracaoRolar.substring(0, 8));
            //checkZeros('#VV0' + i + 'hDuracaoRolar', "00:00:00");

            //$('#VV0' + i + 'hRetira').html(DataAPI[i - 1].hRetira.substring(0, 5));
            //checkZeros('#VV0' + i + 'hRetira', "00:00"); 
        }
    }

    function atualizaRelogio() {
        if (DataAPI[0]['horaAtual'].substring(0, 5) != $('#clock').html())
            $('#clock').html(DataAPI[0]['horaAtual'].substring(0, 5));
    }

    getDataAPI();
    LastDataAPI = DataAPI;
    if (DataAPI[4] != null) {
        checkZeros();
        mapTable1();
        mapTable2();
        checkMudancas();
        atualizaRelogio();
    } else {
        //console.log('dados em alteração');
        getDataAPI();
    }
     
    
    //Contador: Atualizando em 5,4,3,2,1...
    setInterval(function () {
        i--;
        if (i < 1) {
            checkMudancas();
            getDataAPI();
            if (DataAPI[0] != null || DataAPI[1] != null || DataAPI[2] != null || DataAPI[3] != null || DataAPI[4] != null) {
                checkZeros();
                mapTable1();
                mapTable2();
                atualizaRelogio();
            } else {
                console.error('Erro na Atualização da tabela');
                getDataAPI();
            }
            i = 5;
        }
        $('#counter').html(i);
    },1000);

    //Toggle Expand Screen
    $('#expandScreen').click(function () {
        if (!screenExpanded) {
            $('.sidebar').hide();
            $('#page-wrapper').css({ 'margin-left': '0px' });
            $('#expandScreen i').removeClass().addClass('fa fa-compress fa-2x');
            screenExpanded = true;
        } else {
            $('.sidebar').show();
            $('#page-wrapper').css({ 'margin-left': '250px' });
            $('#expandScreen i').removeClass().addClass('fa fa-expand fa-2x');
            screenExpanded = false;
        }
    });
})