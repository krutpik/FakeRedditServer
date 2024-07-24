function changeRate(rate, id){
    $.ajax({
        url: '/main/RateChange',
        method: 'post',
        dataType: 'json',
        data: {
            Id: id,
            dataRate: rate,
            __RequestVerificationToken: document.getElementById("RequestVerificationToken").value,
        
        },
    });
}

window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
});

function autoGrow(el) {
    el.style.height = '5px';
    el.style.height = el.scrollHeight + 'px';
}

document.querySelector('textarea').addEventListener('input', function() { autoGrow(this); });
