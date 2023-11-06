function showTooltip() {
    var cellSpecialSymbol = $("span[data-price]");
    var currentPrice = cellSpecialSymbol.attr('data-price').replace(/,/g, '.');

    var selectedCurrencyValue = $("#dropdown-list option:selected").val();

    $.ajax({
        type: 'GET',
        url: '/DataTable/ExchangeCurrency',
        data: { price: currentPrice, selectedCurrencyAbbreviation: selectedCurrencyValue },
        success: function (data) {
            $("#price-cell").attr('title', data);
        }
    });
}



