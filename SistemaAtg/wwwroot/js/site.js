function timeout() {
    setTimeout(function () {

        $.ajax({
            type: "get", url: "http://localhost:61228/api/mensagem",
            success: function (data, textStatus, jqXHR) {
                console.log(data);
                $('#MsgRetorno')
                    .text(textStatus)
                    .fadeIn('slow', function () {
                        $('#MsgRetorno').delay(5000).fadeOut();
                    });
                timeout();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $('#MsgRetorno')
                    .text("Não foi possível consumir a fila remota: " + textStatus)
                    .fadeIn('slow', function () {
                        $('#MsgRetorno').delay(5000).fadeOut();
                    });
                console.log(errorThrown);
            }
        });

    }, 1000);
}

timeout();
