$(document).ready(function () {
    function Contains(text1, text2) {
        if (text1.indexOf(text2) != -1)
            return true
    }
    
});

$("#searchForm").submit(function (event) {
    alert("Handler for .submit() called.");
    event.preventDefault();
});