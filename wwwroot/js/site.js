// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function changeRate(rate, id){
    $.ajax({
        url: '/main/ChangeRate',
        method: 'post',
        dataType: 'json',
        data: {Id: id,
               Rate: rate
        
        },
        
    });
}

function deleteTheme(id){
    $.ajax({
        url: '/Main/Delete',
        method: 'post',
        dataType: 'json',
        data: {ID: id}
    })
}

function autoGrow(el) {
    el.style.height = '5px';
    el.style.height = el.scrollHeight + 'px';
}

document.querySelector('textarea').addEventListener('input', function() { autoGrow(this); });
