$(function () {

    //Variables
    var i = 10;
    //var isTyping = false;

    var optionsTable = {

        ordering: false,
        searching: true,
        paging: true,
        info: false,
        iDisplayLength: 10,
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    };

    var table = $('#tTable').DataTable(optionsTable);

    //Contador: Atualizando em 5,4,3,2,1...    
    setInterval(function () {
        if ($('#tTable_filter').find('label').find('input').val() == "") {
            i--;
        }

        //if (i < 1) {
        //    location.reload();
        //    i = 5;
        //}
        $('.counter').html(i);
    }, 1000);
})